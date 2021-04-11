using MDA.Runtime.Models.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MDA.Runtime.Controllers
{
    [ApiController]
    [Route("/v1.0/metadata")]
    public class MetadataController : ControllerBase
    {
        [HttpPost]
        public async Task<MetadataResponse> GetAsync()
        {
            return new MetadataResponse();
        }
    }
}
