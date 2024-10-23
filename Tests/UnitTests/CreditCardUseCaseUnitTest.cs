using Domain.Events;
using Domain.UseCases;
using Domain.UseCases.Boundaries;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    public class CreditCardUseCaseUnitTest
    {
        private readonly CreditCardUseCase _useCase;
        private readonly Mock<IPublishEndpoint> _publishMock;
        private readonly Mock<ILogger<CreditCardUseCase>> _loggerMock;

        public CreditCardUseCaseUnitTest()
        {
            _publishMock = new Mock<IPublishEndpoint>();
            _loggerMock = new Mock<ILogger<CreditCardUseCase>>();
            _useCase = new CreditCardUseCase(_loggerMock.Object, _publishMock.Object);
        }

        [Fact]
        public async Task Given_Handle_Should_LogInformation_When_EventPublished()
        {
            // Arrange
            var input = new CreditCardInput
            {
                CorrelationId = Guid.NewGuid(),
                Document = 1234567890,
                Income = 5000,
                Score = 750,
                CreditLimit = 1000
            };

            _publishMock.Setup(c => c.Publish(It.IsAny<CreditCardValidatedEvent>(), It.IsAny<CancellationToken>()));

            // Act
            var result = await _useCase.Handle(input, new CancellationToken());

            // Assert
            _publishMock.Verify(publisher =>
                publisher.Publish(It.IsAny<CreditCardValidatedEvent>(), new CancellationToken()), Times.Once);
            _loggerMock.Verify(
                  logger => logger.Log(
                      It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                      It.IsAny<EventId>(),
                      It.Is<It.IsAnyType>((state, t) => state.ToString().Contains("Sent event")),
                      It.IsAny<Exception>(),
                      It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                  Times.Once);
        }

        [Fact]
        public async Task Given_Handle_Should_LogError_When_ExceptionIsThrown()
        {
            // Arrange
            var input = new CreditCardInput
            {
                CorrelationId = Guid.NewGuid(),
                Document = 1234567890,
                Income = 5000,
                Score = 750,
                CreditLimit = 1000
            };

            var cancellationToken = new CancellationToken();
            _publishMock.Setup(p => p.Publish(It.IsAny<CreditCardValidatedEvent>(), cancellationToken))
                         .Throws(new Exception("Error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _useCase.Handle(input, cancellationToken));

            _loggerMock.Verify(
                  logger => logger.Log(
                      It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                      It.IsAny<EventId>(),
                      It.Is<It.IsAnyType>((state, t) => state.ToString().Contains("Error")),
                      It.IsAny<Exception>(),
                      It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                  Times.Once);
        }
    }
}
