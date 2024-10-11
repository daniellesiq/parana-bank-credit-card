using Domain.UseCases.Boundaries;
using MediatR;

namespace Domain.Interfaces
{
    public interface ICreditCardUseCase : IRequestHandler<CreditCardInput, string>
    {
    }
}
