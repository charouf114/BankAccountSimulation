using MediatR;

namespace Application.Authentification.Commands.Authenticate
{
    public record AuthenticateCommand : IRequest<AuthenticateResponse>
    {
        public string CardNumber { get; set; }

        public string SecretCode { get; set; }
    }
}
