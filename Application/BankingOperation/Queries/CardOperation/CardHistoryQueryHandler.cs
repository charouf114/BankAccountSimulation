using Application.Common.Interfaces.Persistance;
using Domain.Entities;
using Domain.Enum;
using MediatR;

namespace Application.Authentification.Commands.Authenticate
{
    public class CardHistoryQueryHandler : IRequestHandler<CardHistoryQuery, CardHistoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CardHistoryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CardHistoryResponse> Handle(CardHistoryQuery request, CancellationToken cancellationToken)
        {
            if (_unitOfWork.Cards.GetCardById(request.CardId) is not Card card)
            {
                return new CardHistoryResponse() { IsSuccess = false, Message = "Card Not Found" };
            }

            if (card.GetCardStatus() != CardState.Enabled)
            {
                return new CardHistoryResponse() { IsSuccess = false, Message = "Card Not Enabled" };
            }

            var transactions = _unitOfWork.Transactions.GetTransactionByCardId(request.CardId);
            var account = _unitOfWork.Accounts.GetAccountById(card.AccountId);

            return new CardHistoryResponse()
            {
                IsSuccess = true,
                Message = "Get Card Information Successfully Done",
                Account = new Domain.Dtos.AccountDto(account),
                Transactions = transactions.Select(t => new Domain.Dtos.TransactionDto(t)).OrderByDescending(t => t.CreationDate).ToList(),

            };
        }
    }
}
