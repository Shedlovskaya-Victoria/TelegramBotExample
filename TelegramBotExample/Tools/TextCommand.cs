using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBotExample.Tools
{
	public class TextCommand
	{
		public	static bool Title = false;
		public static bool FioDirector = false;
		public static bool Year = false;
		public static async Task<string> TextProcess(ITelegramBotClient client, Update update, bool ReadQrCode = false)
		{
			if (ReadQrCode)
				return null;
			else if (update.Message.Text != null)
			{
				if (update.Message.Text.ToLower().StartsWith("/"))
				{
					if (update.Message.Text.ToLower() == "/" | update.Message.Text.ToLower() == "/list")
					{
						return @"Доступные команды бота: 
					/start - начало работы
					/list - возвращает список команд
					/уля-ля - секрет
					/resource - пост с его кнопками
					справа от поля ввода кнопки бота 
					отправьте фотографию документом и получите улучшеную версию";

					}
					else if (update.Message.Text.ToLower() == "/уля-ля")
					{
						return @"Хочу пожелать тебе большой-большой удачи во всех начинаниях! Пусть она прилагается ко всем твоим стараниям! Настойчиво и упорно иди к целям, не отступай ни на шаг и не сомневайся в себе ни на секунду. Знай, что у тебя все получится, ведь если ты по-настоящему к этому стремишься, то заслуживаешь этого!";

					}
					else if (update.Message.Text.ToLower() == "/resource")
					{
						await FunctionAppPotencialButton(client, update);
						return null;
					}
					else if (update.Message.Text.ToLower() == "/start")
					{
						await BaseKeyBoard(client, update);

						await SendMessage.CardPostWithLink(client, update.Message);

						return null;

					}
					else
						return null;
				}
				else
				{
					return await ProccesFillingWordDocData(client, update);
				}

			}
			else
			{
				return "Команда выводящая список возможностей бота: /list";
			}

			static async Task<string> ProccesFillingWordDocData(ITelegramBotClient client, Update update)
			{
				if (update.Message.Text == "1. Название документа")
				{
					Title = true;

					return null;
				}
				if (Title)
				{
					if (update.Message.Text != "1. Название документа")
					{
						WordFile.Title = update.Message.Text;
						await WordDataButton(client, update, "название принято!");
						Title = false;
					}

					return null;
				}
				else if (update.Message.Text == "2. Фио директора")
				{
					FioDirector = true;

					return null;
				}
				if (FioDirector)
				{
					if (update.Message.Text != "2. Фио директора")
					{
						WordFile.FioDirector = update.Message.Text;
						await WordDataButton(client, update, "фио директора принято!");
						FioDirector = false;
					}

					return null;
				}
				else if (update.Message.Text == "3. Год документа")
				{
					Year = true;

					return null;
				}
				else if (Year)
				{
					if (update.Message.Text != "3. Год документа")
					{
						WordFile.Year = update.Message.Text;
						await WordDataButton(client, update, "год документа принят!");
						Year = false;
					}

					return null;
				}
				if (update.Message.Text == "Завершить")
				{
					await TextCommand.BaseKeyBoard(client, update);
					WordFile.ReplaceText(client, update);

					return "Проверь свой рабочий стол на Word файлик ReplaceAllText.docx";
				}
				if (!string.IsNullOrEmpty(WordFile.Title) &&
					!string.IsNullOrEmpty(WordFile.Year) &&
					!string.IsNullOrEmpty(WordFile.FioDirector))
				{

				await TextCommand.MenuButtons(client, update, new List<KeyboardButton[]>()
				{
					new[]
					{
						new KeyboardButton("1. Название документа"),
						new KeyboardButton("2. Фио директора"),
					},
					new[]
					{
						new KeyboardButton("3. Год документа"),
						new KeyboardButton("Завершить"),
					}
				}, "все данные приняты!");

					return "Обрабатываю!";
				}
				else
					return null;
			}

			static async Task FunctionAppPotencialButton(ITelegramBotClient client, Update update)
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

				await client.SendTextMessageAsync(update.Message.Chat.Id, "Давайте узнаем какими функицями уже овладел бот. " +
					"Нажмите на кнопку и посмотрим не сломает ли это ваш компьютер! " +
					"Ух, любопытство!", replyMarkup: ikm);
			}
		}

		public static async Task BaseKeyBoard(ITelegramBotClient client, Update update)
		{
			await MenuButtons(client, update, new List<KeyboardButton[]>()
			{
				new KeyboardButton[]
				{
					new KeyboardButton("1" ),
					new KeyboardButton("2"),
				},
				new []
				{
					new KeyboardButton("3"),
					new KeyboardButton("4"),
				}
			},
			"Приветсвую, вас! " +
			"Благодарю что воспользовались этим примером. " +
			"Давайте вместе начнем работу и посмотрим возможности телеграм бота." +
			"Обратите внимание на кнопки слева и справа от поля ввода сообщения.");
		}

		public static async Task MenuButtons(ITelegramBotClient client, Update update,
			 List<KeyboardButton[]> listButton, string description)
		{
			var chatId = SendMessage.GetChatId(update);

			var ikm = new ReplyKeyboardMarkup(listButton.ToArray())
			{ ResizeKeyboard = true };

			await client.SendTextMessageAsync(
				chatId,
				description,
				replyMarkup: ikm
				);
		}
		public static async Task WordDataButton(ITelegramBotClient client, Update update, string descroption)
		{
			await TextCommand.MenuButtons(client, update, new List<KeyboardButton[]>()
				{
					new[]
					{
					new KeyboardButton("1. Название документа"),
					new KeyboardButton("2. Фио директора"),
					},
					new[]
					{
					 new KeyboardButton("3. Год документа")
					}
				}, descroption);
		}


	}
}
