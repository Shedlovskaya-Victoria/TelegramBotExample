using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Aspose.Email.Clients;
using Telegram.Bot.Types;
using Windows.Media.Protection.PlayReady;

namespace TelegramBotExample.Tools
{
    public class RegisterWithEmail
    {
        public static void Register(string to, string from = "dvikashe@gmail.com") 
        {
			//Set Aspose license before sending email through Gmail SMTP
			//using Aspose.Email for .NET
			Aspose.Email.License AsposeEmailLicense = new Aspose.Email.License();
			AsposeEmailLicense.SetLicense(@"c:\asposelicense\license.lic");

			//create an instance of MailMessage
			MailMessage EmailMessage = new MailMessage(from, to);
			//Set email message properties which you want to specify
			EmailMessage.Subject = "How to Send Mail Using SMTP Server in C#";
			EmailMessage.Body = "This is a test of sending email using SMTP in C#.";

			//Initiate an instance of SmptpClient class
			SmtpClient SMTPEmailClient = new SmtpClient();
			SMTPEmailClient.UseDefaultCredentials = true;
			//Set SMTP client properties so the email message can get through the server
			SMTPEmailClient.Host = "smtp.gmail.com";
			//SMTPEmailClient.Username = "YourEmail@gmail.com";
			//SMTPEmailClient.Password = "Your Gamil Password";
			SMTPEmailClient.Port = 57787;
			//SMTPEmailClient.SecurityOptions = SecurityOptions.SSLExplicit;
			//Finally send the email message using Gmail's SMTP client

			try
			{
				SMTPEmailClient.Send(EmailMessage);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception caught in CreateTestMessageSendEmail(): {ex.Message}",
					ex.ToString());
			}
		}
	}
}
