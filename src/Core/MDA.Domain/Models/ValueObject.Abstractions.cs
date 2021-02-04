using MDA.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Domain.Models
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// When overriden in a derived class, returns all members of a value objects which constitute its identity.
        /// </summary>
        protected abstract IEnumerable<object> GetEqualityMembers();

        public bool Equals(ValueObject other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return GetEqualityMembers().SequenceEqual(other.GetEqualityMembers());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (GetType() != obj.GetType()) return false;

            return obj is ValueObject other &&
                   GetEqualityMembers().SequenceEqual(other.GetEqualityMembers());
        }

        public override int GetHashCode()
            => HashHelper.ComputeHashCode(GetEqualityMembers());
    }

    public abstract class ComparableValueObject : ValueObject, IComparable
    {
        protected abstract IEnumerable<IComparable> GetComparableMembers();

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(this, obj)) return 0;
            if (ReferenceEquals(null, obj)) return 1;

            if (GetType() != obj.GetType())
                throw new InvalidOperationException();

            return CompareTo(obj as ComparableValueObject);
        }

        protected int CompareTo(ComparableValueObject other)
        {
            using (var thisComponents = GetComparableMembers().GetEnumerator())
            using (var otherComponents = other.GetComparableMembers().GetEnumerator())
            {
                while (true)
                {
                    var x = thisComponents.MoveNext();
                    var y = otherComponents.MoveNext();

                    if (x != y)
                        throw new InvalidOperationException();

                    if (x)
                    {
                        if (thisComponents.Current == null) continue;
                        var c = thisComponents.Current.CompareTo(otherComponents.Current);
                        if (c != 0)
                            return c;
                    }
                    else
                    {
                        break;
                    }
                }

                return 0;
            }
        }
    }

    public abstract class ComparableValueObject<T> : ComparableValueObject, IComparable<T>
        where T : ComparableValueObject<T>
    {
        public int CompareTo(T other) => base.CompareTo(other);
    }

    public static class ComparableValueObjectExtensions
    {
        public static IComparable AsNonGenericComparable<T>(IComparable<T> comparable)
        {
            return new NonGenericComparable<T>(comparable);
        }

        private class NonGenericComparable<T> : IComparable
        {
            public NonGenericComparable(IComparable<T> comparable)
            {
                _comparable = comparable;
            }

            private readonly IComparable<T> _comparable;

            public int CompareTo(object obj)
            {
                if (ReferenceEquals(_comparable, obj)) return 0;
                if (ReferenceEquals(null, obj))
                    throw new ArgumentNullException();

                return _comparable.CompareTo((T)obj);
            }
        }
    }

}
