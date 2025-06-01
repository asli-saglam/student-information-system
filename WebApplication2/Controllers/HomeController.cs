using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.OgrenciSayisi = _context.Ogrenciler.Count();
            ViewBag.DersSayisi = _context.Dersler.Count();
            ViewBag.BolumSayisi = _context.Bolumler.Count();
            ViewBag.FakulteSayisi = _context.Fakulteler.Count();
            return View();
        }
    }
}