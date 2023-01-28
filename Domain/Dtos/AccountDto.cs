using Domain.Entities;

namespace Domain.Dtos
{
    public class AccountDto
    {
        public string Owner { get; set; }
        public string IBAN { get; set; }
        public string RIB { get; set; }
        public decimal Balance { get; set; }

        public AccountDto(Account account)
        {
            Owner = account.Owner;
            IBAN = account.IBAN;
            RIB = account.RIB;
            Balance = account.Balance;
        }
    }
}
