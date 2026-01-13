using Microsoft.AspNetCore.Mvc;
using FutbolAnalizPazari.Models; // Models klasörünü tanýttýk
using System.Diagnostics;
namespace FutbolAnalizPazari.Controllers
{
    public class HomeController : Controller
    {
        // 1. Veritabaný Baðlantýsýný Tanýmlýyoruz
        private readonly AnalizPlatformuContext _context;

        // 2. Yapýcý Metot (Constructor) ile Baðlantýyý Ýçeri Alýyoruz
        public HomeController(AnalizPlatformuContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Artýk _context kullanýlabilir!
            var ilanlar = _context.Raporlars.ToList();
            return View(ilanlar);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // ==========================================
        // BAÐLANTI TESTÝ (GEÇÝCÝ)
        // ==========================================
        public IActionResult TestDb()
        {
            try
            {
                // 1. Veritabanýna baðlanabiliyor muyuz?
                bool baglantiVarMi = _context.Database.CanConnect();

                if (!baglantiVarMi)
                    return Content("HATA: Veritabanýna ulaþýlamýyor! ConnectionString yanlýþ.");

                // 2. Basvurular tablosu var mý?
                int basvuruSayisi = _context.Basvurulars.Count();

                return Content($"BAÞARILI! ?? \nSQL Baðlantýsý Saðlam. \nBasvurular Tablosunda {basvuruSayisi} kayýt var.");
            }
            catch (Exception ex)
            {
                return Content($"PATLADI! ?? \nHata Detayý: {ex.Message} \n {ex.InnerException?.Message}");
            }
        }
        // Keþfet sayfasý herkese açýk olsun istiyorsan baþýna [Authorize] KOYMA.
        [HttpGet]
        public IActionResult Kesfet()
        {
            return View();
        }
    }

}