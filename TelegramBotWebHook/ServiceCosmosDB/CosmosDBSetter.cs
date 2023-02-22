using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Container = Microsoft.Azure.Cosmos.Container;

namespace TelegramBotWebHook.ServiceCosmosDB
{
    internal static class CosmosDBSetter
    {
        private static readonly CosmosClient _client;
        private static Database _database;
        private static Container _container;
        // private static
        static CosmosDBSetter()
        {
            _client = new CosmosClient(
                "https://localhost:8081",
                 "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
        }

        private static async Task CreateDbAsync()
        {
            Database TelegrameMessageDb = await _client.CreateDatabaseIfNotExistsAsync("telegramMessageDb");
            _database = TelegrameMessageDb;

        }

        private static async Task CreateContainerAsync()
        {
            _container = await _database.CreateContainerIfNotExistsAsync("receivedMessages", "/Update_id");
        }

        public static async Task Creator()
        {
            await CreateDbAsync();
            await CreateContainerAsync();
        }


        public static async Task AddItemsToContainerAsync(Update update, int partitionKey)
        {
            MessageInfo item = new MessageInfo
            {
                Id = $"{update.Message.MessageId}",
                Update_id = update.Id,
                Message_text = update.Message.Text,
                UserId = update.Message.From.Id,
                Username = update.Message.From.Username,
            };

            // <create_item> 
            MessageInfo createdItem = await _container.CreateItemAsync<MessageInfo>(
                item: item,
                partitionKey: new PartitionKey($"{partitionKey}")
            );


        }
    }
}
