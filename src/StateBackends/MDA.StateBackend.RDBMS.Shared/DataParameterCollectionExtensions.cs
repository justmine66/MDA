using System.Collections.Generic;
using System.Data;

namespace MDA.StateBackend.RDBMS.Shared
{
    public static class DataParameterCollectionExtensions
    {
        public static void AddRange(this IDataParameterCollection collection, List<IDbDataParameter> parameters)
        {
            if (collection == null || 
                parameters == null)
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                collection.Add(parameter);
            }
        }
    }
}
