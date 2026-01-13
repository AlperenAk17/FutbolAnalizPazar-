using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbolAnalizPazari.Models
{
    // Class ismi projenin aradığı şekilde 'Raporlar' olarak ayarlandı
    public partial class Raporlar
    {
        [Key]
        // HATA ÇÖZÜMÜ: Proje 'RaporId' arıyor olabilir. 
        // Genelde isimlendirme RaporId şeklindedir. Bunu düzeltiyoruz.
        public int RaporId { get; set; }

        public int? MusteriId { get; set; }
        public int? KategoriID { get; set; } // Veritabanında ID ise böyle kalsın

        public string? MacBilgisi { get; set; }
        public DateTime SonTeslimTarihi { get; set; }
       
    
    }
}