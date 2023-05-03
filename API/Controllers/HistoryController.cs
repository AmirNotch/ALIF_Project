using System.Threading.Tasks;
using API.Decryption;
using API.DTOs;
using API.Services;
using Application.Wallets;
using Domain;
using Microsoft.AspNetCore.Mvc;
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
        
        [HttpGet("{Id}")]
    }
}