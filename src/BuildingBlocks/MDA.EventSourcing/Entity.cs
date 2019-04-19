using System;
using System.Collections.Generic;
using System.Linq;
using MDA.Shared;

namespace MDA.EventSourcing
{
    public abstract class Entity { }

    public abstract class Entity<TId> : Entity, IEquatable<Entity<TId>>
    {
        protected Entity(TId id)
        {
            Id = id;
        }

        public TId Id { get; }

        public bool Equals(Entity<TId> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entity<TId>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return GetType().GetHashCode() * 397 + Id.GetHashCode();
            }
        }
    }

    public abstract class EntityWithCompositeId : Entity
    {
        /// <summary>
        /// When overriden in a derived class, gets all components of the identity of the entity.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<object> GetIdentityComponents();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (GetType() != obj.GetType()) return false;
            return obj is EntityWithCompositeId other && GetIdentityComponents().SequenceEqual(other.GetIdentityComponents());
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.CombineHashCodes(GetIdentityComponents());
        }
    }
}
