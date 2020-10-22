using MDA.StateBackend.RDBMS.Shared;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MDA.StateBackend.MySql
{
    public class DbParameterProvider
    {
        private static readonly Dictionary<string, List<IDbDataParameter>> Cache = new Dictionary<string, List<IDbDataParameter>>();

        public static List<IDbDataParameter> GetDbParameters<T>(T it, string cacheKey = null, IReadOnlyDictionary<string, string> nameMap = null)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
            {
                return ReflectionParameters(it, nameMap);
            }

            if (Cache.TryGetValue(cacheKey, out var parameters))
                return parameters;

            Cache[cacheKey] = ReflectionParameters(it, nameMap);

            return Cache[cacheKey];
        }

        private static List<IDbDataParameter> ReflectionParameters<T>(T it, IReadOnlyDictionary<string, string> nameMap = null)
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
