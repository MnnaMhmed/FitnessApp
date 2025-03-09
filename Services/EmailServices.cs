
    using System.Net;
    using System.Net.Mail;
namespace fitapp.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _port = 587;
        private readonly string _email = "brabfblfll@gmail.com";
        private readonly string _password = "fwejkfncw3";

        public void SendConfirmationEmail(string recipientEmail, string confirmationLink)
        {
            var smtpClient = new SmtpClient("stmp.gmail.com",587)
            {
                Port = 587,
                Credentials = new NetworkCredential("brabfblfll@gmail.com", "tzke aeqt rovq rkkm"),
                EnableSsl = true,
            };
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587; // استخدم 465 إذا كنت تفعل SSL
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_email),
                Subject = "Confirm your email",
                Body = $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(recipientEmail);
            smtpClient.Send(mailMessage);
        }
    }


}