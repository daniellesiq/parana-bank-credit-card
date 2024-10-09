using Domain.Events;
using MassTransit;
using Worker.Message;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, collection) =>
    {
        collection.AddHttpContextAccessor();

        collection.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();
            x.AddConsumer<CreditCardConsumer>();
            x.AddRequestClient<CreditCardEvent>();

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(context.Configuration.GetConnectionString("RabbitMq"));
                cfg.UseDelayedMessageScheduler();
                cfg.ServiceInstance(instance =>
                {
                    instance.ConfigureJobServiceEndpoints();
                    instance.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));

                });
            });
        });
    }).Build();

await host.RunAsync();