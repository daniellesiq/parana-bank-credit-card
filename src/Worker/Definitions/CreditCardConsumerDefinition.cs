using MassTransit;
using Worker.Message;

namespace Worker.Definitions
{
    public class CreditCardConsumerDefinition : ConsumerDefinition<CreditCardConsumer>
    {
        protected override void ConfigureConsumer(
          IReceiveEndpointConfigurator endpointConfigurator,
          IConsumerConfigurator<CreditCardConsumer> consumerConfigurator)
        {
            consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(5)));
        }
    }
}
