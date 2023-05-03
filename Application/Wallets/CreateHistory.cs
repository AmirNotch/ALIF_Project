using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Wallets
{
    public class CreateHistory
    {
        public class Command : IRequest<Result<Unit>>
        {
            public History History { get; set; }
            public string FromId { get; set; }
            public string ToId { get; set; }
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
                var walletFrom = await _context.Wallets.Where(x => 
                    x.Id.ToString() == request.FromId).FirstOrDefaultAsync();
                var walletTo = await _context.Wallets.Where(x => 
                    x.Id.ToString() == request.ToId).FirstOrDefaultAsync();

                if (walletFrom == null || walletTo == null)
                {
                    return walletFrom == null
                        ? Result<Unit>.Failure("Wallet not found")
                        : Result<Unit>.Failure("Second Wallet not found");
                }

                if (walletFrom.Value != walletTo.Value)
                {
                    return Result<Unit>.Failure(
                        $"You can not send money from {walletFrom.Value} to {walletTo.Value} Value.");
                }

                walletFrom.Balance -= request.History.Sum;
                walletTo.Balance += request.History.Sum;

                if (walletFrom.Balance < 0)
                {
                    return Result<Unit>.Failure("You hadn't enough money.");
                }
                else if (walletTo.Identified == false && walletTo.Balance > 10_000)
                {
                    return Result<Unit>.Failure($"You are not Identified and can not have more than 10000 {walletTo.Value}.");
                }
                else if (walletTo.Balance > 100_000)
                {
                    return Result<Unit>.Failure($"You can not have more than 100000 {walletTo.Value}.");
                }

                await using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    var historyFirst = new History
                    {
                        Sum = 0 - request.History.Sum,
                        Value = walletFrom.Value,
                        CreatedAt = DateTime.UtcNow,
                        Wallet = walletFrom,
                    };
                    
                    var historySecond = new History
                    {
                        Sum = 0 + request.History.Sum,
                        Value = walletTo.Value,
                        CreatedAt = DateTime.UtcNow,
                        Wallet = walletTo,
                    };

                    try
                    {
                        _context.Histories.Add(historyFirst);
                        _context.Histories.Add(historySecond);
                        _context.Wallets.UpdateRange(walletFrom,walletTo);
                        var result = await _context.SaveChangesAsync() > 0;
                        if (!result)
                        {
                            return Result<Unit>.Failure("Failed to save Transaction");
                        }
                        transaction.Commit();  
                    }  
                    catch (Exception ex)  
                    {  
                        transaction.Rollback(); 
                        return Result<Unit>.Failure($"You something went wrong.");
                    }  
                }
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}