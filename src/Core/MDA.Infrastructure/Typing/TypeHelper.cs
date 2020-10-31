using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace MDA.Infrastructure.Typing
{
    public class TypeHelper
    {
        public static readonly string[] ListTypeStrings = { "List`1", "HashSet`1", "IList`1", "ISet`1", "ICollection`1", "IEnumerable`1" };

        public static readonly string[] DicTypeStrings = { "Dictionary`2", "IDictionary`2" };

        private static readonly object SyncRoot = new object();

        private static readonly ConcurrentDictionary<string, TypeInfo> InstanceCache = new ConcurrentDictionary<string, TypeInfo>();

        public static T ConvertType<T>(object source)
        {
            if (source == null)
            {
                return default;
            }

            var sourceType = source.GetType();
            var sinkType = typeof(T);

            var sinkConverter = TypeDescriptor.GetConverter(sinkType);
            if (sinkConverter.CanConvertFrom(sourceType) &&
                sinkConverter.ConvertFrom(sourceType) is T sink1)
            {
                return sink1;
            }

            var sourceConverter = TypeDescriptor.GetConverter(sourceType);
            if (sourceConverter.CanConvertTo(sinkType) &&
                sourceConverter.ConvertTo(source, sinkType) is T sink2)
            {
                return sink2;
            }

            return Convert.ChangeType(source, sinkType) is T sink3 ? sink3 : default;
        }

        /// <summary>
        /// 添加或获取实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static TypeInfo GetOrAddInstance(Type type, string methodName = "Add")
        {
            if (type.IsInterface)
            {
                throw new Exception("服务方法中不能包含接口内容！");
            }

            if (!type.IsClass) return null;

            var fullName = type.FullName + methodName;

            var typeInfo = InstanceCache.GetOrAdd(fullName, (v) =>
            {
                Type[] argsTypes = null;

                if (type.IsGenericType)
                {
                    argsTypes = type.GetGenericArguments();
                    type = type.GetGenericTypeDefinition().MakeGenericType(argsTypes);
                }

                var mi = type.GetMethod(methodName);

                return new TypeInfo()
                {
                    Type = type,
                    MethodInfo = mi,
                    ArgTypes = argsTypes
                };
            });

            typeInfo.Instance = Activator.CreateInstance(type);

            return typeInfo;
        }

        public static TypeInfo GetOrAddInstance(Type type, MethodInfo mb)
        {
            lock (SyncRoot)
            {
                if (type.IsInterface)
                {
                    throw new Exception("服务方法中不能包含接口内容！");
                }

                var fullName = type.FullName + mb.Name;

                var typeInfo = InstanceCache.GetOrAdd(fullName, (v) => new TypeInfo()
                {
                    Type = type,
                    MethodInfo = mb
                });

                typeInfo.Instance = Activator.CreateInstance(type);

                return typeInfo;
            }
        }

        public class TypeInfo
        {
            public Type Type { get; set; }

            public object Instance { get; set; }

            public Type[] ArgTypes { get; set; }

            public MethodInfo MethodInfo { get; set; }
        }
    }
}
