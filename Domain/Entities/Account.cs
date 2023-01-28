using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Owner { get; set; }
        public string IBAN { get; set; }
        public string RIB { get; set; }

        [Precision(18, 2)]
        public decimal Balance { get; set; }
    }
}