using MassTransit;

namespace parana_bank_credit_card.Extensions
{
    public static class MasstransitExtension
    {
        public static void AddMassTransitExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(bus =>
            {
                bus.AddDelayedMessageScheduler();
                bus.SetKebabCaseEndpointNameFormatter();

                bus.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Durable = true;
                    cfg.AutoDelete = false;
                    cfg.Host(configuration.GetConnectionString("RabbitMq"));
                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                    cfg.UseMessageRetry(retry => { retry.Interval(3, TimeSpan.FromSeconds(5)); });
                });
            });
        }
    }
}
