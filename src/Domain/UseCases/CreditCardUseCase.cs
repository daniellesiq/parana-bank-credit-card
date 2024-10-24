using Domain.Events;
using Domain.Interfaces;
using Domain.Mappers;
using Domain.UseCases.Boundaries;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text;

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

                var cardEvent = CreditCardMappers.InputToEvent(input, GetCreditCardNumber(16));

                await _publisher.Publish(cardEvent, cancellationToken);

                _logger.LogInformation("Sent event | Ending | CorrelationId: {CorrelationId} | Document: {Document}",
                   input.CorrelationId,
                   input.Document);

                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error.");

                await _publisher.Publish(new ErrorEvent
                {
                    CorrelationId = input.CorrelationId,
                    ErrorMessage = ex.Message,
                    Source = "CreditCard"
                }, cancellationToken);

                throw;
            }
        }

        public static string GetCreditCardNumber(int length)
        {
            var random = new Random();
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int numero = random.Next(0, 10);
                stringBuilder.Append(numero);
            }
            return stringBuilder.ToString();
        }
    }
}
