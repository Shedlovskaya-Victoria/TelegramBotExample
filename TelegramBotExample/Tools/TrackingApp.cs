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
			if(update.Message == null)
			{
				Console.WriteLine();
				Console.WriteLine($"{update.CallbackQuery.Message.Chat.FirstName ?? "Имени нет"} " +
					$"{update.CallbackQuery.Message.Chat.LastName ?? "фамилии нет"}    |   " +
					$"Date: {update.CallbackQuery.Message.Date}    |   " +
					$"button: {button}");
				Console.WriteLine();
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine($"{update.Message.Chat.FirstName ?? "Имени нет"} " +
					$"{update.Message.Chat.LastName ?? "фамилии нет"}    |   " +
					$"Date: {update.Message.Date}    |   " +
					$"{update.Message.Text ?? $"текста нет. формат сообщения: " + update.Message.Type}    |   " +
					$"button: {button}");
				Console.WriteLine();
			}
			

		}

		public static void WriteExeption(Exception ex, string description)
		{
			Console.WriteLine();
			Console.WriteLine(description + ex.Message);
			Console.WriteLine();
		}
	}
}
