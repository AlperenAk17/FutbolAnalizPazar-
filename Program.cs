using Microsoft.EntityFrameworkCore;
using FutbolAnalizPazari.Models;

var builder = WebApplication.CreateBuilder(args);
// --- KÝMLÝK DOÐRULAMA (COOKIE) SERVÝSÝ ---
// Sisteme "Biz Cookie (Çerez) ile giriþ iþlemi yapacaðýz" diyoruz.
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Giriþ yapmamýþ biri yasaklý yere girerse buraya at.
        options.LogoutPath = "/Account/CikisYap";
        options.AccessDeniedPath = "/Account/Login"; // Yetkisiz giriþ denemesi
        options.Cookie.Name = "FutbolAnaliz.Auth"; // Çerezin adý
        options.SlidingExpiration = true; // Kullanýcý aktifse süreyi uzat
        options.ExpireTimeSpan = TimeSpan.FromDays(30); // Çerez ömrü
    });
// Veritabaný
builder.Services.AddDbContext<AnalizPlatformuContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servisler
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor(); 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
// ... Diðer Use komutlarý ...
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // Önce bu

app.UseSession(); // Session kullanýyorsan bu burada dursun

// --- BURAYI EKLE ---
app.UseAuthentication(); // <--- BU SATIRI EKLE (Kimlik Doðrulama)
// -------------------

app.UseAuthorization(); // Sonra bu (Yetkilendirme)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();