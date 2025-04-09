using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PiwkoMozna.Data;
using PiwkoMozna.Models;

namespace PiwkoMozna.Controllers
{
    public class BrowarModelController : Controller
    {
        private readonly PiwkoMoznaContext _context;

        public BrowarModelController(PiwkoMoznaContext context)
        {
            _context = context;
        }

        // GET: BrowarModel
        public async Task<IActionResult> Index()
        {            
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                return _context.BrowarModel != null ? 
                        View(await _context.BrowarModel.ToListAsync()) :
                        Problem("Entity set 'PiwkoMoznaContext.BrowarModel'  is null.");
            }
            return RedirectToAction("Index", "Home");            
        }

        // GET: BrowarModel/Details/5
        public async Task<IActionResult> Details(string id)
        {
            
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.BrowarModel == null)
                {
                    return NotFound();
                }

                var browarModel = await _context.BrowarModel
                    .FirstOrDefaultAsync(m => m.BreweryName == id);
                if (browarModel == null)
                {
                    return NotFound();
                }

                return View(browarModel);
            }
            return RedirectToAction("Index", "Home");
            
        }

        // GET: BrowarModel/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
            
        }

        // POST: BrowarModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BreweryName,City,Country,Founded,Description")] BrowarModel browarModel)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (ModelState.IsValid)
                {
                    _context.Add(browarModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(browarModel);
            }
            return RedirectToAction("Index", "Home");
            
        }

        // GET: BrowarModel/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.BrowarModel == null)
                {
                    return NotFound();
                }

                var browarModel = await _context.BrowarModel.FindAsync(id);
                if (browarModel == null)
                {
                    return NotFound();
                }
                return View(browarModel);
            }
            return RedirectToAction("Index", "Home");

        }

        // POST: BrowarModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BreweryName,City,Country,Founded,Description")] BrowarModel browarModel)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id != browarModel.BreweryName)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(browarModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BrowarModelExists(browarModel.BreweryName))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(browarModel);
            }
            return RedirectToAction("Index", "Home");
           
        }

        // GET: BrowarModel/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.BrowarModel == null)
                {
                    return NotFound();
                }

                var browarModel = await _context.BrowarModel
                    .FirstOrDefaultAsync(m => m.BreweryName == id);
                if (browarModel == null)
                {
                    return NotFound();
                }

                return View(browarModel);
            }
            return RedirectToAction("Index", "Home");             
        }

        // POST: BrowarModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (_context.BrowarModel == null)
                {
                    return Problem("Entity set 'PiwkoMoznaContext.BrowarModel'  is null.");
                }
                var browarModel = await _context.BrowarModel.FindAsync(id);
                if (browarModel != null)
                {
                    _context.BrowarModel.Remove(browarModel);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");          
        }

        private bool BrowarModelExists(string id)
        {
          return (_context.BrowarModel?.Any(e => e.BreweryName == id)).GetValueOrDefault();
        }
    }
}
