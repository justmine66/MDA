using System;
using System.Collections.Generic;
using System.Text;

namespace MDA.Domain.Model
{
    public abstract class Identity : IIdentity<string>
    {
        public string Id { get; set; }
    }
}
