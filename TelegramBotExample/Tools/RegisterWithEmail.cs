using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TelegramBotExample.Tools
{
	public class RegisterWithEmail
	{
		public static void Register(string toEmail, string passwordRegister, string fioToSend)
		{
			using (var emailService = new SmtpClient())
			{
				try
				{
					//465 - обычное защищенное соединение
					//587 - антиспам для почт(чтобы не закинуло в спам сообщения) защищенное соединение
					//"smtp.beget.com"
					emailService.Host = "smtp.beget.com";
					emailService.Port = 25;
					emailService.EnableSsl = false;
					emailService.DeliveryMethod = SmtpDeliveryMethod.Network;
					emailService.Credentials = new NetworkCredential("viktoria1135@suz-ppk.ru", "%KxfBDY7b*z6g%EIaWxJn1X&");

					MailAddress from = new("viktoria1135@suz-ppk.ru", "От Example Bot Helper");
					MailAddress to = new(toEmail, $"Для {fioToSend}");
					MailMessage mailMessage = new(from, to);

					MailAddress repluTo = new(toEmail);

					mailMessage.Subject = "Регистрация от Example Bot Helper";
					mailMessage.SubjectEncoding = Encoding.UTF8;
					mailMessage.Body = $"<div class=\"container h-100\">\r\n  <div class=\"row h-100 justify-content-center align-items-center\">\r\n    <form class=\"col-12\">\r\n      <div class=\"form-group\">\r\n        <label >Это письмо от телеграм бота Example</label>  </br>\r\n        <label >Регистрация прошла успешно!</label>\r\n\t\t\t\t</br></br>\r\n        <label >Ваши учетные данные: </label>\r\n\t\t\t\t</br></br>\r\n        <label >Пароль: {passwordRegister} </label>\r\n      </div>\r\n    </form>   \r\n  </div>\r\n</div>";
					mailMessage.IsBodyHtml = true;

					 emailService.Send(mailMessage);
					/*типа способы отправки
					 * emailService.Send(mailMessage);
					//emailService.Send("dvikashe@gmail.com", "dvikashe@gmail.com", "test", "body");
					emailService.Dispose();
					*/
					Console.WriteLine($"send email to: {toEmail}");
				}
				catch (SmtpException ex)
				{
					Console.WriteLine("email: " + ex.ToString());
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}

			}
		}
	}
}
