using System;

namespace MDA.Domain.Models
{
    public interface IIdentity<out TId>
    {
        TId Id { get; }
    }

    public abstract class Identity<TId> : IEquatable<Identity<TId>>, IIdentity<TId>
    {
        protected Identity(TId id)
        {
            Id = id;
        }

        public TId Id { get; }

        public bool Equals(Identity<TId> id)
        {
            if (ReferenceEquals(this, id)) return true;
            if (ReferenceEquals(null, id)) return false;

            return Id.Equals(id.Id);
        }

        public override bool Equals(object other)
        {
            return Equals(other as Identity<TId>);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString() => Id.ToString();
    }
}
