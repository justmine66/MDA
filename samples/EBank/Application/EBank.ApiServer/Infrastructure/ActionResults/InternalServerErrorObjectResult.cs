using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBank.ApiServer.Infrastructure.ActionResults
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error, int statusCode = StatusCodes.Status500InternalServerError)
            : base(error)
        {
            StatusCode = statusCode;
        }
    }
}
