using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class DersAtamaController : Controller
    {
        private readonly AppDbContext _context;

        public DersAtamaController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           
            ViewBag.Ogrenciler = new SelectList(
                _context.Ogrenciler.Select(o => new {
                    o.OgrenciID,
                    AdSoyad = o.Ad + " " + o.Soyad
                }).ToList(),
                "OgrenciID",
                "AdSoyad"
            );

            ViewBag.Dersler = new SelectList(_context.Dersler.ToList(), "DersID", "DersAd");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int ogrenciID, int dersID, string yil, string yariyil)
        {
            var atama = new OgrenciDers
            {
                OgrenciID = ogrenciID,
                DersID = dersID,
                Yil = yil,
                Yariyil = yariyil
            };

            _context.OgrenciDersler.Add(atama);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NotKaydet(int dersID, Dictionary<int, (int? vize, int? final)> notlar)
        {
            foreach (var not in notlar)
            {
                var ogrenciDers = await _context.OgrenciDersler
                    .FirstOrDefaultAsync(od => od.OgrenciID == not.Key && od.DersID == dersID);

                if (ogrenciDers != null)
                {
                    ogrenciDers.Vize = not.Value.vize;
                    ogrenciDers.Final = not.Value.final;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("DersOgrencileri", "Ders", new { id = dersID });
        }

        public async Task<IActionResult> Sil(int ogrenciID, int dersID)
        {
            var atama = await _context.OgrenciDersler
                .FirstOrDefaultAsync(od => od.OgrenciID == ogrenciID && od.DersID == dersID);

            if (atama != null)
            {
                _context.OgrenciDersler.Remove(atama);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("OgrenciDersleri", "Ogrenci", new { ogrenciID });
        }
    }
}