using System;
using System.ComponentModel.DataAnnotations;

namespace FutbolAnalizPazari.Models
{
    public class Basvurular
    {
        [Key]
        public int BasvuruID { get; set; }

        public int RaporID { get; set; }
        public int AnalizciID { get; set; }

        public string? DosyaYolu { get; set; }
        public string? AnalistNotu { get; set; }
        public DateTime Tarih { get; set; }
        public byte? Durum { get; set; }

        public byte? Puan { get; set; }


    }
}