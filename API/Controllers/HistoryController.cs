using System.Threading.Tasks;
using API.Services;
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

        [HttpPost("{id}")]
        public async Task<IActionResult> CreateHistory([FromBody] )
        {
            
        }
    }
}