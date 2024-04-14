using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotExample.Tools
{
	public class QrCode
	{
		public QrCode()
		{
		}

		public static Image Create(string qrtext = "")
		{
			try
			{
				var encoder = new QRCodeEncoder(); //создаем объект класса QRCodeEncoder
				Bitmap qrcode = encoder.Encode(qrtext); // кодируем слово, полученное из TextBox'a (qrtext) в переменную qrcode. класса Bitmap(класс, который используется для работы с изображениями)
				return qrcode as Image; // pictureBox выводит qrcode как изображение.
			} catch(Exception ex)
			{
				TrackingApp.WriteExeption(ex, "Ошибка создания qr code: ");

				return null;
			}
		}
		public static string Read(Image image)
		{
			try
			{
				var decoder = new QRCodeDecoder(); // создаём "раскодирование изображения"
				return decoder.Decode(new QRCodeBitmapImage(image as Bitmap)); //в MessageBox'e программа запишет раскодированное сообщение с изображения, которое предоврительно будет переведено из pictureBox'a в класс Bitmap, чтобы мы смогли с этим изображением работать. 
			}
			catch (Exception ex)
			{
				TrackingApp.WriteExeption(ex, "Ошибка чтения qr code: ");

				return null;
			}
		}
		public static async Task ScanQrCode(ITelegramBotClient client, Update update)
		{
			//получить путь 
			string? filePath = await DocumentFile.GetFilePath(client, update);
			//скачать файл
			string? destinationFilePath = await DocumentFile.DownloadFromTelegramToApp(client, update, filePath);
			if (!string.IsNullOrEmpty(filePath))
			{
				var qrCodeMessage = Read(Image.FromFile(destinationFilePath));

				if (!string.IsNullOrEmpty(qrCodeMessage))
					SendMessage.BotAnswer(client, update, qrCodeMessage);
				else
					SendMessage.BotAnswer(client, update, "Произошла неявная ошибка. Попробуйте сначала четко следуя всем инструкциям.");

				DocumentFile.CheckToProblemFile(filePath);
				DocumentFile.CheckToProblemFile(destinationFilePath);

			}
			else
			{
				SendMessage.BotAnswer(client, update, "Произошла неявная ошибка. Попробуйте сначала четко следуя всем инструкциям.");
			}

		}
		public static async Task SendQrCode(ITelegramBotClient client, Update update, string Email, string Password, Image qrcode)
		{
			if (qrcode != null)
			{
				string pathToIQrCode = DocumentFile.UploadFile(qrcode);

				await SendMessage.BotAnswer(client, update,
					"Поздравляю! На каком-то несуществующем сайте вы дай бог зарегистрованы, проверьте вашу почту.");

				await SendMessage.SendMessageWithPicture(client, update.Message, pathToIQrCode, "Ваш Qr Code.");

				DocumentFile.CheckToProblemFile(pathToIQrCode);
			}
			else
			{
				await SendMessage.BotAnswer(client, update, "Произошла неявная ошибка. Попробуйте сначала четко следуя всем инструкциям.");
			}
		}
	}
}
