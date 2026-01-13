using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace FutbolAnalizPazari.Models;

public partial class AnalizciProfil
{
    [Key]
    public int AnalizciId { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? Mail { get; set; }

    public string? Sifre { get; set; }

    public DateOnly? DogumTarihi { get; set; }

    public string? Hakkinda { get; set; }

    public string? CalistigiKulup { get; set; }

    public int? YaptigiAnalizSay { get; set; }

    public int? OnayAnalizSay { get; set; }

    public decimal? AnalizPuan { get; set; }
}
