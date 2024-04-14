using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using File = System.IO.File;
using System.Drawing;

namespace TelegramBotExample.Tools
{
	public class DocumentFile
	{
		

		public static async Task<string?> GetFilePath(ITelegramBotClient client, Update update)
		{
			try
			{
				var fileId = update.Message.Document.FileId;
				var fileInfo = await client.GetFileAsync(fileId);
				var filePath = fileInfo.FilePath;
				return filePath;
			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("Ошибка загрузки файла: " + ex.Message);
				Console.WriteLine();
				return null;
			}
		}

		public static async Task<string?> DownloadFromTelegramToApp(ITelegramBotClient client, Update update, string filePath)
		{
			//создать путь к новому файлу	
			string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\{update.Message.Document.FileName}";

			//проверить на сущестование его же прежде и удалить для загрузки заново
			CheckToProblemFile(destinationFilePath);

			//загрузить файл
			await using (Stream fileStream = System.IO.File.OpenWrite(destinationFilePath))
				await client.DownloadFileAsync(filePath, fileStream);

			return destinationFilePath;
		}

		public static void CheckToProblemFile(string destinationFilePath)
		{
			if (System.IO.File.Exists(destinationFilePath))
				File.Delete(destinationFilePath);
		}

		public static async Task ProcessUpdatePhotoDocument(ITelegramBotClient client, Update update)
		{
			//получить путь
			string? filePath = await GetFilePath(client, update);
			if (!string.IsNullOrEmpty(filePath))
			{
				//скачать файл
				string? destinationFilePath = await DownloadFromTelegramToApp(client, update, filePath);

				//типа улучшие фотку...
				//TODO: проблема неподдерживаемых классов WinForms
				/*
				//отправить на сайт для обработки
				Message message = await client.SendPhotoAsync(update.Message.Chat.Id,
					InputFile.FromUri($"https://snapedit.app/ru/enhance/upload/{update.Message.Document.FileName}"));
				//открыть браузер
				System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
				Thread.Sleep(5000);  // пауза 5 секунд

				//кликаем по кнопке скачать
				HTMLCollection elmCol;
				var webBrowser1 = new WebBrowser();
				elmCol = webBrowser1.Document.GetElementsByTagName("button");
				foreach (HTMLBodyElement elmBtn in elmCol)
				{
					if (elmBtn.GetAttribute("className") == "inline-flex items-center justify-center w-full py-3 px-4 bg-blue-500 rounded-lg text-base transition text-white hover:bg-opacity-80")
					{
						elmBtn.InvokeMember("Click");
					}
				}
				*/

				//отправить в чат
				await SendMessage.SendFileToUser(client, update, destinationFilePath);

				CheckToProblemFile(filePath);
				CheckToProblemFile(destinationFilePath);
			}
			else
			{
				await SendMessage.BotAnswer(client, update, "Произошла неявная ошибка. Попробуйте сначала четко следуя всем инструкциям.");

			}
		}
		public static string UploadFile(Image qrcode)
		{
			var pathToIQrCode = "QrCode.png";

			if (File.Exists(pathToIQrCode))
				File.Delete(pathToIQrCode);

			qrcode.Save(pathToIQrCode);

			return pathToIQrCode;
		}
	}
	

}
