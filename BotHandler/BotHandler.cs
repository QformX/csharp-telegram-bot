using Telegram.Bot;
using TokenHandler;

namespace BotHandler;

public class BotHandler
{
    public async static void Start()
    {
        string path = "..\\..\\..\\..\\BotHandler\\encrypted.bin";
        var token = new Tokenizer(path).Token();
        var botClient = new TelegramBotClient(token);
        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
    }
}
