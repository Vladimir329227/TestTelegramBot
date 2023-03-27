using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace TestTelegramBot
{
    internal class Program
    {

        static ITelegramBotClient bot = new TelegramBotClient("6035041242:AAEBzNjMlg-tome33-XFowNTrv8lCPVD08Y");

        public TimeSpan remaining;

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            DateTime otherDate = new DateTime(2023, 5, 30, 10, 0, 0);
            TimeSpan remaining = otherDate - DateTime.Now;

            JObject contect = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            Console.WriteLine(contect);
            string otvet = (contect["message"]["from"]["first_name"] + " " + contect["message"]["from"]["last_name"] + "\nдо ЕГЭ по русскому осталось \n" + remaining.Days + " дней\n" + "или\n" +
                              (remaining.Hours + remaining.Days * 24) + " часов\n" + "или\n" +
                              (remaining.Minutes + (remaining.Hours + remaining.Days * 24) * 60) + " минут\n" + "или\n" +
                              (remaining.Seconds + (remaining.Minutes + (remaining.Hours + remaining.Days * 24) * 60) * 60) + " секунд");
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text?.ToLower() == "/time")
                {
                    await botClient.SendTextMessageAsync(message.Chat, otvet);
                    return;
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine("Возникла ошибка: " + Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}
