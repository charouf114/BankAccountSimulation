using Domain.Entities;

namespace Application.Common.Interfaces.Persistance
{
    public interface IAccountRepository
    {
        void UpdateAccountBalance(Account? account, decimal amount);
        Account? GetAccountById(Guid accountId);

    }
}
