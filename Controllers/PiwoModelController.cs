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
    public class PiwoModelController : Controller
    {
        private readonly PiwkoMoznaContext _context;

        public PiwoModelController(PiwkoMoznaContext context)
        {
            _context = context;
        }

        // GET: PiwoModel
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                var piwkoMoznaContext = _context.PiwoModel.Include(p => p.BrowarModel);
                return View(await piwkoMoznaContext.ToListAsync());
            }
            return RedirectToAction("Index", "Home");
            
        }

        public async Task<IActionResult> Katalog(string BeerName, string BreweryName, string Style, double? MinABV, double? MaxABV, double? MinRating, double? MaxRating, string SortBy)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                var piwkoMoznaContext = _context.PiwoModel.Include(p => p.BrowarModel).AsQueryable();

                if (!string.IsNullOrEmpty(BeerName))
                {
                    piwkoMoznaContext = piwkoMoznaContext.Where(p => p.BeerName == BeerName);
                }
                if (!string.IsNullOrEmpty(BreweryName))
                {
                    piwkoMoznaContext = piwkoMoznaContext.Where(p => p.BrowarModel.BreweryName == BreweryName);
                }
                if (!string.IsNullOrEmpty(Style))
                {
                    piwkoMoznaContext = piwkoMoznaContext.Where(p => p.Style == Style);
                }
                if (MinABV.HasValue)
                {
                    piwkoMoznaContext = piwkoMoznaContext.Where(p => p.ABV >= MinABV.Value);
                }
                if (MaxABV.HasValue)
                {
                    piwkoMoznaContext = piwkoMoznaContext.Where(p => p.ABV <= MaxABV.Value);
                }
                if (MinRating.HasValue)
                {
                    piwkoMoznaContext = piwkoMoznaContext.Where(p => p.AverageRating >= MinRating.Value);
                }
                if (MaxRating.HasValue)
                {
                    piwkoMoznaContext = piwkoMoznaContext.Where(p => p.AverageRating <= MaxRating.Value);
                }

                switch (SortBy)
                {
                    case "ABVAsc":
                        piwkoMoznaContext = piwkoMoznaContext.OrderBy(p => p.ABV);
                        break;
                    case "ABVDesc":
                        piwkoMoznaContext = piwkoMoznaContext.OrderByDescending(p => p.ABV);
                        break;
                    case "RatingAsc":
                        piwkoMoznaContext = piwkoMoznaContext.OrderBy(p => p.AverageRating);
                        break;
                    case "RatingDesc":
                        piwkoMoznaContext = piwkoMoznaContext.OrderByDescending(p => p.AverageRating);
                        break;
                }

                ViewBag.BeerNames = await _context.PiwoModel.Select(p => p.BeerName).Distinct().ToListAsync();
                ViewBag.BreweryNames = await _context.BrowarModel.Select(b => b.BreweryName).Distinct().ToListAsync();
                ViewBag.Styles = await _context.PiwoModel.Select(p => p.Style).Distinct().ToListAsync();

                ViewBag.CurrentBeerName = BeerName;
                ViewBag.CurrentBreweryName = BreweryName;
                ViewBag.CurrentStyle = Style;
                ViewBag.CurrentMinABV = MinABV;
                ViewBag.CurrentMaxABV = MaxABV;
                ViewBag.CurrentMinRating = MinRating;
                ViewBag.CurrentMaxRating = MaxRating;
                ViewBag.CurrentSortBy = SortBy;

                return View(await piwkoMoznaContext.ToListAsync());
            }

            return RedirectToAction("Login", "Account");
        }


        public async Task<IActionResult> KatalogRecenzje(string ? BeerName)
        {
            if(HttpContext.Session.GetString("IsLoggedIn") == "true") 
            {
                ViewData["BeerNameRecenzja"] = BeerName;
                var piwkoMoznaContext = _context.RecenzjaModel.Include(r => r.PiwoModel).Include(r => r.UzytkownikModel).Where(r => r.PiwoModel.BeerName == BeerName);               
                return View(await piwkoMoznaContext.ToListAsync());
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> KatalogBrowarow(string ? BreweryName)
        {
            if(HttpContext.Session.GetString("IsLoggedIn") == "true") 
            {
                var browarModel = _context.BrowarModel.Where(m =>m.BreweryName==BreweryName);
                    
                return View(browarModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: PiwoModel/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.PiwoModel == null)
                {
                    return NotFound();
                }

                var piwoModel = await _context.PiwoModel
                    .Include(p => p.BrowarModel)
                    .FirstOrDefaultAsync(m => m.BeerName == id);
                if (piwoModel == null)
                {
                    return NotFound();
                }

                return View(piwoModel);
            }
            return RedirectToAction("Index", "Home");            
        }

        // GET: PiwoModel/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                ViewData["BreweryName"] = new SelectList(_context.BrowarModel, "BreweryName", "BreweryName");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: PiwoModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BeerName,BreweryName,Style,ABV,AverageRating")] PiwoModel piwoModel)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (ModelState.IsValid)
                {
                    _context.Add(piwoModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["BreweryName"] = new SelectList(_context.BrowarModel, "BreweryName", "BreweryName", piwoModel.BreweryName);
                return View(piwoModel);
            }
            return RedirectToAction("Index", "Home");

        }

        // GET: PiwoModel/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.PiwoModel == null)
                {
                    return NotFound();
                }

                var piwoModel = await _context.PiwoModel.FindAsync(id);
                if (piwoModel == null)
                {
                    return NotFound();
                }
                ViewData["BreweryName"] = new SelectList(_context.BrowarModel, "BreweryName", "BreweryName", piwoModel.BreweryName);
                return View(piwoModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: PiwoModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BeerName,BreweryName,Style,ABV,AverageRating")] PiwoModel piwoModel)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id != piwoModel.BeerName)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(piwoModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PiwoModelExists(piwoModel.BeerName))
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
                ViewData["BreweryName"] = new SelectList(_context.BrowarModel, "BreweryName", "BreweryName", piwoModel.BreweryName);
                return View(piwoModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: PiwoModel/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.PiwoModel == null)
                {
                    return NotFound();
                }

                var piwoModel = await _context.PiwoModel
                    .Include(p => p.BrowarModel)
                    .FirstOrDefaultAsync(m => m.BeerName == id);
                if (piwoModel == null)
                {
                    return NotFound();
                }

                return View(piwoModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: PiwoModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (_context.PiwoModel == null)
                {
                    return Problem("Entity set 'PiwkoMoznaContext.PiwoModel'  is null.");
                }
                var piwoModel = await _context.PiwoModel.FindAsync(id);
                if (piwoModel != null)
                {
                    _context.PiwoModel.Remove(piwoModel);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");
        }

        private bool PiwoModelExists(string id)
        {
          return (_context.PiwoModel?.Any(e => e.BeerName == id)).GetValueOrDefault();
        }
    }
}
