using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PiwkoMozna.Data;
using PiwkoMozna.Models;

namespace PiwkoMozna.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrowarModelControllerApi : ControllerBase
    {
        private readonly PiwkoMoznaContext _context;

        public BrowarModelControllerApi(PiwkoMoznaContext context)
        {
            _context = context;
        }

        // GET: api/BrowarModelControllerApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrowarModel>>> GetBrowarModel()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            var email = HttpContext.Request.Headers["Email"];
            var userContext = _context.UzytkownikModel.Where(p=>p.Email==email.ToString()).Where(m => m.Token == accessToken.ToString()).Any();
            if (userContext && accessToken.ToString()!="0")
            {
                if (_context.BrowarModel == null)
                {
                    return NotFound();
                }
                return await _context.BrowarModel.ToListAsync();
            }
            else
            {
                return Problem("Autoryzacja się nie powiodła.");
            }
        }

        // GET: api/BrowarModelControllerApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BrowarModel>> GetBrowarModel(string id)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            var email = HttpContext.Request.Headers["Email"];
            var userContext = _context.UzytkownikModel.Where(p=>p.Email==email.ToString()).Where(m => m.Token == accessToken.ToString()).Any();
            if (userContext && accessToken.ToString()!="0")
            {
                
                if (_context.BrowarModel == null)
                {
                    return NotFound();
                }
                var browarModel = await _context.BrowarModel.FindAsync(id);

                if (browarModel == null)
                {
                    return NotFound();
                }

                return browarModel;
            }
            else
            {
                return Problem("Autoryzacja się nie powiodła.");
            }
        }

        // PUT: api/BrowarModelControllerApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrowarModel(string id, BrowarModel browarModel)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            var email = HttpContext.Request.Headers["Email"];
            var userContext = _context.UzytkownikModel.Where(p=>p.Email==email.ToString()).Where(m => m.Token == accessToken.ToString()).Any();
            if (userContext && accessToken.ToString()!="0")
            {
                if (id != browarModel.BreweryName)
                {
                    return BadRequest();
                }
                _context.Entry(browarModel).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrowarModelExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            else
            {
                return Problem("Autoryzacja się nie powiodła.");
            }
        }

        // POST: api/BrowarModelControllerApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        
        public async Task<ActionResult<BrowarModel>> PostBrowarModel(BrowarModel browarModel)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            var email = HttpContext.Request.Headers["Email"];
            var userContext = _context.UzytkownikModel.Where(p=>p.Email==email.ToString()).Where(m => m.Token == accessToken.ToString()).Any();
            if (userContext && accessToken.ToString()!="0")
            {
                if (_context.BrowarModel == null)
                {
                    return Problem("Entity set 'PiwkoMoznaContext.BrowarModel'  is null.");
                }

                _context.BrowarModel.Add(browarModel);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (BrowarModelExists(browarModel.BreweryName))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetBrowarModel", new { id = browarModel.BreweryName }, browarModel);

            }
            else
            {
                return Problem("Autoryzacja się nie powiodła.");
            }
            

        }

        // DELETE: api/BrowarModelControllerApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrowarModel(string id)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            var email = HttpContext.Request.Headers["Email"];
            var userContext = _context.UzytkownikModel.Where(p=>p.Email==email.ToString()).Where(m => m.Token == accessToken.ToString()).Any();
            if (userContext && accessToken.ToString()!="0")
            {
                if (_context.BrowarModel == null)
                {
                    return NotFound();
                }
                var browarModel = await _context.BrowarModel.FindAsync(id);
                if (browarModel == null)
                {
                    return NotFound();
                }

                _context.BrowarModel.Remove(browarModel);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return Problem("Autoryzacja się nie powiodła.");
            }
        }

        private bool BrowarModelExists(string id)
        {
            return (_context.BrowarModel?.Any(e => e.BreweryName == id)).GetValueOrDefault();
        }
    }
}
