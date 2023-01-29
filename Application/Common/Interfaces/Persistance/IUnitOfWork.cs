
namespace Application.Common.Interfaces.Persistance
{
    public interface IUnitOfWork : IDisposable
    {
        public ICardRepository Cards { get; }

        public IAccountRepository Accounts { get; }

        public ITransactionRepository Transactions { get; }

        int Save();
    }
}
