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
			
			if (update.CallbackQuery.Data == "audio")
			{
				TrackingApp.ConsoleControl(update, "audio");
				await SendMessage.ProccesSendAudio(client, update); 
			}
			else if (update.CallbackQuery.Data == "document")
			{
				TrackingApp.ConsoleControl(update, "document");
				await TextCommand.WordDataButton(client, update, "Отправьте эти данные для шаблона!");
				//WordFile.CreateWordFile();

			}
			else if (update.CallbackQuery.Data == "register")
			{
				TrackingApp.ConsoleControl(update, "register");
				RegisterEmail = true;
				SendMessage.BotAnswer(client, update, "Отправьте вашу почту.");

				return;
			}
			else if (update.CallbackQuery.Data == "readQrCode")
			{
				TrackingApp.ConsoleControl(update, "readQrCode");
				ReadQrCode = true;
				SendMessage.BotAnswer(client, update, "Отправьте Qr Code.");

				return;
			}
			break;
		default:
			break;
	}
	if (update.Message != null)
	{
		TrackingApp.ConsoleControl(update);

		var answer = TextCommand.TextProcess(client, update, ReadQrCode);

		if (!string.IsNullOrEmpty(answer.Result))
			SendMessage.BotAnswer(client, update, answer.Result);

		if (RegisterEmail == true)
		{
			Email = update.Message.Text;

			RegisterEmail = false;
			SendMessage.BotAnswer(client, update, "Отправьте пароль.");

			RegisterPassword = true;

			return;
		}
		if (RegisterPassword == true)
		{
			if (update.Message.Text != Email)
			{
				Password = update.Message.Text;
				RegisterPassword = false;

				Image qrcode = Registration.Register(Email, Password, $"{update.Message.Chat.FirstName} " +
									$"{update.Message.Chat.LastName}");

				await QrCode.SendQrCode(client, update, Email, Password, qrcode);
				ReadQrCode = false;

				return;

			}
		}

		if (update.Message.Photo != null)
		{
			await SendMessage.BotAnswer(client, update, "Фото это конечно круто. Я знаю о таком. " +
			"Но лучше отошли файликом. " +
			"Без сжатия.");

			return;
		}

		if (update.Message.Document != null)
		{
			if (ReadQrCode)
			{
				await QrCode.ScanQrCode(client, update);
				return;
			}

			await SendMessage.BotAnswer(client, update, "Ща, погодь, сделаю лучше.");

			await DocumentFile.ProcessUpdatePhotoDocument(client, update);

			return;
		}
		if (update.Message.Voice != null)
		{
			await SendMessage.BotAnswer(client, update, "Ща, погодь, распознаю аудио.");

			return;
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

}

Console.ReadLine();

