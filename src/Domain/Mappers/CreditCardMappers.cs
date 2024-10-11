using Domain.Events;
using Domain.UseCases.Boundaries;
using MassTransit;

namespace Domain.Mappers
{
    public static class CreditCardMappers
    {
        public static CreditCardInput EventToInput(ConsumeContext<CreditCardEvent> clientEvent)
        {
            return new CreditCardInput(
                clientEvent.Message.CorrelationId,
                clientEvent.Message.Document,
                clientEvent.Message.Income,
                clientEvent.Message.Score,
                clientEvent.Message.CreditLimit
                );
        }

        public static CreditCardValidatedEvent InputToEvent(CreditCardInput input, long creditCardNumber)
        {
            return new CreditCardValidatedEvent(
                input.CorrelationId,
                input.Document,
                input.Income,
                input.Score,
                input.CreditLimit,
                creditCardNumber
                );
        }
    }
}
