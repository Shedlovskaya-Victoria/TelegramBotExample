using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotExample.Tools
{
	public class TrackingApp
	{
		public static void ConsoleControl(Update update, string button = "нет нажатия")
		{
			Console.WriteLine();
			Console.WriteLine($"{update.Message.Chat.FirstName ?? "Имени нет"} " +
				$"{update.Message.Chat.LastName ?? "фамилии нет"}    |   " +
				$"Date: {update.Message.Date}    |   " +
				$"{update.Message.Text ?? $"текста нет. формат сообщения: " + update.Message.Type}");
			Console.WriteLine();

		}
	}
}
