using MDA.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Domain.Models
{
    public interface IEntity<TId> : IEquatable<IEntity<TId>>
    {
        TId Id { get; }
    }

    public interface IEntityWithCompositeId<TId> : IEntity<TId>
    {
        /// <summary>
        /// Gets all members of the identity of the entity.
        /// </summary>
        IEnumerable<object> GetIdentityMembers();
    }

    public abstract class Entity<TId> : IEntity<TId>
    {
        protected Entity(TId id)
            => Id = id;

        public TId Id { get; }

        public virtual bool Equals(IEntity<TId> other)
        {
            if (ReferenceEquals(null, other)) return false;

            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return obj is IEntity<TId> other && Equals(other);
        } 

        public override int GetHashCode()
            => HashHelper.ComputeHashCode(Id);
    }

    public abstract class EntityWithCompositeId<TId> : Entity<TId>
    {
        protected EntityWithCompositeId(TId id)
            : base(id)
        { }

        public abstract IEnumerable<object> GetIdentityMembers();

        public bool Equals(IEntityWithCompositeId<TId> other)
        {
            if (ReferenceEquals(null, other)) return false;

            return GetIdentityMembers().SequenceEqual(other.GetIdentityMembers());
        }

        public override bool Equals(object obj)
            => obj is IEntityWithCompositeId<TId> other && Equals(other);

        public override int GetHashCode()
            => HashHelper.ComputeHashCode(GetIdentityMembers());
    }
}
