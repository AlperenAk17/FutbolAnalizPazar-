using Microsoft.AspNetCore.Mvc;
using FutbolAnalizPazari.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FutbolAnalizPazari.Controllers
{
    public class AccountController : Controller
    {
        private readonly AnalizPlatformuContext _context;

        public AccountController(AnalizPlatformuContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. SEÇİM EKRANI
        // ==========================================
        [HttpGet]
        public IActionResult RegisterSecim()
        {
            return View();
        }

        // ==========================================
        // 2. ANALİST KAYIT
        // ==========================================
        [HttpGet]
        public IActionResult AnalizciKayit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AnalizciKayit(AnalizciProfil p)
        {
            p.AnalizPuan = 0;
            p.YaptigiAnalizSay = 0;
            p.OnayAnalizSay = 0;

            _context.AnalizciProfils.Add(p);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // ==========================================
        // 3. TAKIM (MÜŞTERİ) KAYIT
        // ==========================================
        [HttpGet]
        public IActionResult TakimKayit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TakimKayit(Musteriler p)
        {
            p.HesapDurum = "Aktif";

            _context.Musterilers.Add(p);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // ==========================================
        // 4. GİRİŞ İŞLEMLERİ (GÜNCELLENDİ: BENİ HATIRLA)
        // ==========================================
        [HttpGet]
        public IActionResult Login()
        {
            // Eğer zaten giriş yapılmışsa, tekrar giriş sayfasına sokma
            if (User.Identity.IsAuthenticated)
            {
                if (HttpContext.Session.GetString("Rol") == "Takim") return RedirectToAction("Taleplerim", "Talep");
                if (HttpContext.Session.GetString("Rol") == "Analist") return RedirectToAction("Dashboard", "Profil");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string mail, string sifre, bool beniHatirla)
        {
            // A) TAKIM KONTROLÜ
            var musteri = _context.Musterilers.FirstOrDefault(x => x.YetkiliMail == mail && x.Sifre == sifre);

            if (musteri != null)
            {
                // Cookie (Çerez) için Kimlik Oluşturma
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, musteri.YetkiliAd + " " + musteri.YetkiliSoyad),
                    new Claim(ClaimTypes.Role, "Takim"),
                    new Claim("KullaniciID", musteri.MusteriID.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = beniHatirla, // Kutucuk işaretliyse kalıcı olsun
                    ExpiresUtc = beniHatirla ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1)
                };

                // Sisteme Giriş Yap (Cookie Oluştur)
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Session'ı da doldur (Eski kodların çalışması için)
                HttpContext.Session.SetInt32("KullaniciID", musteri.MusteriID);
                HttpContext.Session.SetString("AdSoyad", musteri.YetkiliAd + " " + musteri.YetkiliSoyad);
                HttpContext.Session.SetString("Rol", "Takim");

                return RedirectToAction("Taleplerim", "Talep");
            }

            // B) ANALİST KONTROLÜ
            var analist = _context.AnalizciProfils.FirstOrDefault(x => x.Mail == mail && x.Sifre == sifre);

            if (analist != null)
            {
                // Cookie (Çerez) için Kimlik Oluşturma
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, analist.Ad + " " + analist.Soyad),
                    new Claim(ClaimTypes.Role, "Analist"),
                    new Claim("KullaniciID", analist.AnalizciId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = beniHatirla,
                    ExpiresUtc = beniHatirla ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                HttpContext.Session.SetInt32("KullaniciID", analist.AnalizciId);
                HttpContext.Session.SetString("AdSoyad", analist.Ad + " " + analist.Soyad);
                HttpContext.Session.SetString("Rol", "Analist");

                return RedirectToAction("Dashboard", "Profil");
            }

            // C) BAŞARISIZ GİRİŞ
            TempData["Hata"] = "Mail adresi veya şifre hatalı!";
            return RedirectToAction("Login");
        }

        // ==========================================
        // 5. ÇIKIŞ YAP (GÜVENLİ)
        // ==========================================
        public async Task<IActionResult> CikisYap()
        {
            // 1. Çerezleri temizle (Beni Hatırla iptal olur)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 2. Session verilerini sil
            HttpContext.Session.Clear();

            // 3. Tarayıcı önbelleğini temizle (Geri tuşunu engellemek için)
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // 6. ŞİFREMİ UNUTTUM
        // ==========================================
        [HttpGet]
        public IActionResult SifremiUnuttum()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SifremiUnuttum(string mail)
        {
            var takim = _context.Musterilers.FirstOrDefault(x => x.YetkiliMail == mail);
            var analist = _context.AnalizciProfils.FirstOrDefault(x => x.Mail == mail);

            if (takim == null && analist == null)
            {
                TempData["Hata"] = "Bu mail adresi sistemde kayıtlı değil!";
                return View();
            }

            Random rnd = new Random();
            string uretilenKod = rnd.Next(100000, 999999).ToString();

            HttpContext.Session.SetString("DogrulamaKodu", uretilenKod);
            HttpContext.Session.SetString("SifirlanacakMail", mail);

            if (takim != null) HttpContext.Session.SetString("SifirlanacakRol", "Takim");
            else HttpContext.Session.SetString("SifirlanacakRol", "Analist");

            try
            {
                MailServisi.KodGonder(mail, uretilenKod);
            }
            catch (Exception ex)
            {
                // Hata değişkenini (ex) kullanarak uyarıyı kaldırdık
                System.Diagnostics.Debug.WriteLine("Mail Gönderme Hatası: " + ex.Message);
                TempData["Hata"] = "Mail gönderilemedi. Lütfen internet bağlantınızı veya mail ayarlarını kontrol edin.";
                return View();
            }

            return RedirectToAction("KoduDogrula");
        }

        // --- KOD DOĞRULAMA ---
        [HttpGet]
        public IActionResult KoduDogrula()
        {
            if (HttpContext.Session.GetString("DogrulamaKodu") == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public IActionResult KoduDogrula(string girilenKod)
        {
            string? gercekKod = HttpContext.Session.GetString("DogrulamaKodu");

            if (girilenKod == gercekKod)
            {
                HttpContext.Session.Remove("DogrulamaKodu");
                HttpContext.Session.SetString("KodOnaylandi", "Evet");
                return RedirectToAction("SifreYenile");
            }

            TempData["Hata"] = "Girdiğiniz kod hatalı! Lütfen tekrar deneyin.";
            return View();
        }

        // --- ŞİFRE YENİLEME ---
        [HttpGet]
        public IActionResult SifreYenile()
        {
            if (HttpContext.Session.GetString("KodOnaylandi") == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public IActionResult SifreYenile(string yeniSifre)
        {
            string? mail = HttpContext.Session.GetString("SifirlanacakMail");
            string? rol = HttpContext.Session.GetString("SifirlanacakRol");

            if (rol == "Takim")
            {
                var kullanici = _context.Musterilers.FirstOrDefault(x => x.YetkiliMail == mail);
                if (kullanici != null)
                {
                    kullanici.Sifre = yeniSifre;
                    _context.SaveChanges();
                }
            }
            else if (rol == "Analist")
            {
                var kullanici = _context.AnalizciProfils.FirstOrDefault(x => x.Mail == mail);
                if (kullanici != null)
                {
                    kullanici.Sifre = yeniSifre;
                    _context.SaveChanges();
                }
            }

            HttpContext.Session.Clear();
            TempData["Basari"] = "Şifreniz başarıyla değiştirildi. Yeni şifrenizle giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }
    }
}