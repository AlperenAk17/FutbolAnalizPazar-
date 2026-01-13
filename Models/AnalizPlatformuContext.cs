using Microsoft.EntityFrameworkCore;

namespace FutbolAnalizPazari.Models
{
    public class AnalizPlatformuContext : DbContext
    {
        public AnalizPlatformuContext(DbContextOptions<AnalizPlatformuContext> options) : base(options) { }

        // Kod tarafında kullanacağımız listeler (Sonunda 's' olabilir, sorun değil)
        public DbSet<AnalizciProfil> AnalizciProfils { get; set; }
        public DbSet<Musteriler> Musterilers { get; set; }
        public DbSet<Raporlar> Raporlars { get; set; }
        public DbSet<Basvurular> Basvurulars { get; set; }
        public DbSet<AnalizKategori> AnalizKategori { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // BURASI HAYATİ ÖNEM TAŞIYOR: Kod ismini SQL ismine bağlıyoruz.

            modelBuilder.Entity<Raporlar>(entity => {
                entity.ToTable("Raporlar"); // SQL'deki gerçek adı (s yok)
                entity.HasKey(e => e.RaporId);
            });

            modelBuilder.Entity<Basvurular>(entity => {
                entity.ToTable("Basvurular"); // SQL'deki gerçek adı
                entity.HasKey(e => e.BasvuruID);
            });

            modelBuilder.Entity<Musteriler>(entity => {
                entity.ToTable("Musteriler");
                entity.HasKey(e => e.MusteriID);
            });

            modelBuilder.Entity<AnalizciProfil>(entity => {
                entity.ToTable("AnalizciProfil");
                entity.HasKey(e => e.AnalizciId);
            });

            modelBuilder.Entity<AnalizKategori>(entity => {
                entity.HasKey(e => e.KatergoriId);
            });
        }
    }
}