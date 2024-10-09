namespace Domain.Events
{
    public class CreditCardValidatedEvent
    {
        public CreditCardValidatedEvent(Guid correlationId, long document, decimal income, int score, decimal creditLimit, int account)
        {
            CorrelationId = correlationId;
            Document = document;
            Income = income;
            Score = score;
            CreditLimit = creditLimit;
            Account = account;
        }

        public Guid CorrelationId { get; init; } = default!;
        public long Document { get; init; } = default!;
        public decimal Income { get; init; } = default!;
        public int Score { get; init; } = default!;
        public decimal CreditLimit { get; init; }
        public int Account { get; init; } = default!;
    }
}
