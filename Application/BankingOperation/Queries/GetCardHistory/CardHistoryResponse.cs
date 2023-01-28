using Domain.Dtos;

namespace Application.Authentification.Commands.Authenticate
{
    public record CardHistoryResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public List<TransactionDto> Transactions { get; set; }

        public AccountDto Account { get; set; }

    }

}
