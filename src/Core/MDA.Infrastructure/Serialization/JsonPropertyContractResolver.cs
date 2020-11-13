using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Infrastructure.Serialization
{
    public class JsonPropertyContractResolver : DefaultContractResolver
    {
        private readonly IEnumerable<string> _ignoreProperties;

        public JsonPropertyContractResolver(IEnumerable<string> ignoreProperties)
        {
            _ignoreProperties = ignoreProperties;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).ToList()
                .FindAll(p => !_ignoreProperties.Contains(p.PropertyName));
        }
    }
}
