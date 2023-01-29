namespace Application.Authentification.Commands.Authenticate
{
    public record AuthenticateResponse
    {
        public bool IsSuccess { get; set; }

        public string AccessToken { get; set; }

        public string Message { get; set; }
    }

}
