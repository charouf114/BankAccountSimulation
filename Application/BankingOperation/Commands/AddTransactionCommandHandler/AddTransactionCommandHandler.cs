using Application.Common.Interfaces.Persistance;
using Domain.Entities;
using Domain.Enum;
using MediatR;

namespace Application.Authentification.Commands.Authenticate
{
    public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand, AddTransactionResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddTransactionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AddTransactionResponse> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            if (_unitOfWork.Cards.GetCardById(request.CardId) is not Card card)
            {
                return new AddTransactionResponse() { Success = false, Message = "Card Not Found" };
            }

            if (card.GetCardStatus() != CardState.Enabled)
            {
                return new AddTransactionResponse() { Success = false, Message = "Card Not Enabled" };
            }

            if (_unitOfWork.Accounts.GetAccountById(card.AccountId) is not Account account)
            {
                return new AddTransactionResponse() { Success = false, Message = "Account Not Found" };
            }

            var transaction = new CardTransaction()
            {
                Id = Guid.NewGuid(),
                CardId = card.Id,
                Amount = request.Amount,
                Type = request.TransactionType,
                Currency = request.Currency,
                CreationDate = DateTime.UtcNow,
                Status = TransactionState.Accepted
            };

            if (request.TransactionType == TransactionType.Debit && account.Balance < request.Amount)
            {
                transaction.Status = TransactionState.Rejected;
            }
            else
            {
                _unitOfWork.Accounts.UpdateAccountBalance(account, transaction.Amount);
            }

            _unitOfWork.Transactions.AddCardTransaction(transaction);

            _unitOfWork.Save();

            return new AddTransactionResponse()
            {
                Success = transaction.Status == TransactionState.Accepted,
                Message = transaction.Status == TransactionState.Accepted ? "Transaction Added Sucessefully" : "Transaction Rejected"
            };
        }
    }
}
