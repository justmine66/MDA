using System;

namespace MDA.Domain.Models
{
    public static class TypeExtensions
    {
        public static bool IsAggregateRoot(this Type type) => !type.IsAbstract &&
                                                              !type.IsInterface &&
                                                              typeof(IAggregateRoot).IsAssignableFrom(type);

        public static Type GetAggregateRootIdType(this Type type)
        {
            if (!type.IsAggregateRoot())
            {
                throw new NotSupportedException($"The {type.FullName} not a aggregate root.");
            }

            foreach (var property in type.GetProperties())
            {
                if (property.Name == nameof(IAggregateRoot.Id))
                {
                    return property.PropertyType;
                }
            }

            throw new NotSupportedException();
        }
    }
}