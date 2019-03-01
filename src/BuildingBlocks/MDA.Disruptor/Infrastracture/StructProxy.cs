﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MDA.Disruptor.Infrastracture
{
    public static class StructProxy
    {
        private static readonly ModuleBuilder ModuleBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(nameof(StructProxy) + ".DynamicAssembly"), AssemblyBuilderAccess.Run).DefineDynamicModule(nameof(StructProxy));

        private static readonly Dictionary<Type, Type> ProxyTypes = new Dictionary<Type, Type>();

        public static TInterface CreateProxyInstance<TInterface>(TInterface target)
        {
            var targetType = target.GetType();

            if (targetType.IsValueType)
                return target;

            Type proxyType;
            lock (ProxyTypes)
            {
                if (!ProxyTypes.TryGetValue(targetType, out proxyType))
                {
                    proxyType = GenerateStructProxyType(targetType);
                    ProxyTypes.Add(targetType, proxyType);
                }
            }

            if (!typeof(TInterface).IsAssignableFrom(proxyType))
                return target;

            return (TInterface)Activator.CreateInstance(proxyType, target);
        }

        private static Type GenerateStructProxyType(Type targetType)
        {
            if (!targetType.IsVisible)
                return null;

            var typeBuilder = ModuleBuilder.DefineType($"StructProxy_{targetType.Name}_{Guid.NewGuid():N}", TypeAttributes.Public, typeof(ValueType));

            var fieldBuilder = typeBuilder.DefineField("_target", targetType, FieldAttributes.Private);

            GenerateConstructor(targetType, typeBuilder, fieldBuilder);

            var interfaceTypes = targetType.GetInterfaces().Where(x => x.IsVisible);
            foreach (var interfaceType in interfaceTypes)
            {
                GenerateInterfaceImplementation(interfaceType, targetType, typeBuilder, fieldBuilder);
            }

            return typeBuilder.CreateTypeInfo();
        }

        private static void GenerateConstructor(Type targetType, TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
        {
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { targetType });

            var constructorGenerator = constructor.GetILGenerator();
            constructorGenerator.Emit(OpCodes.Ldarg_0);
            constructorGenerator.Emit(OpCodes.Ldarg_1);
            constructorGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            constructorGenerator.Emit(OpCodes.Ret);
        }

        private static void GenerateInterfaceImplementation(Type interfaceType, Type targetType, TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
        {
            typeBuilder.AddInterfaceImplementation(interfaceType);

            var interfaceMap = targetType.GetInterfaceMap(interfaceType);

            for (var index = 0; index < interfaceMap.InterfaceMethods.Length; index++)
            {
                var interfaceMethodInfo = interfaceMap.InterfaceMethods[index];
                var targetMethodInfo = interfaceMap.TargetMethods[index];
                var parameters = interfaceMethodInfo.GetParameters();

                var method = typeBuilder.DefineMethod(interfaceMethodInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final, interfaceMethodInfo.ReturnType, parameters.Select(x => x.ParameterType).ToArray());

                if (targetMethodInfo.IsGenericMethod)
                {
                    var genericArguments = targetMethodInfo.GetGenericArguments();
                    method.DefineGenericParameters(genericArguments.Select((x, i) => $"T{i}").ToArray());
                }

                method.SetImplementationFlags(method.GetMethodImplementationFlags() | MethodImplAttributes.AggressiveInlining);

                var methodGenerator = method.GetILGenerator();
                methodGenerator.Emit(OpCodes.Ldarg_0);
                methodGenerator.Emit(OpCodes.Ldfld, fieldBuilder);

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
