using MDA.Common;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Domain.Model
{
    /// <summary>
    /// 表示一个实体。
    /// </summary>
    public abstract class Entity { }
    /// <summary>
    /// 表示一个带复合身份标识的实体。
    /// </summary>
    public abstract class EntityWithCompositeId : Entity
    {
        /// <summary>
        /// 复合身份标识。
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<object> GetIdentityComponents();

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;
            if (object.ReferenceEquals(null, obj)) return false;
            if (GetType() != obj.GetType()) return false;

            return obj is EntityWithCompositeId other && GetIdentityComponents().SequenceEqual(other.GetIdentityComponents());
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.CombineHashCodes(GetIdentityComponents());
        }
    }
}
