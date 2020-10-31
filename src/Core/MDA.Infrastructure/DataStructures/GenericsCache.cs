using System;
using System.Collections.Generic;

namespace MDA.Infrastructure.DataStructures
{
    public class GenericCacheBase
    {
        protected static readonly List<Action> ClearCacheActions = new List<Action>();

        public static void ClearAllCache()
        {
            foreach (var action in ClearCacheActions)
            {
                action.Invoke();
            }

            ClearCacheActions.Clear();
        }
    }

    public class GenericCache<TModel> : GenericCacheBase where TModel : class
    {
        private static TModel CacheData { get; set; }

        public static void Set(TModel data) => CacheData = data;

        public static TModel Get() => CacheData;

        public static TModel GetOrSet(Func<TModel> dataAccessor)
        {
            if (dataAccessor == null)
            {
                throw new ArgumentNullException(nameof(dataAccessor));
            }

            if (CacheData != null) return CacheData;

            CacheData = dataAccessor.Invoke();
            ClearCacheActions.Add(() => CacheData = null);

            return CacheData;
        }

        public static void ClearCache()
        {
            CacheData = null;
        }
    }
}