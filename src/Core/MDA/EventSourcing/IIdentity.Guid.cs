using System;

namespace MDA.EventSourcing
{
    public abstract class GuidIdentity : IEquatable<GuidIdentity>, IIdentity<Guid>
    {
        protected GuidIdentity()
        {
            Id = Guid.NewGuid();
        }

        protected GuidIdentity(Guid id)
        {
            Id = id;
        }

        public bool Equals(GuidIdentity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public Guid Id { get; }

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
