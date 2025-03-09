using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fitapp.Data;
using fitapp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace fitapp.Controllers
{
   
    
        [Route("api/messages")]
        [ApiController]
        public class MessagesController : ControllerBase
        {
            private readonly FitnessAppContext _context;

            public MessagesController(FitnessAppContext context)
            {
                _context = context;
            }

            [HttpGet("{userId}")]
            public async Task<ActionResult<IEnumerable<Message>>> GetMessages(int userId)
            {
                return await _context.Messages
                    .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                    .OrderByDescending(m => m.Timestamp)
                    .ToListAsync();
            }

            [HttpPost]
            public async Task<ActionResult<Message>> SendMessage(Message message)
            {
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetMessages), new { userId = message.SenderId }, message);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> MarkAsRead(int id)
            {
                var message = await _context.Messages.FindAsync(id);
                if (message == null)
                    return NotFound();

                message.IsRead = true;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }


