using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebApplication2.Data;
using WebApplication2.Models;

using WebApplication2.Models.ViewModels;
namespace WebApplication2.Controllers
{
    public class DersController : Controller
    {
        private readonly AppDbContext _context;

        public DersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Dersler.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DersAd")] Ders ders)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(ders);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"{ders.DersAd} dersi başarıyla oluşturuldu!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                }
            }

            return View(ders);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ders = await _context.Dersler.FindAsync(id);
            if (ders == null)
            {
                return NotFound();
            }
            return View(ders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DersID,DersAd")] Ders ders)
        {
            if (id != ders.DersID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DersExists(ders.DersID))
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
            return View(ders);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ders = await _context.Dersler
                .FirstOrDefaultAsync(m => m.DersID == id);
            if (ders == null)
            {
                return NotFound();
            }

            return View(ders);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ders = await _context.Dersler.FindAsync(id);
            _context.Dersler.Remove(ders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> DersOgrencileri(int? id)
        {
            if (id == null) return NotFound();

            var ders = await _context.Dersler
                .Include(d => d.OgrenciDersler)
                    .ThenInclude(od => od.Ogrenci)
                .FirstOrDefaultAsync(d => d.DersID == id);

            if (ders == null) return NotFound();

            return View(ders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NotGuncelle(int dersId, Dictionary<int, NotGirisModel> notlar)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("DersOgrencileri", new { id = dersId });
            }

            foreach (var not in notlar)
            {
                var ogrenciDers = await _context.OgrenciDersler
                    .FirstOrDefaultAsync(od => od.OgrenciID == not.Key && od.DersID == dersId);

                if (ogrenciDers != null)
                {
                    ogrenciDers.Vize = not.Value.Vize;
                    ogrenciDers.Final = not.Value.Final;
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Notlar başarıyla güncellendi!";
            return RedirectToAction("DersOgrencileri", new { id = dersId });
        }

        [HttpGet]
        public async Task<IActionResult> DersIstatistikleri(string yil = null, string yariyil = null)
        {
            var istatistikler = await _context.OgrenciDersler
                .Where(od =>
                    (string.IsNullOrEmpty(yil) || od.Yil == yil) &&
                    (string.IsNullOrEmpty(yariyil) || od.Yariyil == yariyil))
                .GroupBy(od => od.Ders.DersAd)
                .Select(g => new DersIstatistikViewModel
                {
                    DersAd = g.Key,
                    OgrenciSayisi = g.Count(),
                    BasariOrani = g.Average(od =>
                        (od.Vize * 0.4 + od.Final * 0.6) >= 50 ? 1 : 0) * 100
                })
                .OrderByDescending(x => x.OgrenciSayisi)
                .ToListAsync();

            var model = new DersIstatistikListViewModel
            {
                Yil = yil,
                Yariyil = yariyil,
                Istatistikler = istatistikler,
                Yillar = await _context.OgrenciDersler.Select(od => od.Yil).Distinct().ToListAsync(),
                Yariyillar = new List<string> { "Güz", "Bahar", "Yaz" }
            };

            return View(model);
        }

        
        

        private bool DersExists(int id)
        {
            return _context.Dersler.Any(e => e.DersID == id);
        }
    }
}