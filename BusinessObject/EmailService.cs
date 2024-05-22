using System;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly string _host = "smtp.gmail.com";
    private readonly int _port = 587;
    private readonly bool _enableSsl = true;
    private readonly string _userName = "ishahzaibazhar@gmail.com";
    private readonly string _password = "xkut tdea baaz vzzu";

    public void SendEmail(string to, string subject, string body)
    {
        using (var client = new SmtpClient(_host, _port))
        {
            client.Credentials = new NetworkCredential(_userName, _password);
            client.EnableSsl = _enableSsl;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_userName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            try
            {
                client.Send(mailMessage);
                // Log success if needed
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                Console.WriteLine("Email sending failed: " + ex.Message);
                throw new Exception("Email sending failed.", ex);
            }
        }
    }
}
