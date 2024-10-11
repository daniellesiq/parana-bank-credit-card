using Domain.Events;
using Domain.UseCases.Boundaries;
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
        collection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreditCardInput).Assembly));
        collection.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();
            x.AddConsumer<CreditCardConsumer>();
            x.AddRequestClient<CreditCardEvent>();

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Durable = true;
                cfg.AutoDelete = false;
                cfg.Host(context.Configuration.GetConnectionString("RabbitMq"));
                cfg.UseDelayedMessageScheduler();
                cfg.ServiceInstance(instance =>
                {
                    instance.ConfigureJobServiceEndpoints();
                    instance.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));

                });
                cfg.UseMessageRetry(retry => { retry.Interval(3, TimeSpan.FromSeconds(5)); });
            });
        });
    }).Build();

await host.RunAsync();
