using SQLDatabase;
using Telegram.Bot.Types;

namespace BotApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sql = new SQLDatabase.SQLHandler();
            sql.IniInitialize();
            var bot = new BotHandler.BotHandler();
            bot.Start();
            Console.ReadKey();
        }
    }
}