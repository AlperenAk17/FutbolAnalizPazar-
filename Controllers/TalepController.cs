using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Dropdown (SelectListItem) için gerekli
using FutbolAnalizPazari.Models;

namespace FutbolAnalizPazari.Controllers
{
    public class TalepController : Controller
    {
        private readonly AnalizPlatformuContext _context;

        public TalepController(AnalizPlatformuContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. TALEP OLUŞTURMA SAYFASI (GET)
        // ==========================================
        [HttpGet]
        public IActionResult Olustur()
        {
            // Önce: Giriş yapmış mı kontrol et?
            if (HttpContext.Session.GetString("Rol") != "Takim")
            {
                // Takım değilse veya giriş yapmamışsa Ana Sayfaya at
                return RedirectToAction("Login", "Account");
            }

            // Kategorileri Veritabanından Çek (Dropdown için)
            // AnalizKategori tablosundaki ID ve Ad bilgilerini alıyoruz
            List<SelectListItem> kategoriListesi = (from x in _context.AnalizKategori.ToList()
                                                    select new SelectListItem
                                                    {
                                                        Text = x.KategoriAd,
                                                        Value = x.KatergoriId.ToString() // Tablonda "KatergoriID" yazmıştın
                                                    }).ToList();

            // Bu listeyi sayfaya (View) taşıyalım
            ViewBag.Kategoriler = kategoriListesi;

            return View();
        }

        // ==========================================
        // 2. TALEBİ KAYDETME (POST)
        // ==========================================
        [HttpPost]
        public IActionResult Olustur(Raporlar p)
        {
            // Giriş yapan Takımın ID'sini al (Session'dan)
            int? takimId = HttpContext.Session.GetInt32("KullaniciID");

            if (takimId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Eksik bilgileri tamamla
            p.MusteriId = takimId;  

            // Veritabanına kaydet
            _context.Raporlars.Add(p);
            _context.SaveChanges();

            // İş bitince Ana Sayfaya veya İlanlarım sayfasına git
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Taleplerim()
        {
            int? takimId = HttpContext.Session.GetInt32("KullaniciID");

            // Eğer giriş yapmamışsa Login'e at
            if (takimId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // İlanları çek
            var ilanlar = _context.Raporlars
                                  .Where(x => x.MusteriId == takimId)
                                  .OrderByDescending(x => x.SonTeslimTarihi)
                                  .ToList();

            return View(ilanlar);
        }
    }


}