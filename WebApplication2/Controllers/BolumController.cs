using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class BolumController : Controller
    {
        private readonly AppDbContext _context;

        public BolumController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bolumler = await _context.Bolumler.Include(b => b.Fakulte).ToListAsync();
            return View(bolumler);
        }

        public IActionResult Create()
        {
            ViewData["FakulteID"] = new SelectList(_context.Fakulteler, "FakulteID", "FakulteAd");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BolumID,BolumAd,FakulteID")] Bolum bolum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bolum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FakulteID"] = new SelectList(_context.Fakulteler, "FakulteID", "FakulteAd", bolum.FakulteID);
            return View(bolum);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bolum = await _context.Bolumler.FindAsync(id);
            if (bolum == null)
            {
                return NotFound();
            }
            ViewData["FakulteID"] = new SelectList(_context.Fakulteler, "FakulteID", "FakulteAd", bolum.FakulteID);
            return View(bolum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BolumID,BolumAd,FakulteID")] Bolum bolum)
        {
            if (id != bolum.BolumID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bolum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BolumExists(bolum.BolumID))
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
            ViewData["FakulteID"] = new SelectList(_context.Fakulteler, "FakulteID", "FakulteAd", bolum.FakulteID);
            return View(bolum);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bolum = await _context.Bolumler
                .Include(b => b.Fakulte)
                .FirstOrDefaultAsync(m => m.BolumID == id);
            if (bolum == null)
            {
                return NotFound();
            }

            return View(bolum);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bolum = await _context.Bolumler
                .Include(b => b.Fakulte)
                .Include(b => b.Ogrenciler)
                .FirstOrDefaultAsync(m => m.BolumID == id);

            if (bolum == null)
            {
                return NotFound();
            }

            ViewBag.OgrenciSayisi = bolum.Ogrenciler?.Count ?? 0;

            return View(bolum);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bolum = await _context.Bolumler.FindAsync(id);
            _context.Bolumler.Remove(bolum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BolumExists(int id)
        {
            return _context.Bolumler.Any(e => e.BolumID == id);
        }
    }
}