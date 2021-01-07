using MDA.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace EBank.ApiServer.Infrastructure
{
    /// <summary>
    /// 可枚举集合扩展方法列表
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 数据塑性
        /// 应用场景：同样的接口在前端不同的场景下需要返回不一样的字段数据。
        /// </summary>
        /// <typeparam name="T">数据源条目类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="properties">塑形的属性列表</param>
        /// <returns>已塑形的数据</returns>
        public static IEnumerable<ExpandoObject> Shaping<T>(this IEnumerable<T> source, params string[] properties)
        {
            if (source.IsEmpty())
            {
                throw new ArgumentNullException(nameof(source));
            }

            var objectList = new List<ExpandoObject>(source.Count());
            var propertyInfoList = new List<PropertyInfo>();

            if (properties == null || properties.Length <= 0)
            {
                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                foreach (var property in properties)
                {
                    var propertyName = property.Trim();
                    var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null)
                    {
                        continue;
                    }

                    propertyInfoList.Add(propertyInfo);
                }
            }

            foreach (var t in source)
            {
                var obj = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var value = propertyInfo.GetValue(t);

                    ((IDictionary<string, object>)obj).Add(propertyInfo.Name, value);
                }

                objectList.Add(obj);
            }

            return objectList;
        }
    }
}
