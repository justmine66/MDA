using MDA.Infrastructure.Scheduling;
using MDA.Infrastructure.Serialization;
using MDA.Infrastructure.Typing;

namespace MDA.Infrastructure.DependencyInjection
{
    public static class MdaConfigureContextExtensions
    {
        public static IMdaConfigureContext AddInfrastructure(this IMdaConfigureContext context)
        {
            context.Services.AddTyping();
            context.Services.AddSerialization();
            context.Services.AddScheduling();

            return context;
        }
    }
}
