using Microsoft.AspNetCore.Http;

namespace MDA.AspnetCore.Extensions
{
    public static class HttpContextExtension
    {
        public static TServiceType GetService<TServiceType>(this HttpContext context) 
            where TServiceType : class
        {
            return context.RequestServices.GetService(typeof(TServiceType)) as TServiceType;
        }
    }
}
