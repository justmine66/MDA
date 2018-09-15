using Grain.interfaces.Reentrancy;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Orleans.CodeGeneration;
using Orleans.Concurrency;

namespace Grain.Implementations.Reentrancy
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class InterleaveAttribute : Attribute { }

    [MayInterleave(nameof(ArgHasInterleaveAttribute))]
    public class MyGrain: Orleans.Grain,IMyGrain
    {
        public static bool ArgHasInterleaveAttribute(InvokeMethodRequest req)
        {
            // Returning true indicates that this call should be interleaved with other calls.
            // Returning false indicates the opposite.
            return req.Arguments.Length == 1
                   && req.Arguments[0]?.GetType().GetCustomAttribute<InterleaveAttribute>() != null;
        }

        public Task Process(object payload)
        {
            throw new NotImplementedException();
        }
    }
}
