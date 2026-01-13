using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutbolAnalizPazari.Migrations
{
    /// <inheritdoc />
    public partial class RaporTablosuGuncelleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalizciProfil",
                columns: table => new
                {
                    AnalizciId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sifre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DogumTarihi = table.Column<DateOnly>(type: "date", nullable: true),
                    Hakkinda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalistigiKulup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YaptigiAnalizSay = table.Column<int>(type: "int", nullable: true),
                    OnayAnalizSay = table.Column<int>(type: "int", nullable: true),
                    AnalizPuan = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalizciProfil", x => x.AnalizciId);
                });

            migrationBuilder.CreateTable(
                name: "AnalizKategori",
                columns: table => new
                {
                    KatergoriId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriAd = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalizKategori", x => x.KatergoriId);
                });

            migrationBuilder.CreateTable(
                name: "Basvurular",
                columns: table => new
                {
                    BasvuruID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaporID = table.Column<int>(type: "int", nullable: false),
                    AnalizciID = table.Column<int>(type: "int", nullable: false),
                    DosyaYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalistNotu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basvurular", x => x.BasvuruID);
                });

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    MusteriID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusteriTip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YetkiliAd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YetkiliSoyad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YetkiliMail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sifre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SirketAd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seviye = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HesapDurum = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.MusteriID);
                });

            migrationBuilder.CreateTable(
                name: "Raporlar",
                columns: table => new
                {
                    RaporId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusteriId = table.Column<int>(type: "int", nullable: true),
                    KategoriID = table.Column<int>(type: "int", nullable: true),
                    MacBilgisi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SonTeslimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OzelIstekler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Durum = table.Column<byte>(type: "tinyint", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raporlar", x => x.RaporId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalizciProfil");

            migrationBuilder.DropTable(
                name: "AnalizKategori");

            migrationBuilder.DropTable(
                name: "Basvurular");

            migrationBuilder.DropTable(
                name: "Musteriler");

            migrationBuilder.DropTable(
                name: "Raporlar");
        }
    }
}
