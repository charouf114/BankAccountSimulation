using FluentValidation;

namespace Application.Authentification.Commands.Authenticate
{
    public class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
    {
        public AddTransactionCommandValidator()
        {
            RuleFor(p => p.Amount).NotEmpty().NotNull();
            RuleFor(p => p.Currency).NotEmpty().NotNull();
            RuleFor(p => p.TransactionType).NotEmpty().NotNull().IsInEnum();
            RuleFor(p => p.CardId).NotEmpty().NotNull();
        }
    }
}
