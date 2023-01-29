using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    public class CardTransaction
    {
        public Guid Id { get; set; }

        public Guid CardId { get; set; }

        public TransactionType Type { get; set; }

        public string Currency { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }

        public DateTime CreationDate { get; set; }

        public TransactionState Status { get; set; }

    }
}