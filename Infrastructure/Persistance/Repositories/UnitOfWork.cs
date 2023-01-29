using Application.Common.Interfaces.Persistance;
using Infrastructure.Persistence;

namespace Infrastructure.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDBContext _context;
        public UnitOfWork(ApplicationDBContext context)
        {
            this._context = context;
            Cards = new CardRepository(this._context);
            Accounts = new AccountRepository(this._context);
            Transactions = new TransactionRepository(this._context);
        }

        public ICardRepository Cards
        {
            get;
            private set;
        }
        public IAccountRepository Accounts
        {
            get;
            private set;
        }
        public ITransactionRepository Transactions
        {
            get;
            private set;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
        }
    }
}
