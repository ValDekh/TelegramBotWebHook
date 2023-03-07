using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(TelegramBotWebHook.Startup))]
namespace TelegramBotWebHook
{
    public class Startup : FunctionsStartup
    {
    }
}
