namespace Domain.Dtos
{
    public class TransactionInput
    {
        public Guid CardId { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }
    }
}
