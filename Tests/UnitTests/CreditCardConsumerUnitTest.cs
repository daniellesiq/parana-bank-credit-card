using Domain.Events;
using Domain.UseCases.Boundaries;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Worker.Message;

namespace UnitTests
{
    public class CreditCardConsumerUnitTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<CreditCardConsumer>> _loggerMock;
        private readonly CreditCardConsumer _creditCardConsumer;
        private readonly Mock<ConsumeContext<CreditCardEvent>> _contextMock;

        public CreditCardConsumerUnitTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<CreditCardConsumer>>();
            _creditCardConsumer = new CreditCardConsumer(_loggerMock.Object, _mediatorMock.Object);
            _contextMock = new Mock<ConsumeContext<CreditCardEvent>>();
        }

        [Fact]
        public async Task Given_Consume_Should_LogInformation_When_EventIsReceived()
        {
            // Arrange
            var creditCardEvent = new CreditCardEvent
            {
                CorrelationId = Guid.NewGuid(),
                Document = 1234567890,
                Income = 5000,
                Score = 750,
                CreditLimit = 1000
            };

            _contextMock.SetupGet(x => x.Message).Returns(creditCardEvent);

            // Act
            await _creditCardConsumer.Consume(_contextMock.Object);

            // Assert
            _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<CreditCardInput>(), default), Times.Once);
            _loggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, t) => state.ToString().Contains("Event received")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task Given_Consume_Should_LogError_When_ExceptionIsThrown()
        {
            // Arrange
            var creditCardEvent = new CreditCardEvent
            {
                CorrelationId = Guid.NewGuid(),
                Document = 1234567890,
                Income = 5000,
                Score = 750,
                CreditLimit = 1000
            };

            _contextMock.SetupGet(x => x.Message).Returns(creditCardEvent);
            _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreditCardInput>(), default))
                        .Throws(new Exception("Test Exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _creditCardConsumer.Consume(_contextMock.Object));

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
