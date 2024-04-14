using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

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
				Console.WriteLine();
				Console.WriteLine("Ошибка создания qr code: " + ex.Message);
				Console.WriteLine();

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
				Console.WriteLine();
				Console.WriteLine("Ошибка чтения qr code: " + ex.Message);
				Console.WriteLine();

				return null;
			}
		}
	}
}
