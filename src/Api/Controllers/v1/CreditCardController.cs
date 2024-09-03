using Microsoft.AspNetCore.Mvc;

namespace parana_bank_credit_card.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreditCardController : ControllerBase
    {

        private readonly ILogger<CreditCardController> _logger;

        public CreditCardController(ILogger<CreditCardController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendNewCreditCardAsync()
        {
            return Ok();
        }
    }
}
