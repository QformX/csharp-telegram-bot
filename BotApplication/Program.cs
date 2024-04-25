namespace BotApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bot = new BotHandler.BotHandler();
            bot.Start();
            Console.ReadKey();
        }
    }
}