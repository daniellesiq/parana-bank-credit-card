using Domain.Events;
using MassTransit;

namespace Worker.Message
{
    public class CreditCardConsumer : IConsumer<CreditCardEvent>
    {
        private readonly ILogger<CreditCardConsumer> _logger;

        public CreditCardConsumer(ILogger<CreditCardConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreditCardEvent> context)
        {
            var document = context.Message.Document;
            try
            {
                _logger.LogInformation($"Event received: {nameof(CreditCardConsumer)}: {document} ");

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
