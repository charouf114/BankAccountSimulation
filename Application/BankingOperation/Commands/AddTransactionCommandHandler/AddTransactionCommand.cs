using Domain.Enum;
using MediatR;

namespace Application.Authentification.Commands.Authenticate
{
    public record AddTransactionCommand : IRequest<AddTransactionResponse>
    {
        public Guid CardId { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }
    }
}
