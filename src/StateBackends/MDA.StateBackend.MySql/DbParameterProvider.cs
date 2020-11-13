using MDA.StateBackend.RDBMS.Shared;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MDA.StateBackend.MySql
{
    public class DbParameterProvider
    {
        public static List<IDbDataParameter> ReflectionParameters<T>(T it, IReadOnlyDictionary<string, string> nameMap = null)
        {
            var properties = it.GetType().GetProperties();
            var parameters = new List<IDbDataParameter>();

            foreach (var t in properties)
            {
                var property = t;
                var value = property.GetValue(it, null);
                var parameter = new MySqlParameter
                {
                    Value = value ?? DBNull.Value,
                    Direction = ParameterDirection.Input,
                    ParameterName = nameMap != null && nameMap.ContainsKey(t.Name)
                        ? nameMap[property.Name]
                        : t.Name,
                    DbType = DbExtensions.DbTypeMap[property.PropertyType]
                };

                parameters.Add(parameter);
            }

            return parameters;
        }
    }
}
