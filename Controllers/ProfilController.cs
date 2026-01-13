using Microsoft.AspNetCore.Mvc;
using FutbolAnalizPazari.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using System;

namespace FutbolAnalizPazari.Controllers
{
    public class ProfilController : Controller
    {
        private readonly AnalizPlatformuContext _context;

        public ProfilController(AnalizPlatformuContext context)
        {
            _context = context;
        }

        // ---------------------------------------------------------
        // DASHBOARD (ANALİST PANELİ) - DÜZELTİLDİ
        // ---------------------------------------------------------
        public IActionResult Dashboard()
        {
            // 1. HATA BURADAYDI: "Id" değil "KullaniciID" olarak ve int olarak çekiyoruz.
            var userId = HttpContext.Session.GetInt32("KullaniciID");

            // Eğer oturum düşmüşse veya giriş yapılmamışsa Login'e at
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Analist Profilini Getir (userId.Value diyerek int değerini alıyoruz)
            var analist = _context.AnalizciProfils.Find(userId.Value);

            // Eğer ID var ama veritabanında böyle bir analist yoksa (Belki Takım hesabıyla girmeye çalıştı)
            if (analist == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // 3. İSTATİSTİKLER
            // Bekleyen iş sayısı (Durum 0 ise bekliyor demektir)
            int bekleyenIs = _context.Basvurulars
                                     .Count(x => x.AnalizciID == userId.Value && (x.Durum == 0 || x.Durum == null));

            // 4. GEÇMİŞ İŞLERİ HAZIRLA
            var gecmisIsler = (from b in _context.Basvurulars
                               join r in _context.Raporlars on b.RaporID equals r.RaporId
                               where b.AnalizciID == userId.Value
                               orderby b.Tarih descending
                               select new IsModeli
                               {
                                   BasvuruID = b.BasvuruID,
                                   MacBilgisi = r.MacBilgisi,
                                   Tarih = b.Tarih,
                                   Durum = b.Durum,
                                   Puan = b.Puan,
                                   DosyaYolu = b.DosyaYolu
                               }).Take(5).ToList();

            // 5. İŞ HAVUZU (Müsait Olan İşler)
            var isFirsatlari = (from r in _context.Raporlars
                                join m in _context.Musterilers on r.MusteriId equals m.MusteriID
                                orderby r.SonTeslimTarihi descending
                                select new IsFirsatiModeli
                                {
                                    RaporID = r.RaporId,
                                    TakimAdi = m.SirketAd,
                                    Lig = m.Seviye,
                                    MacBilgisi = r.MacBilgisi,
                                    SonTarih = r.SonTeslimTarihi
                                }).Take(6).ToList();

            // Verileri paketleyip View'a gönder
            var model = new DashboardViewModel
            {
                Analist = analist,
                GecmisIsler = gecmisIsler,
                IsFirsatlari = isFirsatlari,
                BekleyenIsSayisi = bekleyenIs
            };

            return View(model);
        }

        // Diğer metodların (Index, Ara, Goruntule vs.) buraya eklenebilir veya eski dosyanızda varsa kalabilir.
        // Ben sadece Dashboard hatasını düzelttim, diğerlerini (Index, Ara) silmediysen onlar da çalışır.

        [HttpGet]
        public IActionResult Ara(string q)
        {
            if (string.IsNullOrEmpty(q)) return RedirectToAction("Index", "Home");

            ViewBag.ArananKelime = q;

            // DÜZELTME: Önce "!= null" diyerek dolu olup olmadığına bakıyoruz.
            var sonuclar = _context.AnalizciProfils
                .Where(x => (x.Ad != null && x.Ad.Contains(q)) ||
                            (x.Soyad != null && x.Soyad.Contains(q)) ||
                            (x.CalistigiKulup != null && x.CalistigiKulup.Contains(q)))
                .ToList();

            return View(sonuclar);
        }
        [HttpGet]
        public IActionResult Goruntule(int id)
        {
            var profil = _context.AnalizciProfils.Find(id);
            if (profil == null) return RedirectToAction("Index", "Home");

            // Profildeki kişinin geçmiş biten işlerini (Onaylananları) çekelim
            var bitenIsler = (from b in _context.Basvurulars
                              join r in _context.Raporlars on b.RaporID equals r.RaporId
                              where b.AnalizciID == id && b.Durum == 1 // Sadece onaylılar
                              orderby b.Tarih descending
                              select b).ToList();

            ViewBag.BitenIsler = bitenIsler;

            return View(profil);
        }

        [HttpGet]
        public IActionResult Kesfet()
        {
            // Basit bir listeleme sayfası, tüm analistleri getirir
            var analistler = _context.AnalizciProfils.OrderByDescending(x => x.AnalizPuan).ToList();
            return View("Ara", analistler); // Ara view'ını kullanarak listeler
        }
    }

    // ViewModel sınıfları
    public class DashboardViewModel
    {
        public AnalizciProfil? Analist { get; set; }
        public List<IsModeli> GecmisIsler { get; set; } = new List<IsModeli>();
        public List<IsFirsatiModeli> IsFirsatlari { get; set; } = new List<IsFirsatiModeli>();
        public int BekleyenIsSayisi { get; set; }
    }

    public class IsModeli
    {
        public int BasvuruID { get; set; }
        public string? MacBilgisi { get; set; }
        public DateTime Tarih { get; set; }
        public int? Durum { get; set; } // Veritabanındaki tipine göre (byte/int) dikkat et
        public int? Puan { get; set; }
        public string? DosyaYolu { get; set; }
    }

    public class IsFirsatiModeli
    {
        public int RaporID { get; set; }
        public string? TakimAdi { get; set; }
        public string? Lig { get; set; }
        public string? MacBilgisi { get; set; }
        public DateTime SonTarih { get; set; }
    }
}