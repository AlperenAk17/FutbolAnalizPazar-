using System.Net;
using System.Net.Mail;

namespace FutbolAnalizPazari.Models
{
    public class MailServisi
    {
        public static void KodGonder(string aliciMail, string kod)
        {
            // BURAYA KENDİ MAİLİNİ VE ALDIĞIN 16 HANELİ UYGULAMA ŞİFRESİNİ YAZ
            string gonderenMail = "seninmailin@gmail.com";
            string gonderenSifre = "buraya_16_haneli_uygulama_sifresi";

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(gonderenMail, gonderenSifre);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(gonderenMail, "Futbol Analiz Pazarı");
            mailMessage.To.Add(aliciMail);
            mailMessage.Subject = "Şifre Sıfırlama Doğrulama Kodu";
            mailMessage.Body = $"Merhabalar,\n\nŞifrenizi sıfırlamak için gereken doğrulama kodunuz: {kod}\n\nBu kodu kimseyle paylaşmayın.";

            client.Send(mailMessage);
        }
    }
}