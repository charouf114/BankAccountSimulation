using Application.Common.Interfaces.Persistance;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Persistance.Repositories
{
    public class CardRepository : ICardRepository
    {
        protected readonly ApplicationDBContext _context;
        public CardRepository(ApplicationDBContext context)
        {
            this._context = context;
        }

        public Card? GetCardById(Guid id)
        {
            return _context.Cards.SingleOrDefault(c => c.Id == id);
        }

        public Card? GetCardByNumber(string cardNumber)
        {
            return _context.Cards.SingleOrDefault(c => c.CardNumber == cardNumber);
        }
    }
}
