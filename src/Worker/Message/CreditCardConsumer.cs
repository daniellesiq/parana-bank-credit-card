using Domain.Events;
using Domain.Mappers;
using MassTransit;
using MediatR;

namespace Worker.Message
{
    public class CreditCardConsumer : IConsumer<CreditCardEvent>
    {
        private readonly ILogger<CreditCardConsumer> _logger;
        private readonly IMediator _mediator;

        public CreditCardConsumer(ILogger<CreditCardConsumer> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<CreditCardEvent> context)
        {
            var correlationId = context.Message.CorrelationId;
            try
            {
                _logger.LogInformation("Event received: {Class} | CorrelationId: {CorrelationId} | Document: {Document}",
                   nameof(CreditCardConsumer),
                   context.Message.CorrelationId,
                   context.Message.Document);

                var input = CreditCardMappers.EventToInput(context);
                await _mediator.Send(input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error.");
                throw;
            }
        }
    }
}
