using Domain.Entities;
using Domain.Enum;

namespace Domain.Dtos
{
    public class TransactionDto
    {
        public TransactionType TransactionType { get; set; }

        public string OperationType => TransactionType.ToString();

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreationDate { get; set; }

        public TransactionState Status { get; set; }


        // May Be AutoMapper In the Future
        public TransactionDto(CardTransaction cardTransaction)
        {
            TransactionType = cardTransaction.Type;
            Currency = cardTransaction.Currency;
            Amount = cardTransaction.Amount;
            Status = cardTransaction.Status;
            CreationDate = cardTransaction.CreationDate;
        }
    }
}
