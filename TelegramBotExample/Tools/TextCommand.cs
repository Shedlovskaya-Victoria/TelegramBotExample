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
		public static async Task<string> TextProcess(ITelegramBotClient client, Message message, bool ReadQrCode = false)
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
						await SendMessage.CardPostWithLink(client, message);
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



	}
}
