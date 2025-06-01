using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;


namespace WebApplication2.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly AppDbContext _context;

        public OgrenciController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ogrenciler = await _context.Ogrenciler.Include(o => o.Bolum).ToListAsync();
            return View(ogrenciler);
        }

        public IActionResult Create()
        {
            ViewData["BolumID"] = new SelectList(_context.Bolumler, "BolumID", "BolumAd");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OgrenciID,Ad,Soyad,BolumID")] Ogrenci ogrenci)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ogrenci);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BolumID"] = new SelectList(_context.Bolumler, "BolumID", "BolumAd", ogrenci.BolumID);
            return View(ogrenci);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler
                .Include(o => o.Bolum)
                    .ThenInclude(b => b.Fakulte)
                .Include(o => o.OgrenciDersler)
                    .ThenInclude(od => od.Ders)
                .FirstOrDefaultAsync(m => m.OgrenciID == id);

            if (ogrenci == null)
            {
                return NotFound();
            }

            return View(ogrenci);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if (ogrenci == null)
            {
                return NotFound();
            }
            ViewData["BolumID"] = new SelectList(_context.Bolumler, "BolumID", "BolumAd", ogrenci.BolumID);
            return View(ogrenci);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OgrenciID,Ad,Soyad,BolumID")] Ogrenci ogrenci)
        {
            if (id != ogrenci.OgrenciID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ogrenci);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OgrenciExists(ogrenci.OgrenciID))
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
            ViewData["BolumID"] = new SelectList(_context.Bolumler, "BolumID", "BolumAd", ogrenci.BolumID);
            return View(ogrenci);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler
                .Include(o => o.Bolum)
                .FirstOrDefaultAsync(m => m.OgrenciID == id);
            if (ogrenci == null)
            {
                return NotFound();
            }

            return View(ogrenci);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            _context.Ogrenciler.Remove(ogrenci);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OgrenciDersleri(int? ogrenciID)
        {
            if (ogrenciID == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler
                .Include(o => o.OgrenciDersler)
                    .ThenInclude(od => od.Ders)
                .FirstOrDefaultAsync(o => o.OgrenciID == ogrenciID);

            if (ogrenci == null)
            {
                return NotFound();
            }

            return View(ogrenci);
        }

        public async Task<IActionResult> DersIstatistikleri(string yil, string yariyil)
        {
            var istatistikler = await _context.OgrenciDersler
                .Where(od => od.Yil == yil && od.Yariyil == yariyil)
                .GroupBy(od => od.Ders.DersAd)
                .Select(g => new { DersAd = g.Key, OgrenciSayisi = g.Count() })
                .ToListAsync();

            ViewBag.Yil = yil;
            ViewBag.Yariyil = yariyil;

            return View(istatistikler);
        }
        [HttpGet]
        public async Task<IActionResult> OgrenciDersListesi(int? ogrenciNo)
        {
            if (ogrenciNo == null)
            {
                return View(new OgrenciDersListesiViewModel());
            }

            var ogrenci = await _context.Ogrenciler
                .Include(o => o.OgrenciDersler)
                    .ThenInclude(od => od.Ders)
                .FirstOrDefaultAsync(o => o.OgrenciID == ogrenciNo);

            if (ogrenci == null)
            {
                ViewBag.ErrorMessage = "Öğrenci bulunamadı!";
                return View(new OgrenciDersListesiViewModel());
            }

            var model = new OgrenciDersListesiViewModel
            {
                OgrenciID = ogrenci.OgrenciID,
                AdSoyad = $"{ogrenci.Ad} {ogrenci.Soyad}",
                Dersler = ogrenci.OgrenciDersler.Select(od => new OgrenciDersDto
                {
                    DersAd = od.Ders.DersAd,
                    Yil = od.Yil,
                    Yariyil = od.Yariyil,
                    Vize = od.Vize,
                    Final = od.Final,
                    Ortalama = od.Vize.HasValue && od.Final.HasValue ?
                              (od.Vize * 0.4 + od.Final * 0.6) : null
                }).ToList()
            };

            return View(model);
        }


        private bool OgrenciExists(int id)
        {
            return _context.Ogrenciler.Any(e => e.OgrenciID == id);
        }
    }
    

   
}