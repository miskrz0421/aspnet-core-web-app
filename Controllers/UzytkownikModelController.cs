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
    public class UzytkownikModelController : Controller
    {
        private readonly PiwkoMoznaContext _context;

        public UzytkownikModelController(PiwkoMoznaContext context)
        {
            _context = context;
        }

        // GET: UzytkownikModel
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
              return _context.UzytkownikModel != null ? 
                          View(await _context.UzytkownikModel.ToListAsync()) :
                          Problem("Entity set 'PiwkoMoznaContext.UzytkownikModel'  is null.");
            }
            return RedirectToAction("Index", "Home");

        }

        // GET: UzytkownikModel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.UzytkownikModel == null)
                {
                    return NotFound();
                }

                var uzytkownikModel = await _context.UzytkownikModel
                    .FirstOrDefaultAsync(m => m.UserID == id);
                if (uzytkownikModel == null)
                {
                    return NotFound();
                }

                return View(uzytkownikModel);       
            }
            return RedirectToAction("Index", "Home");

        }

        // GET: UzytkownikModel/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: UzytkownikModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,IsAdmin, Token")] UzytkownikModel uzytkownikModel)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (ModelState.IsValid)
                {
                    uzytkownikModel.Password = PiwkoMozna.Commons.Hash.CalculateMD5Hash(uzytkownikModel.Password);
                    if(uzytkownikModel.IsAdmin){
                        uzytkownikModel.Token = PiwkoMozna.Commons.Tokens.GenerateToken();
                    }
                    _context.Add(uzytkownikModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine("\n");
                            Console.WriteLine(error.ErrorMessage);
                            Console.WriteLine("\n");
                        }
                    }
                }
                return View(uzytkownikModel);
            }
            return RedirectToAction("Index", "Home");
        }
        // GET: UzytkownikModel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.UzytkownikModel == null)
                {
                    return NotFound();
                }

                var uzytkownikModel = await _context.UzytkownikModel.FindAsync(id);
                if (uzytkownikModel == null)
                {
                    return NotFound();
                }
                return View(uzytkownikModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: UzytkownikModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,IsAdmin,Token")] UzytkownikModel uzytkownikModel)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id != uzytkownikModel.UserID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(uzytkownikModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UzytkownikModelExists(uzytkownikModel.UserID))
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
                return View(uzytkownikModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: UzytkownikModel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.UzytkownikModel == null)
                {
                    return NotFound();
                }

                var uzytkownikModel = await _context.UzytkownikModel
                    .FirstOrDefaultAsync(m => m.UserID == id);
                if (uzytkownikModel == null)
                {
                    return NotFound();
                }

                return View(uzytkownikModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: UzytkownikModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (_context.UzytkownikModel == null)
                {
                    return Problem("Entity set 'PiwkoMoznaContext.UzytkownikModel'  is null.");
                }
                var uzytkownikModel = await _context.UzytkownikModel.FindAsync(id);
                if (uzytkownikModel != null)
                {
                    _context.UzytkownikModel.Remove(uzytkownikModel);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");
        }

        private bool UzytkownikModelExists(int id)
        {
          return (_context.UzytkownikModel?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
    }
}
