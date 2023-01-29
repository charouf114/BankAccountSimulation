using FluentValidation;

namespace Application.Authentification.Commands.Authenticate
{
    public class RegisterCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(p => p.CardNumber).NotEmpty().Length(16);
            RuleFor(p => p.SecretCode).NotEmpty().Length(4);
        }
    }
}
