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
using Microsoft.Extensions.Configuration;

namespace TelegramBotWebHook
{
    public class TelegramBotWebHook
    {
        private static IConfiguration AppConfig { get; set; }
        private static ICosmosDBSetter CosmosDBSetter { get; set; }
        

        public TelegramBotWebHook(IConfiguration configuration)
        {
            AppConfig = configuration;
            CosmosDBSetter = new CosmosDBSetter(configuration);
        }


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
            await CosmosDBSetter.AddItemsToContainerAsync(data);

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
            var token = AppConfig["AppConfig:Token"];
            if (token is null)
            {
                throw new ArgumentException("Can't get a token");
            }
            var telegramClient = new TelegramBotClient(token);
            return telegramClient;
        }
    }



}
