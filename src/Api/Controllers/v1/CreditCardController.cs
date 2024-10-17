using Domain.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace parana_bank_credit_card.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CreditCardController : ControllerBase
    {
        private readonly IPublishEndpoint _publisher;
        private readonly ILogger<CreditCardController> _logger;

        public CreditCardController(IPublishEndpoint publisher, ILogger<CreditCardController> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "new credit card", Description = "Insert new credit card")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendNewCreditCardAsync([FromBody] CreditCardEvent input, CancellationToken cancellationToken)
        {
            if (input != null)
            {
                await _publisher.Publish<CreditCardEvent>(input, cancellationToken);

                _logger.LogInformation("Sent event: CorrelationId: {CorrelationId} | Document: {Document}",
                    input.CorrelationId,
                    input.Document);

                return Ok();
            }
            return BadRequest();
        }
    }
}
