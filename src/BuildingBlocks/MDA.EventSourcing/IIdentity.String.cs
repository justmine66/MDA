using System;

namespace MDA.EventSourcing
{
    public abstract class StringIdentity : IEquatable<StringIdentity>, IIdentity<string>
    {
        protected StringIdentity()
        {
            Id = Guid.NewGuid().ToString();
        }

        protected StringIdentity(string id)
        {
            Id = id;
        }

        public bool Equals(StringIdentity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public string Id { get; }

        public override bool Equals(object other)
        {
            return Equals(other as StringIdentity);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{{Id: \"{Id}\"}}";
        }
    }
}
