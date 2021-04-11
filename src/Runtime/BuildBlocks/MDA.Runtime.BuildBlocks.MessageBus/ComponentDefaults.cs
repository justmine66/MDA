namespace MDA.Runtime.BuildBlocks.MessageBus
{
    public class Component
    {
        public static string Name = "MessageBus";

        public class Types
        {
            public static string Kafka = $"{Name}.Kafka";
            public static string Redis = $"{Name}.redis";
            public static string RabbitMq = $"{Name}.RabbitMq";
        }
    }
}
