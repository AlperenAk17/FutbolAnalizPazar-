using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // [Key] için bu kütüphane şart!

namespace FutbolAnalizPazari.Models
{
    public partial class Musteriler
    {
        [Key] // İşte hatayı çözen sihirli kelime bu
        public int MusteriID { get; set; }

        public string? MusteriTip { get; set; }
        public string? YetkiliAd { get; set; }
        public string? YetkiliSoyad { get; set; }
        public string? YetkiliMail { get; set; }
        public string? Sifre { get; set; }
        public string? SirketAd { get; set; }
        public string? Seviye { get; set; }
        public string? HesapDurum { get; set; }
        
    }
}