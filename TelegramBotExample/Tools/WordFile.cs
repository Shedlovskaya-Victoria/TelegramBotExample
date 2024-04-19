using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Doc;
using Spire.Doc.Documents;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramBotExample.Tools
{
	public class WordFile
	{
		public static string Title { get; set; } = "";
		public static string FioDirector { get; set; } = "";
		public static string Year { get; set; } = "";
		public static void CreateWordFile()
		{
			//Create a Document instance
			Spire.Doc.Document doc = new();
			//Add a section
			Section section = doc.AddSection();
			//Add a paragraph
			Paragraph para = section.AddParagraph();
			//Append text to the paragraph
			para.AppendText("Hello World!");
			//Save the result document
			doc.SaveToFile(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Output.docx", FileFormat.Docx2019);
		}
		public static void ReplaceText(ITelegramBotClient client, Update update)
		{
			if(!string.IsNullOrEmpty(Title)&&
				!string.IsNullOrEmpty(FioDirector)&&
				!string.IsNullOrEmpty(Year)
				)
			{
                //Создание экземпляра объекта класса Document
                Spire.Doc.Document document = new();

				//Загрузите образец документа Word 
				
				document.LoadFromFile(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\УП 03 01.docx");

				//Change the text replacement mode to replace the first instance only
				document.ReplaceFirst = true;

				//Замените все экземпляры определенного текста новым текстом 
				document.Replace("{{Title}}", Title, false, true);
				document.Replace("{{FioDirector}}", FioDirector, false, true);
				document.Replace("{{YEAR}}", Year, false, true);

				//Сохранить результирующий документ 
				document.SaveToFile($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\ReplaceAllText.docx", 
					FileFormat.Docx2019);
				document.Close();
			}
			else
			{
				SendMessage.BotAnswer(client, update, "Не все поля были заполнены! " +
					"Что-то пошло не так. " +
					"Начните с начала следуя всем инструкциям");
			}
			
		}
	}
}
