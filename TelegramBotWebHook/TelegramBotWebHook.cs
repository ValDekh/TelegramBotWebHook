using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBotWebHook.ServiceCosmosDB;

namespace TelegramBotWebHook
{
    public static class TelegramBotWebHook
    {
        [FunctionName("TelegramBotWebHook")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Invoke telegrame Update fuction");
           await CosmosDBSetter.Creator();

            // string name = req.Query["name"];
            //var token = "6058383219:AAH8O4pcNxHzQ6jG9HntuYJ_U3kU58WE5IE";
            TelegramBotClient telegramClient = GetTelegramBotClient();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Update>(requestBody);
            await CosmosDBSetter.AddItemsToContainerAsync(data, data.Id);

            // name = name ?? data?.name;
            if (data.Type == UpdateType.Message)
            {
                await telegramClient.SendTextMessageAsync(
                     chatId: data.Message.Chat,
                     text: $"Answer : {data.Message.Text}");
            }

            return new OkResult();
        }

        private static TelegramBotClient GetTelegramBotClient()
        {
            var token = Environment.GetEnvironmentVariable("token", EnvironmentVariableTarget.Process);
            if (token is null)
            {
                throw new ArgumentException("Can't get a token");
            }
            var telegramClient = new TelegramBotClient(token);
            return telegramClient;
        }
    }


    public class Rootobject
    {
        public int update_id { get; set; }
        public Message message { get; set; }
    }

    public class Message
    {
        public int message_id { get; set; }
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
    }

    public class From
    {
        public int id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public string language_code { get; set; }
    }

    public class Chat
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public string type { get; set; }
    }

}
