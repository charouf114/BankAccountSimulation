using Domain.Entities;

namespace Application.Common.Interfaces.Persistance
{
    public interface ITransactionRepository
    {
        void AddCardTransaction(CardTransaction transaction);

        List<CardTransaction> GetTransactionByCardId(Guid cardId);

    }
}
