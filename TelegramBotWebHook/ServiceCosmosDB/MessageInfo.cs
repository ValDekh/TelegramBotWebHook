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
        public int Id { get; set; }
        public int Update_id { get; set; }
        public string Message_text { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }


    }

}
