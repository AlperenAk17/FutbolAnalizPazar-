using Microsoft.AspNetCore.Mvc;
using FutbolAnalizPazari.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;

namespace FutbolAnalizPazari.Controllers
{
    public class BasvuruController : Controller
    {
        private readonly AnalizPlatformuContext _context;
        private readonly IWebHostEnvironment _env;

        public BasvuruController(AnalizPlatformuContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Sayfayı Aç
        [HttpGet]
        public IActionResult Yap(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Analist") return RedirectToAction("Login", "Account");
            var rapor = _context.Raporlars.Find(id);
            if (rapor == null) return RedirectToAction("Index", "Home");
            ViewBag.MacBilgisi = rapor.MacBilgisi;
            ViewBag.RaporID = id;
            return View();
        }

        // POST: Yükle (ÇÖKMEYEN VERSİYON)
        [HttpPost]
        public IActionResult Yap()
        {
            try
            {
                // 1. Önce En Basit Kontroller (Çökme yaratmaz)
                int? analizciId = HttpContext.Session.GetInt32("KullaniciID");
                if (analizciId == null) return RedirectToAction("Login", "Account");

                if (!Request.HasFormContentType) return Content("HATA: Form verisi yok (enctype eksik olabilir).");

                // 2. Verileri Manuel Çek
                var form = Request.Form;
                var dosyalar = form.Files;
                string? raporIdString = form["RaporID"];
                string? analistNotu = form["AnalistNotu"];

                if (dosyalar.Count == 0) return Content("HATA: Dosya seçilmedi.");
                if (string.IsNullOrEmpty(raporIdString)) return Content("HATA: Rapor ID yok.");

                // 3. Dosyayı Kaydet
                // (WebRootPath boş gelirse hata vermesin diye önlem)
                string kokDizin = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string klasorYolu = Path.Combine(kokDizin, "raporlar");

                if (!Directory.Exists(klasorYolu)) Directory.CreateDirectory(klasorYolu);

                var dosya = dosyalar[0];
                var yeniIsim = Guid.NewGuid() + Path.GetExtension(dosya.FileName);
                var tamYol = Path.Combine(klasorYolu, yeniIsim);

                using (var stream = new FileStream(tamYol, FileMode.Create))
                {
                    dosya.CopyTo(stream);
                }

                // 4. Veritabanı Nesnesini Oluştur
                // DİKKAT: Burada 'Rapor' veya 'Analizci' gibi nesne atamaları YAPMIYORUZ. Sadece ID.
                Basvurular b = new Basvurular();
                b.RaporID = int.Parse(raporIdString);
                b.AnalistNotu = analistNotu;
                b.DosyaYolu = yeniIsim;
                b.AnalizciID = analizciId.Value;
                b.Tarih = DateTime.Now;
                b.Durum = 0;

                _context.Basvurulars.Add(b);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return Content($"HATA OLUŞTU: {ex.Message} \n\n {ex.InnerException?.Message}");
            }
        }

        // (Listele ve Onayla metodlarını da buraya eklemeyi unutma, önceki kodlarda vardı)
        [HttpGet]
        public IActionResult Listele(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Takim") return RedirectToAction("Login", "Account");
            var basvurular = _context.Basvurulars.Where(x => x.RaporID == id).OrderByDescending(x => x.Tarih).ToList();
            var rapor = _context.Raporlars.Find(id);
            if (rapor != null) ViewBag.MacBasligi = rapor.MacBilgisi;
            return View(basvurular);
        }

        [HttpGet]
        public IActionResult Onayla(int id)
        {
            var basvuru = _context.Basvurulars.Find(id);
            if (basvuru != null)
            {
                basvuru.Durum = 1;
                // Puanlama kodlarını buraya ekleyebilirsin
                _context.SaveChanges();
            }
            return RedirectToAction("Listele", new { id = basvuru?.RaporID });
        }
        [HttpPost]
        public IActionResult PuanVer(int basvuruId, byte puan)
        {
            // 1. Başvuruyu Bul
            var basvuru = _context.Basvurulars.Find(basvuruId);

            // Eğer başvuru bulunamazsa ana sayfaya at (Güvenlik Önlemi)
            if (basvuru == null) return RedirectToAction("Index", "Home");

            // Rapor ID'sini kenara not et (Dönüşte lazım olacak)
            int raporId = basvuru.RaporID;

            // --- BAŞVURU GÜNCELLEME ---
            basvuru.Puan = puan;
            basvuru.Durum = 1;

            // 2. Analizciyi Bul
            var analizci = _context.AnalizciProfils.FirstOrDefault(x => x.AnalizciId == basvuru.AnalizciID);

            if (analizci != null)
            {
                // İstatistikleri güncelle
                analizci.YaptigiAnalizSay = (analizci.YaptigiAnalizSay ?? 0) + 1;
                analizci.OnayAnalizSay = (analizci.OnayAnalizSay ?? 0) + 1;

                // --- PUAN HESAPLAMA ---
                // Değişkenleri en üstte tanımlayalım ki "zaten tanımlı" hatası almayalım.
                decimal eskiOrtalama = analizci.AnalizPuan ?? 0;
                int yeniOnaySayisi = analizci.OnayAnalizSay ?? 1; // 0 gelirse 1 yap ki bölme hatası olmasın

                if (yeniOnaySayisi == 1)
                {
                    analizci.AnalizPuan = (decimal)puan;
                }
                else
                {
                    // Matematiksel hesaplama
                    decimal oncekiToplamPuan = eskiOrtalama * (yeniOnaySayisi - 1);
                    analizci.AnalizPuan = (oncekiToplamPuan + puan) / yeniOnaySayisi;
                }
            }

            // 3. Kaydet
            _context.SaveChanges();

            // İşlem bitince listeye geri dön
            return RedirectToAction("Listele", new { id = raporId });
        }
    }

}