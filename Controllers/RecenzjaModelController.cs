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
    public class RecenzjaModelController : Controller
    {
        private readonly PiwkoMoznaContext _context;

        public RecenzjaModelController(PiwkoMoznaContext context)
        {
            _context = context;
        }

        // GET: RecenzjaModel
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                var piwkoMoznaContext = _context.RecenzjaModel.Include(r => r.PiwoModel).Include(r => r.UzytkownikModel);
                return View(await piwkoMoznaContext.ToListAsync());
            }
            return RedirectToAction("Index", "Home");

        }

        // GET: RecenzjaModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.RecenzjaModel == null)
                {
                    return NotFound();
                }

                var recenzjaModel = await _context.RecenzjaModel
                    .Include(r => r.PiwoModel)
                    .Include(r => r.UzytkownikModel)
                    .FirstOrDefaultAsync(m => m.ReviewID == id);
                if (recenzjaModel == null)
                {
                    return NotFound();
                }

                return View(recenzjaModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: RecenzjaModel/Create
        public IActionResult Create(string ? BeerName)
        {
            if(HttpContext.Session.GetString("IsLoggedIn") == "true") 
            {
                ViewData["BeerName"] = new SelectList(_context.PiwoModel, "BeerName", "BeerName");
                ViewData["UserID"] = new SelectList(_context.UzytkownikModel, "UserID", "Email");
                ViewData["UserIDRecenzja"] = _context.UzytkownikModel.FirstOrDefault(m => m.Email == HttpContext.Session.GetString("email")).UserID;                
                ViewData["BeerNameRecenzja"] = BeerName;
            
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: RecenzjaModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,BeerName,Rating,Comment,ReviewDate")] RecenzjaModel recenzjaModel)
        {
            if(HttpContext.Session.GetString("IsLoggedIn") == "true") 
            {
                if (ModelState.IsValid)
                {                                    
                    recenzjaModel.ReviewDate = DateTime.Now;
                    _context.Add(recenzjaModel);
                    await _context.SaveChangesAsync();

                    var averageRating = _context.RecenzjaModel
                    .Where(r => r.BeerName == recenzjaModel.BeerName)
                    .Average(r => r.Rating);

                    var beer = _context.PiwoModel.FirstOrDefault(p => p.BeerName == recenzjaModel.BeerName);
                    if (beer != null)
                    {
                        beer.AverageRating = averageRating;
                        _context.Update(beer);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("KatalogRecenzje","PiwoModel",new { beerName = recenzjaModel.BeerName });
                }
                ViewData["BeerName"] = new SelectList(_context.PiwoModel, "BeerName", "BeerName", recenzjaModel.BeerName);
                ViewData["UserID"] = new SelectList(_context.UzytkownikModel, "UserID", "Email", recenzjaModel.UserID);
                return View(recenzjaModel);
            }
            return RedirectToAction("Index", "Home");

        }

        // GET: RecenzjaModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.RecenzjaModel == null)
                {
                    return NotFound();
                }

                var recenzjaModel = await _context.RecenzjaModel.FindAsync(id);
                if (recenzjaModel == null)
                {
                    return NotFound();
                }
                ViewData["BeerName"] = new SelectList(_context.PiwoModel, "BeerName", "BeerName", recenzjaModel.BeerName);
                ViewData["UserID"] = new SelectList(_context.UzytkownikModel, "UserID", "Email", recenzjaModel.UserID);
                return View(recenzjaModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: RecenzjaModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewID,UserID,BeerName,Rating,Comment,ReviewDate")] RecenzjaModel recenzjaModel)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id != recenzjaModel.ReviewID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(recenzjaModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RecenzjaModelExists(recenzjaModel.ReviewID))
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
                ViewData["BeerName"] = new SelectList(_context.PiwoModel, "BeerName", "BeerName", recenzjaModel.BeerName);
                ViewData["UserID"] = new SelectList(_context.UzytkownikModel, "UserID", "Email", recenzjaModel.UserID);
                return View(recenzjaModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: RecenzjaModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.RecenzjaModel == null)
                {
                    return NotFound();
                }

                var recenzjaModel = await _context.RecenzjaModel
                    .Include(r => r.PiwoModel)
                    .Include(r => r.UzytkownikModel)
                    .FirstOrDefaultAsync(m => m.ReviewID == id);
                if (recenzjaModel == null)
                {
                    return NotFound();
                }

                return View(recenzjaModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: RecenzjaModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (_context.RecenzjaModel == null)
                {
                    return Problem("Entity set 'PiwkoMoznaContext.RecenzjaModel'  is null.");
                }
                var recenzjaModel = await _context.RecenzjaModel.FindAsync(id);
                if (recenzjaModel != null)
                {
                    _context.RecenzjaModel.Remove(recenzjaModel);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");
        }

        private bool RecenzjaModelExists(int id)
        {
          return (_context.RecenzjaModel?.Any(e => e.ReviewID == id)).GetValueOrDefault();
        }
    }
}
