using Bogus;
using Domain.Mappers;
using Domain.UseCases.Boundaries;

namespace UnitTests
{
    public class CreditCardMappersUnitTest
    {
        [Fact]
        public async Task Given_InputToEvent_ReturnsExpectedCreditCardValidatedEvent()
        {
            // Arrange
            var creditCardNumber = "4567257963478140";
            var input = new Faker<CreditCardInput>()
                .RuleFor(c => c.Document, 1234567890)
                .RuleFor(c => c.Income, 1000m)
                .RuleFor(c => c.Score, 500)
                .RuleFor(c => c.CreditLimit, 1000m)
                .Generate();

            // Act
            var result = CreditCardMappers.InputToEvent(input, creditCardNumber);

            // Assert
            Assert.Equal(input.Document, result.Document);
            Assert.Equal(input.Income, result.Income);
            Assert.Equal(input.Score, result.Score);
            Assert.Equal(input.CreditLimit, result.CreditLimit);
            Assert.Equal(creditCardNumber, result.CreditCardNumber);
        }
    }
}