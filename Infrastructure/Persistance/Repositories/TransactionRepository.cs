using Application.Common.Interfaces.Persistance;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Persistance.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        protected readonly ApplicationDBContext _context;
        public TransactionRepository(ApplicationDBContext context)
        {
            this._context = context;
        }

        public void AddCardTransaction(CardTransaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public List<CardTransaction> GetTransactionByCardId(Guid cardId)
        {
            return _context.Transactions.Where(t => t.CardId == cardId).ToList();
        }
    }
}
