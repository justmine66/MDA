using System;
using System.Collections.Generic;

namespace MDA.EventSourcing
{
    public abstract class ComparableValueObject : ValueObject, IComparable
    {
        protected abstract IEnumerable<IComparable> GetComparableComponents();

        protected IComparable AsNonGenericComparable<T>(IComparable<T> comparable)
        {
            return new NonGenericComparable<T>(comparable);
        }

        private class NonGenericComparable<T> : IComparable
        {
            private readonly IComparable<T> _comparable;

            public NonGenericComparable(IComparable<T> comparable)
            {
                _comparable = comparable;
            }
            public int CompareTo(object obj)
            {
                if (object.ReferenceEquals(_comparable, obj)) return 0;
                if (object.ReferenceEquals(null, obj))
                    throw new ArgumentNullException();

                return _comparable.CompareTo((T)obj);
            }
        }

        protected int CompareTo(ComparableValueObject other)
        {
            using (var thisComponents = GetComparableComponents().GetEnumerator())
            using (var otherComponents = other.GetComparableComponents().GetEnumerator())
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

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(this, obj)) return 0;
            if (ReferenceEquals(null, obj)) return 1;

            if (GetType() != obj.GetType())
                throw new InvalidOperationException();

            return CompareTo(obj as ComparableValueObject);
        }
    }
}
