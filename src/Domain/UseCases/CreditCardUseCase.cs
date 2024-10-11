using Domain.Events;
using Domain.Interfaces;
using Domain.Mappers;
using Domain.UseCases.Boundaries;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Domain.UseCases
{
    public class CreditCardUseCase : ICreditCardUseCase
    {
        private readonly ILogger<CreditCardUseCase> _logger;
        private readonly IPublishEndpoint _publisher;

        public CreditCardUseCase(ILogger<CreditCardUseCase> logger, IPublishEndpoint publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        public async Task<string> Handle(CreditCardInput input, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("{Class} | Starting | CorrelationId: {CorrelationId}",
                     nameof(CreditCardUseCase),
                    input.CorrelationId);

                var cardEvent = CreditCardMappers.InputToEvent(input, GetCreditCardNumber());

                await _publisher.Publish(cardEvent, cancellationToken);

                _logger.LogInformation("{Class} | Ending | CorrelationId: {CorrelationId}",
                    nameof(CreditCardUseCase),
                    input.CorrelationId);

                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error.");
                throw;
            }
        }

        public long GetCreditCardNumber()
        {
            Random randon = new Random();
            return randon.Next(1, 13);
        }
    }
}
