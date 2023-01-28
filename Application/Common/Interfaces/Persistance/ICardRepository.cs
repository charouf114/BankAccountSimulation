using Domain.Entities;

namespace Application.Common.Interfaces.Persistance
{
    public interface ICardRepository
    {
        Card? GetCardByNumber(string cardNumber);

        Card? GetCardById(Guid id);
    }
}
