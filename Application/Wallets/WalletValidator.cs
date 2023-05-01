using Domain;
using FluentValidation;

namespace Application.Wallets
{
    public class WalletValidator : AbstractValidator<Wallet>
    {
        public WalletValidator()
        {
            RuleFor(x => x.Value).NotEmpty();
            RuleFor(x => x.Balance).NotEmpty();
            RuleFor(x => x.Identified).NotEmpty();
        }
    }
}