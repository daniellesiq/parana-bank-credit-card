using Domain.Events;
using Domain.UseCases.Boundaries;
using MassTransit;

namespace Domain.Mappers
{
    public static class CreditCardMappers
    {
        public static CreditCardInput EventToInput(ConsumeContext<CreditCardEvent> clientEvent)
        {
            var creditCardInput = new CreditCardInput
            {
                CorrelationId = clientEvent.Message.CorrelationId,
                Document = clientEvent.Message.Document,
                Income = clientEvent.Message.Income,
                Score = clientEvent.Message.Score,
                CreditLimit = clientEvent.Message.CreditLimit
            };
            return creditCardInput;
        }

        public static CreditCardValidatedEvent InputToEvent(CreditCardInput input, string creditCardNumber)
        {
            var creditCardValidatedEvent = new CreditCardValidatedEvent
            {
                CorrelationId = input.CorrelationId,
                Document = input.Document,
                Income = input.Income,
                Score = input.Score,
                CreditLimit = input.CreditLimit,
                CreditCardNumber = creditCardNumber
            };
           return creditCardValidatedEvent;
        }
    }
}
