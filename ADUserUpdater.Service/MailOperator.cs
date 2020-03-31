using System;
using System.Net;
using System.Net.Mail;

namespace ADUserUpdater.Service
{
    public static class MailOperator
    {
        public static void Send(string appEmailAddress, string appEmailPassword, string smtpHost, string targetEmailAddress, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(Properties.Settings.Default.AppEmailAddress),
                    Subject = subject,
                    Body = body,
                };
                mail.To.Add(new MailAddress(targetEmailAddress));

                SmtpClient client = new SmtpClient()
                {
                    Host = smtpHost,
                    EnableSsl = true,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential()
                    {
                        UserName = appEmailAddress,
                        Password = appEmailPassword,
                    },
                    Port = 587,
                };

                client.Send(mail);
                }
            catch (Exception e)
            {
                throw new Exception("Failed to send ADInfo errors: " + e.Message);
            }

        }
    }
}
