using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutbolAnalizPazari.Migrations
{
    /// <inheritdoc />
    public partial class PuanKolonuEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Raporlar");

            migrationBuilder.DropColumn(
                name: "OlusturmaTarihi",
                table: "Raporlar");

            migrationBuilder.DropColumn(
                name: "OzelIstekler",
                table: "Raporlar");

            migrationBuilder.AddColumn<byte>(
                name: "Puan",
                table: "Basvurular",
                type: "tinyint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Puan",
                table: "Basvurular");

            migrationBuilder.AddColumn<byte>(
                name: "Durum",
                table: "Raporlar",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OlusturmaTarihi",
                table: "Raporlar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OzelIstekler",
                table: "Raporlar",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
