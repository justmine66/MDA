using System.Collections.Generic;

namespace MDA.Runtime.Models.Metadata
{
    public class MetadataResponse
    {
        /// <summary>
        /// loaded components
        /// </summary>
        public IEnumerable<Component> Components { get; set; }

        /// <summary>
        /// Component of Metadata
        /// </summary>
        public class Component
        {
            /// <summary>
            /// Name of the component.
            /// </summary>
            /// <example>MessageBus</example>
            public string Name { get; set; }

            /// <summary>
            /// Type of the component.
            /// </summary>
            /// <example>MessageBus.kafka</example>
            public string Type { get; set; }

            /// <summary>
            /// Version of the component.
            /// </summary>
            /// <example>v1.0</example>
            public string Version { get; set; }
        }
    }
}
