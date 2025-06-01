using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class FakulteController : Controller
    {
        private readonly AppDbContext _context;

        public FakulteController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Fakulteler.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FakulteID,FakulteAd")] Fakulte fakulte)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fakulte);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fakulte);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fakulte = await _context.Fakulteler.FindAsync(id);
            if (fakulte == null)
            {
                return NotFound();
            }
            return View(fakulte);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FakulteID,FakulteAd")] Fakulte fakulte)
        {
            if (id != fakulte.FakulteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fakulte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FakulteExists(fakulte.FakulteID))
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
            return View(fakulte);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fakulte = await _context.Fakulteler
                .FirstOrDefaultAsync(m => m.FakulteID == id);
            if (fakulte == null)
            {
                return NotFound();
            }

            return View(fakulte);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fakulte = await _context.Fakulteler.FindAsync(id);
            _context.Fakulteler.Remove(fakulte);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FakulteExists(int id)
        {
            return _context.Fakulteler.Any(e => e.FakulteID == id);
        }
    }
}