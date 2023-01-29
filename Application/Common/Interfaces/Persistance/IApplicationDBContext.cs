using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces.Persistance
{
    public interface IApplicationDBContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbSet<Account> Accounts { get; }
        DbSet<Card> Cards { get; }
        DbSet<CardTransaction> Transactions { get; }
    }
}
