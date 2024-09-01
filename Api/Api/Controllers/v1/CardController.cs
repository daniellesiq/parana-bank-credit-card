using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        public CardController()
        {
                
        }

        [HttpPost]
        public async Task<IActionResult> SendNewCardAsync()
        {
            return Ok();
        }
    }
}
