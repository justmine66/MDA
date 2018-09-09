using System;

namespace MDA.Common.Domain.Model
{
    public abstract class Identity : IEquatable<Identity>, IIdentity<string>
    {
        public Identity()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }

        public Identity(string id)
        {
            this.Id = id;
        }

        public string Id { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is Identity other && Equals(other);
        }

        public bool Equals(Identity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return false;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() * 907 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}
