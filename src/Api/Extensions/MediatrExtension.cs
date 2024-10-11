using Domain.Interfaces;
using Domain.UseCases;

namespace parana_bank_credit_card.Extensions
{
    public static class MediatrExtension
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddScoped<ICreditCardUseCase, CreditCardUseCase>();
            return services;
        }
    }
}
