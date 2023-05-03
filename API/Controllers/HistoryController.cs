using System.Linq;
using System.Threading.Tasks;
using API.Decryption;
using API.DTOs;
using API.Filter;
using API.Helpers;
using API.Services;
using Application.Wallets;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class HistoryController : BaseApiController
    {
        private readonly DataContext context;
        private readonly IUriService uriService;
        
        public HistoryController(DataContext context, IUriService uriService)
        {
            this.context = context;
            this.uriService = uriService;
        }

        [HttpPost("{fromId}/{toId}/ClientWallets")]
        public async Task<IActionResult> CreateHistory([FromBody] HistoryDTO historyDTO, string fromId, string toId)
        {
            var sum = int.Parse(DecryptClass.Decrypt(historyDTO.Sum));
            var value = DecryptClass.Decrypt(historyDTO.Value);
            var history = new History
            {
                Sum = sum,
                Value = value
            };
            return Ok(await Mediator.Send(new CreateHistory.Command {History = history, FromId = fromId, ToId = toId}));
        }
        
        [HttpGet]
        public async Task<ActionResult> GetWallets([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await context.Histories
                .Where(t => t.Wallet.Id.ToString() == filter.Id)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            if (pagedData == null)
            {
                return NotFound("Not Found!");
            }
            var totalRecords = await context.Wallets.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<History>(pagedData, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
    }
}