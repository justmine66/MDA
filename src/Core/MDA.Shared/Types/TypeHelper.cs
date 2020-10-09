using System;
using System.ComponentModel;

namespace MDA.Shared.Types
{
    public class TypeHelper
    {
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
    }
}
