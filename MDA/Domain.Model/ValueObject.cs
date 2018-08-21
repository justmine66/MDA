using MDA.Common;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Domain.Model
{
    public abstract class ValueObject
    {
        /// <summary>
        /// 获取相等的组件。
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;
            if (object.ReferenceEquals(null, obj)) return false;
            if (this.GetType() != obj.GetType()) return false;

            return obj is ValueObject other && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.CombineHashCodes(GetEqualityComponents());
        }
    }
}
