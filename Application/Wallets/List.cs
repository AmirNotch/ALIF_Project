using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Persistence;

namespace Application.Wallets
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<WalletDto>>>
        {
            public ListParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<WalletDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<PagedList<WalletDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Wallets
                    .Where(l => l.AppUser.Id == request.Params.Id)
                    .ProjectTo<WalletDto>(_mapper.ConfigurationProvider, new {currentUsername = _userAccessor.GetUsername()})
                    .AsQueryable();
                
                return Result<PagedList<WalletDto>>.Success(
                    await PagedList<WalletDto>.CreateAsync(query, request.Params.PageNumber,
                        request.Params.PageSize)
                );
            }
        }
    }
}