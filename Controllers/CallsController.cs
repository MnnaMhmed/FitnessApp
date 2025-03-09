using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fitapp.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using fitapp.Models;
namespace fitapp.Controllers
{

    namespace MyApp.Controllers
    {
        [Route("api/calls")]
        [ApiController]
        public class CallsController : ControllerBase
        {
            private readonly FitnessAppContext _context;

            public CallsController(FitnessAppContext context)
            {
                _context = context;
            }

            [HttpGet("{userId}")]
            public async Task<ActionResult<IEnumerable<Call>>> GetCallHistory(int userId)
            {
                return await _context.Calls
                    .Where(c => c.CallerId == userId || c.ReceiverId == userId)
                    .OrderByDescending(c => c.CallTime)
                    .ToListAsync();
            }

            [HttpPost]
            public async Task<ActionResult<Call>> AddCall(Call call)
            {
                _context.Calls.Add(call);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCallHistory), new { userId = call.CallerId }, call);
            }
        }
    }
}
    

