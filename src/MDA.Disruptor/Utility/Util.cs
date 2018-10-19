using System.Runtime.CompilerServices;
using InlineIL;
using static InlineIL.IL.Emit;

namespace MDA.Disruptor.Utility
{
    /// <summary>
    /// Set of common functions used by the Disruptor.
    /// </summary>
    internal class Util
    {
        private static readonly int OffsetToArrayData = ElemOffset(new object[1]);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(object array, int index)
        {
            IL.DeclareLocals(
                false,
                typeof(byte).MakeByRefType()
            );

            Ldarg(nameof(array)); // load the object
            Stloc_0(); // convert the object pointer to a byref
            Ldloc_0(); // load the object pointer as a byref

            Ldarg(nameof(index)); // load the index
            Sizeof(typeof(object)); // get the size of the object pointer
            Mul(); // multiply the index by the offset size of the object pointer

            Ldsfld(new FieldRef(typeof(Util), nameof(OffsetToArrayData))); // get the offset to the start of the array
            Add(); // add the start offset to the element offset

            Add(); // add the start + offset to the byref object pointer

            Ldobj(typeof(T)); // load a T value from the computed address

            return IL.Return<T>();
        }

        private static int ElemOffset(object[] arr)
        {
            Ldarg(nameof(arr));
            Ldc_I4_0();
            Ldelema(typeof(object));
            Ldarg(nameof(arr));
            Sub();

            return IL.Return<int>();
        }
    }
}
