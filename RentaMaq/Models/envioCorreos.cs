using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using SendGrid;

namespace RentaMaq.Models
{
    public class envioCorreos
    {
        /*
        static string smtpAddress = "smtp.gmail.com";
        static int portNumber = 587;
        static bool enableSSL = true;
        static string emailFrom = "norum19@gmail.com";
        static string password = "europe2089";
        //string emailTo = "norum19@gmail.com";
        static string subject = "";
        static string body = "";
        static string org;
        //*/
        /*
        public static void enviarAlerta(string emailTo, string subject,string body)
        {
            try
            {                

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    // Can set to false, if you are sending pure text.

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }
            }
            catch
            {
                
            }
        }
        //*/

        public static void enviarAlerta(List<string> emailTo, string subject, string body) 
        {
            // Add multiple addresses to the To field.
            List<String> recipients = emailTo;

            var username = System.Environment.GetEnvironmentVariable("SENDGRID_USERNAME");
            var pswd = System.Environment.GetEnvironmentVariable("SENDGRID_PASSWORD");

            // Create the email object first, then add the properties.
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(recipients);
            myMessage.From = new MailAddress("avisosrentamaq@gmail.com", "Sistema de avisos Renta-Maq");
            myMessage.Subject = subject;
            myMessage.Html= body;

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(username, pswd);

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email, which returns an awaitable task.
            transportWeb.DeliverAsync(myMessage);

            // If developing a Console Application, use the following
            // transportWeb.DeliverAsync(mail).Wait();
        }

        public static void enviarAlerta(string emailTo, string subject, string body)
        {
            // Add multiple addresses to the To field.
            List<String> recipients = new List<string>();
            recipients.Add(emailTo);

            var username = "azure_9139b8d19d8d8919ac87387d2b808886@azure.com";
            var pswd = "39OkI9h8mNIt1Gm";

            // Create the email object first, then add the properties.
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(recipients);
            myMessage.From = new MailAddress("avisosrentamaq@gmail.com", "Sistema de avisos Renta-Maq");
            myMessage.Subject = subject;
            myMessage.Html = body;

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(username, pswd);

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email, which returns an awaitable task.
            transportWeb.DeliverAsync(myMessage);

            // If developing a Console Application, use the following
            // transportWeb.DeliverAsync(mail).Wait();
        }
    }
}