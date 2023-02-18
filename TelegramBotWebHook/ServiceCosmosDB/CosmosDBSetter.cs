using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotWebHook.ServiceCosmosDB
{
    internal static class CosmosDBSetter
    {
        private static readonly CosmosClient _client;
        static CosmosDBSetter()
        {
            _client = new CosmosClient(
                accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
                authKeyOrResourceToken: Environment.GetEnvironmentVariable("OSMOS_KEY")!);
        }

        static async Task<Database> CreateCosmosAsynk()
        {
            Database TelegrameMessageDb = await _client.CreateDatabaseIfNotExistsAsync("telegramMessageDb");
            return TelegrameMessageDb;
        }
    }
}
