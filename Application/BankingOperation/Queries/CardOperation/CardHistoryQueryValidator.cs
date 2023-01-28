using FluentValidation;

namespace Application.Authentification.Commands.Authenticate
{
    public class CardHistoryQueryValidator : AbstractValidator<CardHistoryQuery>
    {
        public CardHistoryQueryValidator()
        {
            RuleFor(p => p.CardId).NotEmpty().NotNull();
        }
    }
}
