using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TelegramBotExample.Tools
{
	public class Registration : Email
	{
		public static Image Register(string toEmail, string passwordRegister, string fioToSend)
		{
			Email.Send(toEmail, passwordRegister, fioToSend);

			return QrCode.Create($"{passwordRegister}");
		}
	}
}
