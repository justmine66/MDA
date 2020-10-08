using System;
using System.Data;
using System.Linq.Expressions;

namespace MDA.StateBackend.RDBMS.Shared
{
    /// <summary>
    /// This interceptor bypasses some Oracle specifics.
    /// </summary>
    public class OracleCommandInterceptor : ICommandInterceptor
    {
        public static readonly ICommandInterceptor Instance = new OracleCommandInterceptor();

        private readonly Lazy<Action<IDbDataParameter>> _setClobOracleDbTypeAction;
        private readonly Lazy<Action<IDbDataParameter>> _setBlobOracleDbTypeAction;
        private readonly Lazy<Action<IDbCommand>> _setCommandBindByNameAction;

        private OracleCommandInterceptor()
        {
            _setClobOracleDbTypeAction = new Lazy<Action<IDbDataParameter>>(() => BuildSetOracleDbTypeAction("Clob"));
            _setBlobOracleDbTypeAction = new Lazy<Action<IDbDataParameter>>(() => BuildSetOracleDbTypeAction("Blob"));
            _setCommandBindByNameAction = new Lazy<Action<IDbCommand>>(BuildSetBindByNameAction);
        }

        /// <summary>
        /// Creates a compiled lambda which sets the BindByName property on OracleCommand to true.
        /// </summary>
        /// <returns>An action which takes a OracleCommand as IDbCommand </returns>
        private Action<IDbCommand> BuildSetBindByNameAction()
        {
            var type = Type.GetType("Oracle.ManagedDataAccess.Client.OracleCommand, Oracle.ManagedDataAccess");

            var parameterExpression = Expression.Parameter(typeof(IDbCommand), "command");

            var castExpression = Expression.Convert(parameterExpression, type);

            var booleanConstantExpression = Expression.Constant(true);

            var setMethod = type.GetProperty("BindByName")?.GetSetMethod();

            var callExpression = Expression.Call(castExpression, setMethod ?? throw new InvalidOperationException(), booleanConstantExpression);

            return Expression.Lambda<Action<IDbCommand>>(callExpression, parameterExpression).Compile();
        }

        /// <summary>
        /// Creates a compiled lambda which sets the OracleDbType property to the specified <paramref name="enumName"/>
        /// </summary>
        /// <param name="enumName">String value of a OracleDbType enum value.</param>
        /// <returns>An action which takes a OracleParameter as IDbDataParameter.</returns>
        private Action<IDbDataParameter> BuildSetOracleDbTypeAction(string enumName)
        {
            var type = Type.GetType("Oracle.ManagedDataAccess.Client.OracleParameter, Oracle.ManagedDataAccess");

            var parameterExpression = Expression.Parameter(typeof(IDbDataParameter), "dbparameter");

            var castExpression = Expression.Convert(parameterExpression, type ?? throw new InvalidOperationException());

            var enumType = Type.GetType("Oracle.ManagedDataAccess.Client.OracleDbType, Oracle.ManagedDataAccess");

            var clob = Enum.Parse(enumType ?? throw new InvalidOperationException(), enumName);

            var enumConstantExpression = Expression.Constant(clob, enumType);

            var setMethod = type.GetProperty("OracleDbType")?.GetSetMethod();

            var callExpression = Expression.Call(castExpression, setMethod ?? throw new InvalidOperationException(), enumConstantExpression);

            return Expression.Lambda<Action<IDbDataParameter>>(callExpression, parameterExpression).Compile();
        }

        public void Intercept(IDbCommand command)
        {
            foreach (IDbDataParameter commandParameter in command.Parameters)
            {
                //By default oracle binds parameters by index not name
                //The property BindByName must be set to true to change the default behaviour
                _setCommandBindByNameAction.Value(command);

                //String parameters are mapped to NVarChar2 OracleDbType which is limited to 4000 bytes
                //This sets the OracleType explicitly to CLOB
                if (commandParameter.ParameterName == "PayloadJson")
                {
                    _setClobOracleDbTypeAction.Value(commandParameter);
                    continue;
                }

                //Same like above
                if (commandParameter.ParameterName == "PayloadXml")
                {
                    _setClobOracleDbTypeAction.Value(commandParameter);
                    continue;
                }

                //Byte arrays are mapped as RAW which causes problems
                //This sets the OracleDbType explicitly to BLOB
                if (commandParameter.ParameterName == "PayloadBinary")
                {
                    _setBlobOracleDbTypeAction.Value(commandParameter);
                    continue;
                }

                //Oracle doesn´t support DbType.Boolean, instead
                //we map these to DbType.Int32
                if (commandParameter.DbType == DbType.Boolean)
                {
                    commandParameter.Value = commandParameter.ToString() == bool.TrueString ? 1 : 0;
                    commandParameter.DbType = DbType.Int32;
                }
            }
        }
    }
}
