using System;

namespace MDA.EventSourcing
{
    public abstract class ComparableValueObject<T> : ComparableValueObject, IComparable<T>
        where T : ComparableValueObject<T>
    {
        public int CompareTo(T other)
        {
            return base.CompareTo(other);
        }
    }
}
