using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBotExample.Tools
{
	public class AudioFile
	{
		public static async Task ProccesSendAudio(ITelegramBotClient client, Update update)
		{
			try
			{
				string filePath = @$"{Environment.CurrentDirectory}\Resource\Audio\audioplayback.weba";
				Stream stream = System.IO.File.OpenRead(filePath);
				await client.SendAudioAsync(update.Message.Chat.Id, InputFile.FromStream(stream));
			}
			catch (Exception e)
			{
				TrackingApp.ConsoleControl(update);
				Console.WriteLine("Ошибка отправки аудио: " + e.Message.ToString());
				Console.WriteLine();
			}

		}
	}
}
