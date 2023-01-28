namespace Application.Authentification.Commands.Authenticate
{
    public record AddTransactionResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }

}
