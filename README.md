# ⚽ Futbol Analiz Pazarı - Mobil Web Projesi

Futbol kulüpleri ile profesyonel veri analistlerini bir araya getiren; kulüplerin analiz talebi oluşturabildiği, analistlerin ise iş fırsatlarını inceleyip başvurabildiği kapsamlı bir kariyer ve hizmet platformudur.

Proje, **Mobil Öncelikli (Mobile First)** yaklaşımıyla tasarlanmış olup, mobil tarayıcılarda **Native Uygulama (PWA)** deneyimi sunmaktadır.

---

## 🛠 Kullanılan Teknolojiler

Projede platform bağımsız (Cross-Platform) web teknolojileri tercih edilmiştir:

* **Yazılım Dili:** C#
* **Framework:** ASP.NET Core 7.0 MVC
* **Veritabanı:** Microsoft SQL Server (Entity Framework Core - Code First)
* **Ön Yüz (Frontend):** HTML5, CSS3 (Custom Glassmorphism), Bootstrap 5
* **Mobil Teknoloji:** Progressive Web App (PWA) Mimarisinde Responsive Design

---

## 📱 Test Edilen Platformlar

Uygulama, "Platform Bağımsız" yapısı sayesinde aşağıdaki ortamlarda test edilmiş ve başarıyla çalışmıştır:

1.  **Fiziksel Cihazlar:**
    * **Android:** Samsung Galaxy S Serisi (Google Chrome & Samsung Internet Tarayıcıları)
    * **iOS:** iPhone 13 (Safari Tarayıcı)
2.  **Emülatör ve Simülatörler:**
    * Chrome DevTools Mobile Emulation (iPhone 12 Pro, Pixel 5 görünümleri)
    * Responsive Design Mode

---

## ✨ Proje Özellikleri

* **Rol Bazlı Sistem:** Takım Yöneticisi ve Analist için ayrı arayüzler ve yetkiler.
* **Mobil Arayüz:** Sabit alt menü (Dock) ve tam ekran uygulama modu.
* **İş Akışı:** İlan oluşturma, detay görüntüleme ve başvuru yapma süreçleri.
* **Yönetim Paneli (Dashboard):** Kullanıcıya özel istatistikler ve veri kartları.

---

## 🚀 Kurulum ve Çalıştırma

Projeyi yerel ortamda çalıştırmak için:

1.  Repoyu klonlayın veya ZIP olarak indirin.
2.  `appsettings.json` dosyasındaki Connection String'i kendi SQL sunucunuza göre güncelleyin.
3.  Package Manager Console üzerinden veritabanını oluşturun:
    ```powershell
    Update-Database
    ```
4.  Projeyi çalıştırın (Tarayıcıda mobil görünüm için F12 tuşuna basıp mobil modu açabilirsiniz).

---
*Bu proje, Araştırma Yöntem ve Teknikleri Dersi final ödevi kapsamında hazırlanmıştır.*
