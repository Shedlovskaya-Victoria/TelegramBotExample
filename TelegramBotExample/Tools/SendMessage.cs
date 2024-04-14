using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using File = System.IO.File;

namespace TelegramBotExample.Tools
{
	public class SendMessage
	{
		public static async Task SendMessageWithPicture(ITelegramBotClient client, Message message, string pathToImage, string description = "")
		{
			if (!string.IsNullOrEmpty(pathToImage))
			{
				using (Stream stream = System.IO.File.OpenRead(pathToImage))
				{
					await client.SendPhotoAsync(
						message.Chat.Id,
						InputFile.FromStream(stream),
					null,
					description
					);
				}
			}

		}

		public static async Task CardPostWithLink(ITelegramBotClient client, Message message)
		{
			await SendMessage.SendMessageWithPicture(client, message, @$"{Environment.CurrentDirectory}\Resource\Image\закат.jpg",
				 @"Какая красота, а! 
		Жмакни же! 
		https://mangalib.me/bungou-stray-dogs?section=info&ui=627294");
		}

		public static async Task SendFileToUser(ITelegramBotClient client, Update update, string destinationFilePath)
		{
			using (Stream stream = File.OpenRead(destinationFilePath))
			{
				Message message = await client.SendDocumentAsync(
				 update.Message.Chat.Id,
				 InputFile.FromStream(stream, update.Message.Document.FileName.Replace(".jpg", "(edit).jpg")
				 ));
			}

		}

		public static async Task BotAnswer(ITelegramBotClient client, Update update, string answer)
		{
			long id;
			if (update.Message == null)
				id = update.CallbackQuery.From.Id;
			else
				id = update.Message.Chat.Id;

			await client.SendTextMessageAsync(id, $"{answer}");
		}
	}
}
