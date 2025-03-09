using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using fitapp.Data;
using fitapp.Models;
using fitapp.Models.fitapp.Models;

namespace fitapp.Controllers
{
   
       [Route("api/contacts")]
        [ApiController]
        public class ContactsController : ControllerBase
        {
            private readonly FitnessAppContext _context;

            public ContactsController(FitnessAppContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
            {
                return await _context.Contacts.ToListAsync();
            }

            [HttpPost]
            public async Task<ActionResult<Contact>> AddContact(Contact contact)
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetContacts), new { id = contact.Id }, contact);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteContact(int id)
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                    return NotFound();

                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
 }


