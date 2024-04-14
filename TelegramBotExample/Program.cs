// See https://aka.ms/new-console-template for more information
using System.Drawing;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotExample.Tools;
using File = System.IO.File;
bool RegisterEmail = false;
bool RegisterPassword = false;
bool ReadQrCode = false;
string Email = "";
string Password = "";

Console.WriteLine("Hello, World!");
try
{
	/*ну типа воспроизвести текст из аудио
    // Initialize a new instance of the SpeechSynthesizer.
    SpeechSynthesizer synth = new SpeechSynthesizer();

	// Configure the audio output. 
	var stream = await synth.SynthesizeTextToStreamAsync("This example demonstrates a basic use of Speech Synthesizer");
	
	var voices = synth.Voice.Language;
	MediaElement media = new();
	media.SetSource(stream, stream.ContentType);
	media.Play();
	*/

	var bot = new TelegramBotClient("7181923998:AAFlrKT-pYE2VtD9lNn5akJTiH553fCxiRs");
  //  await bot.DeleteWebhookAsync();
	bot.StartReceiving(Update, Exaption);
	
}
catch (Exception ex)
{
	Console.WriteLine();
	Console.WriteLine("Ошибка!");
	Console.WriteLine();
	Console.WriteLine(ex.ToString());
	Console.WriteLine();
}


Task Exaption(ITelegramBotClient client, Exception exception, CancellationToken token)
{
	Console.WriteLine();
	Console.WriteLine("Ошибка!");
	Console.WriteLine();
	Console.WriteLine(exception.ToString());
	Console.WriteLine();

	return null;
}

async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
{
	switch (update.Type)
	{
		case UpdateType.CallbackQuery:

			if(update.CallbackQuery.Data == "audio")
			{
				await ProccesSendAudio(client, update);
			}
			else if(update.CallbackQuery.Data == "document")
			{

			}
			else if(update.CallbackQuery.Data == "register")
			{
                RegisterEmail = true;
				BotAnswer(client, update ,"Отправьте вашу почту.");

				return;
			}
			else if(update.CallbackQuery.Data == "readQrCode")
			{
				ReadQrCode = true;
				BotAnswer(client, update ,"Отправьте Qr Code.");

				return;
			}
			break;
		default:
			break;
	}
	if (update.Message != null)
	{
		ConsoleControl(update);

		var answer = TextProcess(client, update.Message);

		if(!string.IsNullOrEmpty(answer.Result))
			BotAnswer(client, update, answer.Result);

		if (RegisterEmail == true)
		{
			Email = update.Message.Text;

			RegisterEmail = false;
			BotAnswer(client, update, "Отправьте пароль.");

			RegisterPassword = true;

			return;
		}
		if (RegisterPassword == true)
		{
			if(update.Message.Text!= Email)
			{
				Password = update.Message.Text;
				RegisterPassword = false;

				Image qrcode = Registration.Register(Email, Password, $"{update.Message.Chat.FirstName} " +
									$"{update.Message.Chat.LastName}");

				await SendQrCode(client, update, Email, Password, qrcode);

				return;

			}
		}

		if (update.Message.Photo != null)
		{
				await BotAnswer(client, update, "Фото это конечно круто. Я знаю о таком. " +
				"Но лучше отошли файликом. " +
				"Без сжатия.");
			
			return;
		}

		if (update.Message.Document != null)
		{
			if (ReadQrCode)
			{
				await ScanQrCode(client, update);
				return;
			}

			await BotAnswer(client, update, "Ща, погодь, сделаю лучше.");

			await ProcessUpdatePhotoDocument(client, update);

			return;
		}
		if (update.Message.Voice != null)
		{
			await BotAnswer(client, update, "Ща, погодь, распознаю аудио.");

			return;
		}


	}

	async Task ScanQrCode(ITelegramBotClient client, Update update)
	{
		//получить путь 
		string? filePath = await GetFilePath(client, update);
		//скачать файл
		string? destinationFilePath = await DownloadFromTelegramToApp(client, update, filePath);
		if (!string.IsNullOrEmpty(filePath))
		{
			var qrCodeMessage = QrCode.Read(Image.FromFile(destinationFilePath));

			if (!string.IsNullOrEmpty(qrCodeMessage))
				BotAnswer(client, update, qrCodeMessage);
			else
				BotAnswer(client, update, "Произошла неявная ошибка. Попробуйте сначала четко следуя всем инструкциям.");

			CheckToProblemFile(filePath);
			CheckToProblemFile(destinationFilePath);

		}
		else
		{
			BotAnswer(client, update, "Произошла неявная ошибка. Попробуйте сначала четко следуя всем инструкциям.");
		}
		ReadQrCode = false;
	}

	async Task SendQrCode(ITelegramBotClient client, Update update, string Email, string Password, Image qrcode)
	{
		if (qrcode != null)
		{
			string pathToIQrCode = UploadQrCode(qrcode);

			await BotAnswer(client, update,
				"Поздравляю! На каком-то несуществующем сайте вы дай бог зарегистрованы, проверьте вашу почту.");

			await SendMessageWithPicture(client, update.Message, pathToIQrCode, "Ваш Qr Code.");

			CheckToProblemFile(pathToIQrCode);
		}
		else
		{
			BotAnswer(client, update, "Произошла неявная ошибка. Попробуйте сначала четко следуя всем инструкциям.");
		}
	}
}
/*
 // This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
     string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
     string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

    static void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
{
	switch (speechRecognitionResult.Reason)
	{
		case ResultReason.RecognizedSpeech:
			Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
			break;
		case ResultReason.NoMatch:
			Console.WriteLine($"NOMATCH: Speech could not be recognized.");
			break;
		case ResultReason.Canceled:
			var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
			Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

			if (cancellation.Reason == CancellationReason.Error)
			{
				Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
				Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
				Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
			}
			break;
	}
}

async Task ProcessAudioText(ITelegramBotClient client, Update update)
{
	var audio =	update.Message.Audio;

	//var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
	//speechConfig.SpeechRecognitionLanguage = "ru-RU";

	using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
	//using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

	Console.WriteLine("Speak into your microphone.");
	//var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
	//OutputSpeechRecognitionResult(speechRecognitionResult);

	await BotAnswer(client, update.Message, "");
}
*/
async Task<string> TextProcess(ITelegramBotClient client, Message message)
{
	if (ReadQrCode)
		return null;
	else if (message.Text != null)
	{
		if (message.Text.ToLower().StartsWith("/"))
		{
			if (message.Text.ToLower() == "/" | message.Text.ToLower() == "/list")
			{
				return @"Доступные команды бота: 
					/start - начало работы
					/list - возвращает список команд
					/уля-ля - секрет
					/resource - пост с его кнопками
					справа от поля ввода кнопки бота 
					отправьте фотографию документом и получите улучшеную версию";

			}
			else if (message.Text.ToLower() == "/уля-ля")
			{
				return @"Хочу пожелать тебе большой-большой удачи во всех начинаниях! Пусть она прилагается ко всем твоим стараниям! Настойчиво и упорно иди к целям, не отступай ни на шаг и не сомневайся в себе ни на секунду. Знай, что у тебя все получится, ведь если ты по-настоящему к этому стремишься, то заслуживаешь этого!";

			}
			else if (message.Text.ToLower() == "/resource")
			{

				var ikm = new InlineKeyboardMarkup(new[]
				{

					new[]
					{
						InlineKeyboardButton.WithCallbackData("аудио", "audio"),
						InlineKeyboardButton.WithCallbackData("обработка документа", "document"),

					},
					new[]
					{
						InlineKeyboardButton.WithCallbackData("регистрация через почту", "register"),
					},
					new[]
					{
						InlineKeyboardButton.WithCallbackData("сканировать Qr Code", "readQrCode"),
					},
				});

				await client.SendTextMessageAsync(message.Chat.Id, "Давайте узнаем какими функицями уже овладел бот. " +
					"Нажмите на кнопку и посмотрим не сломает ли это ваш компьютер! " +
					"Ух, любопытство!", replyMarkup: ikm);
				return null;
			}
			else if (message.Text.ToLower() == "/start")
			{
				await MenuButtons(client, message);
				await CardPostWithLink(client, message);
				return null;

			}
			else
				return null;
		}
		else 
		{
			if (message.Text.ToLower() == "1")
			{
				return "11";
			}
			else
				return null;
		}
		
	}
	else
	{
		return "Команда выводящая список возможностей бота: /list";
	}
}

Console.ReadLine();

async Task BotAnswer(ITelegramBotClient client,Update update, string answer)
{
	long id;
	if (update.Message == null)
		id = update.CallbackQuery.From.Id;
	else
		id = update.Message.Chat.Id;

	await client.SendTextMessageAsync(id, $"{answer}");
}



 async Task ProcessUpdatePhotoDocument(ITelegramBotClient client, Update update)
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
		await SendFileToUser(client, update, destinationFilePath);

		CheckToProblemFile(filePath);
		CheckToProblemFile(destinationFilePath);
	}
	else
	{
		await BotAnswer(client, update, "Произошла неявная ошибка. Попробуйте сначала четко следуя всем инструкциям.");

	}

}

static async Task<string?> DownloadFromTelegramToApp(ITelegramBotClient client, Update update, string filePath)
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

static void CheckToProblemFile(string destinationFilePath)
{
if (System.IO.File.Exists(destinationFilePath))
	File.Delete(destinationFilePath);
}

static async Task SendFileToUser(ITelegramBotClient client, Update update, string destinationFilePath)
{
using (Stream stream = File.OpenRead(destinationFilePath))
{
	Message message = await client.SendDocumentAsync(
	 update.Message.Chat.Id,
	 InputFile.FromStream(stream, update.Message.Document.FileName.Replace(".jpg", "(edit).jpg")
	 ));
}
}


static async Task<string?> GetFilePath(ITelegramBotClient client, Update update)
{
	try
	{
	   var fileId = update.Message.Document.FileId;
		var fileInfo = await client.GetFileAsync(fileId);
		var filePath = fileInfo.FilePath;
		return filePath;
	} catch(Exception ex)
	{
		Console.WriteLine();
		Console.WriteLine("Ошибка загрузки файла: "+ ex.Message);
		Console.WriteLine();
		return null;
	}
}

static async Task ProccesSendAudio(ITelegramBotClient client, Update update)
{
	try
	{
		string filePath = @$"{Environment.CurrentDirectory}\Resource\Audio\audioplayback.weba";
        Stream stream = System.IO.File.OpenRead(filePath);
         await client.SendAudioAsync(update.Message.Chat.Id, InputFile.FromStream(stream));
    }
	catch(Exception e)
	{
		ConsoleControl(update);
		Console.WriteLine("Ошибка отправки аудио: " + e.Message.ToString());
		Console.WriteLine();
	}
	
	
}

static async Task MenuButtons(ITelegramBotClient client, Message message)
{
	var ikm = new ReplyKeyboardMarkup(new[]
	{
		new[]
		{
			new KeyboardButton("1" ),
			new KeyboardButton("2"),
		},
		new[]
		{
			new KeyboardButton("3"),
			new KeyboardButton("4"),
		}
	});

	await client.SendTextMessageAsync(
		message.Chat.Id, 
		"Приветсвую, вас! " +
		"Благодарю что воспользовались этим примером. " +
		"Давайте вместе начнем работу и посмотрим возможности телеграм бота." +
		"Обратите внимание на кнопки слева и справа от поля ввода сообщения.", 
		replyMarkup: ikm
		);
}

static async Task CardPostWithLink(ITelegramBotClient client, Message message)
{
	await SendMessageWithPicture(client, message, @$"{Environment.CurrentDirectory}\Resource\Image\закат.jpg",
		 @"Какая красота, а! 
		Жмакни же! 
		https://mangalib.me/bungou-stray-dogs?section=info&ui=627294");
}

static async Task SendMessageWithPicture(ITelegramBotClient client, Message message, string pathToImage, string description = "")
{
	if(!string.IsNullOrEmpty(pathToImage))
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

static string UploadQrCode(Image qrcode)
{
	var pathToIQrCode = "QrCode.png";

	if (File.Exists(pathToIQrCode))
		File.Delete(pathToIQrCode);

	qrcode.Save(pathToIQrCode);

	return pathToIQrCode;
}
static void ConsoleControl(Update update, string button = "нет нажатия")
{
	Console.WriteLine();
	Console.WriteLine($"{update.Message.Chat.FirstName ?? "Имени нет"} " +
		$"{update.Message.Chat.LastName ?? "фамилии нет"}    |   " +
		$"Date: {update.Message.Date}    |   " +
		$"{update.Message.Text ?? $"текста нет. формат сообщения: " + update.Message.Type}");
	Console.WriteLine();

}