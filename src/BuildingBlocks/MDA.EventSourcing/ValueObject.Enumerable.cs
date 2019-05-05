using System;
using System.Collections.Generic;

namespace MDA.EventSourcing
{
    public class EnumerableValueObject : ComparableValueObject<EnumerableValueObject>
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        protected EnumerableValueObject(int id, string name)
        {
            Id = id;
            Name = name;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return Name;
        }

        protected override IEnumerable<IComparable> GetComparableComponents()
        {
            yield return this;
        }

        public override string ToString() => Name;
    }
}
