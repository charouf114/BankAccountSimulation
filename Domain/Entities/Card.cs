using Domain.Enum;

namespace Domain.Entities
{
    public class Card
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string OwnerName { get; set; }

        public string CardNumber { get; set; }

        public CardState CardStatus { get; set; }

        public DateTime ExpirationDate { get; set; }

        public byte[] SecretCode { get; set; }

        public byte[] Salt { get; set; }

        public CardState GetCardStatus()
        {
            return ExpirationDate < DateTime.UtcNow ? CardState.Expired : CardStatus;
        }
    }
}