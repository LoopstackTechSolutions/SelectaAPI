using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SelectaAPI.Controllers.Selecta
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class DistributionCenterController : ControllerBase
    {
        [HttpPost("center-register")]
        public async Task<IActionResult> CenterRegister()
        {
            return Ok();
        }
    }
}
