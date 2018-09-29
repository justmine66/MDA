using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MDA.Disruptor.Internal
{
    internal static class StructProxy
    {
        private static readonly ModuleBuilder _moduleBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(nameof(StructProxy) + ".DynamicAssembly"), AssemblyBuilderAccess.Run)
            .DefineDynamicModule(nameof(StructProxy));

        private static readonly Dictionary<Type, Type> _proxyTypes = new Dictionary<Type, Type>();

        public static TInterface CreateProxyInstance<TInterface>(TInterface target)
        {
            var targetType = target.GetType();

            if (targetType.IsValueType)
                return target;

            Type proxyType;
            lock (_proxyTypes)
            {
                if (!_proxyTypes.TryGetValue(targetType, out proxyType))
                {
                    proxyType = GenerateStructProxyType(targetType);
                    _proxyTypes.Add(targetType, proxyType);
                }
            }

            if (!typeof(TInterface).IsAssignableFrom(proxyType))
                return target;

            return (TInterface)Activator.CreateInstance(proxyType, target);
        }

        private static Type GenerateStructProxyType(Type targetType)
        {
            if (!targetType.IsVisible)
            {
                return null;
            }

            var typeBuilder = _moduleBuilder.DefineType($"{nameof(StructProxy)}_{targetType.Name}_{Guid.NewGuid():N}", TypeAttributes.Public, targetType);
            var fieldBuilder = typeBuilder.DefineField("_target", targetType, FieldAttributes.Private);

            GenerateConstructor(targetType, typeBuilder, fieldBuilder);

            var interfaceTypes = targetType.GetInterfaces().Where(x => x.IsVisible);
            foreach (var interfaceType in interfaceTypes)
            {
                GenerateInterfaceImplementation(interfaceType, targetType, typeBuilder, fieldBuilder);
            }

            return typeBuilder.CreateTypeInfo();
        }

        private static void GenerateConstructor(Type targetType, TypeBuilder typeBuilder, FieldBuilder field)
        {
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { targetType });

            var constructorGenerator = constructor.GetILGenerator();
            constructorGenerator.Emit(OpCodes.Ldarg_0);
            constructorGenerator.Emit(OpCodes.Ldarg_1);
            constructorGenerator.Emit(OpCodes.Stfld, field);
            constructorGenerator.Emit(OpCodes.Ret);
        }

        private static void GenerateInterfaceImplementation(Type interfaceType, Type targetType,
            TypeBuilder typeBuilder, FieldBuilder field)
        {
            typeBuilder.AddInterfaceImplementation(interfaceType);
            var interfaceMap = targetType.GetInterfaceMap(interfaceType);

            for (int index = 0; index < interfaceMap.InterfaceMethods.Length; index++)
            {
                var interfaceMethodInfo = interfaceMap.InterfaceMethods[index];
                var targetMethodInfo = interfaceMap.TargetMethods[index];
                var parameters = interfaceMethodInfo.GetParameters();

                var methodBuilder = typeBuilder.DefineMethod(interfaceMethodInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final, interfaceMethodInfo.ReturnType, parameters.Select(x => x.ParameterType).ToArray());

                if (targetMethodInfo.IsGenericMethod)
                {
                    var genericArguments = targetMethodInfo.GetGenericArguments();
                    methodBuilder.DefineGenericParameters(genericArguments.Select((x, i) => $"T{i}").ToArray());
                }

                methodBuilder.SetImplementationFlags(methodBuilder.GetMethodImplementationFlags() | MethodImplAttributes.AggressiveInlining);

                var methodGenerator = methodBuilder.GetILGenerator();
                methodGenerator.Emit(OpCodes.Ldarg_0);
                methodGenerator.Emit(OpCodes.Ldfld, field);

                for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                {
                    methodGenerator.Emit(OpCodes.Ldarg_S, (byte)parameterIndex + 1);
                }

                methodGenerator.Emit(OpCodes.Call, targetMethodInfo);
                methodGenerator.Emit(OpCodes.Ret);
            }
        }
    }
}
