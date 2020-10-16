using MDA.Shared.DataStructures;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace MDA.Shared.Types
{
    public class CachedTypeResolver : ITypeResolver
    {
        private readonly ConcurrentDictionary<string, Type> _typeCache = new ConcurrentDictionary<string, Type>();
        private readonly CachedReadConcurrentDictionary<string, Assembly> _assemblyCache = new CachedReadConcurrentDictionary<string, Assembly>();

        public Type ResolveType(string name) 
            => TryResolveType(name, out var result) ? result : null;

        public bool TryResolveType(string name, out Type type)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("A FullName must not be null nor consist of only whitespace.", nameof(name));
            if (TryGetCachedType(name, out type)) return true;
            if (!TryPerformUncachedTypeResolution(name, out type)) return false;

            AddTypeToCache(name, type);

            return true;
        }

        protected virtual bool TryPerformUncachedTypeResolution(string name, out Type type)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (!TryPerformUncachedTypeResolution(name, out type, assemblies)) return false;

            if (type.Assembly.ReflectionOnly)
                throw new InvalidOperationException($"Type resolution for {name} yielded reflection-only type.");

            return true;
        }

        private bool TryGetCachedType(string name, out Type result)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("type name was null or whitespace");

            return _typeCache.TryGetValue(name, out result);
        }

        private void AddTypeToCache(string name, Type type)
        {
            var entry = _typeCache.GetOrAdd(name, _ => type);

            if (!ReferenceEquals(entry, type)) 
                throw new InvalidOperationException("inconsistent type name association");
        }

        private bool TryPerformUncachedTypeResolution(string fullName, out Type type, Assembly[] assemblies)
        {
            if (null == assemblies) 
                throw new ArgumentNullException(nameof(assemblies));
            if (string.IsNullOrWhiteSpace(fullName)) 
                throw new ArgumentException("A type name must not be null nor consist of only whitespace.", nameof(fullName));

            if (TryResolveFromAllAssemblies(fullName, out type, assemblies)) 
                return true;

            type = Type.GetType(fullName, false) ?? Type.GetType(
                       fullName,
                       ResolveAssembly,
                       ResolveType,
                       false);
            return type != null;

            Assembly ResolveAssembly(AssemblyName assemblyName)
            {
                var fullAssemblyName = assemblyName.FullName;

                if (_assemblyCache.TryGetValue(fullAssemblyName, out var result)) 
                    return result;

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var name = assembly.GetName();

                    _assemblyCache[name.FullName] = assembly;
                    _assemblyCache[name.Name] = assembly;
                }

                return _assemblyCache.TryGetValue(fullAssemblyName, out result) ? result : null;
            }

            Type ResolveType(Assembly asm, string name, bool ignoreCase)
            {
                if (TryResolveFromAllAssemblies(name, out var result, assemblies)) 
                    return result;

                return asm?.GetType(name, false, ignoreCase) ?? 
                       Type.GetType(name, false, ignoreCase);
            }
        }

        private static bool TryResolveFromAllAssemblies(string fullName, out Type type, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                type = assembly.GetType(fullName, false);

                if (type != null)
                {
                    return true;
                }
            }

            // For types in an MDA namespace, allow remapping the assembly to another assembly.
            // This is in order to support migration from version 1.x to 2.x, during which assemblies
            // were split and renamed.
            if (fullName.StartsWith("MDA.", StringComparison.Ordinal))
            {
                var asmSeparator = fullName.LastIndexOf(',');
                if (asmSeparator > -1)
                {
                    var shortName = fullName.Substring(0, asmSeparator).Trim();
                    foreach (var assembly in assemblies)
                    {
                        type = assembly.GetType(shortName, false);
                        if (type != null)
                        {
                            return true;
                        }
                    }
                }
            }

            type = null;

            return false;
        }
    }
}
