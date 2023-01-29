using Application.Common.Interfaces.Persistance;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Persistance.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        protected readonly ApplicationDBContext _context;
        public AccountRepository(ApplicationDBContext context)
        {
            this._context = context;
        }

        public void UpdateAccountBalance(Account? account, decimal amount)
        {
            if (account != null)
            {
                account.Balance += amount;
            }
        }

        public Account? GetAccountById(Guid accountId)
        {
            return _context.Accounts.SingleOrDefault(a => a.Id == accountId);
        }
    }
}
