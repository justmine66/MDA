using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MDA.Shared.Mappers
{
    /// <summary>
    /// 对象端口适配器
    /// 使用表达式树和静态泛型缓存技术，获得接近原生的映射速度。
    /// </summary>
    /// <typeparam name="TInput">输入对象映射类型</typeparam>
    /// <typeparam name="TOutput">输出对象映射类型</typeparam>
    public class ObjectPortMapper<TInput, TOutput>
    {
        private static readonly Func<TInput, TOutput> DoMap;

        static ObjectPortMapper()
        {
            var input = Expression.Parameter(typeof(TInput), "source");
            var memberBindingList = new List<MemberBinding>();

            foreach (var sinkProperty in typeof(TOutput).GetProperties())
            {
                if (!sinkProperty.CanWrite)
                {
                    continue;
                }

                var sourceProperty = typeof(TInput).GetProperty(sinkProperty.Name);
                if (sourceProperty == null)
                {
                    continue;
                }

                var sourcePropertyExpr = Expression.Property(input, sourceProperty);
                var memberBinding = Expression.Bind(sinkProperty, sourcePropertyExpr);

                memberBindingList.Add(memberBinding);
            }

            foreach (var sinkField in typeof(TOutput).GetFields())
            {
                var sourceField = typeof(TInput).GetField(sinkField.Name);
                if (sourceField == null)
                {
                    continue;
                }

                var sourceFieldExpr = Expression.Field(input, sourceField);
                var memberBinding = Expression.Bind(sinkField, sourceFieldExpr);

                memberBindingList.Add(memberBinding);
            }

            var memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOutput)), memberBindingList);
            var lambda = Expression.Lambda<Func<TInput, TOutput>>(memberInitExpression, input);

            DoMap = lambda.Compile();
        }

        public static TOutput Map(TInput t) => DoMap(t);
    }
}