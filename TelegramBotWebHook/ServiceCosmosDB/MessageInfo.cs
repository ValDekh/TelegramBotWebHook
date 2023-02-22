using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotWebHook.ServiceCosmosDB
{
    internal class MessageInfo
    {
        public string Id { get; set; }
        public string Update_id { get; set; }
        public int Message_id { get; set; }
        public string Message_text { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }


    }

}
