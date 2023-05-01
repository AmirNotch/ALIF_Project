using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Wallets
{
    public class CreateWallet
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Wallet Wallet { get; set; }
            public string Id { get; set; }
        }
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Wallet).SetValidator(new WalletValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.AppUsers.Where(x => 
                    x.Id.ToString() == request.Id).FirstOrDefaultAsync();

                var attendee = new Wallet()
                {
                    AppUser = user,
                    Value = request.Wallet.Value,
                    Balance = request.Wallet.Balance,
                    Identified = request.Wallet.Identified
                };
                
                _context.Wallets.Add(attendee);
                
                var result = await _context.SaveChangesAsync() > 0;
                
                if (!result)
                {
                    return Result<Unit>.Failure("Failed to create Wallet");
                }
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}