namespace Application.Authentification.Commands.Authenticate
{
    public record AddTransactionResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }

}
