using System;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TokenHandler;
using WeatherHandler;
using System.Drawing.Imaging;
using System.Drawing;

namespace BotHandler;

public class BotHandler
{
    private bool _callback = false;
    public BotHandler() { }
    public void Start()
    {
        string path = "..\\..\\..\\..\\BotHandler\\encrypted.bin";
        var token = new Tokenizer(path).Token();
        var botClient = new TelegramBotClient(token);
        using CancellationTokenSource cts = new();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandleErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await (update.Type switch
        {
            UpdateType.CallbackQuery => HandleCallbackQueryUpdateAsync(botClient, update, cancellationToken),
            UpdateType.Message => HandleMessageBridge(botClient, update, cancellationToken)
        });
    }

    private async Task HandleWeatherMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        InlineKeyboardMarkup inlineKeyboard = new(new[]
{
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Описать одежду", callbackData: "аа"),
            }
        });

        var chatId = message.Chat.Id;
        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        if (message.Text != "/start")
        {
            var cur_city = messageText;
            var handler = new WeatherHandler.WeatherHandler($"{cur_city}");
            try
            {
                var forecast = handler.GetForecast();
                var forecastmessage = forecast.BuildMessage();
                Bitmap _bitmap = new Bitmap(10, 10);

                if (Convert.ToBoolean(forecast.current.is_day)) _bitmap = new Bitmap("C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\forecast_day.png");
                else _bitmap = new Bitmap("C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\forecast_night.png");
                
                Graphics g = Graphics.FromImage(_bitmap);
                int fontsize;
                int height;

                if (forecast.location.name.Length < 10)
                {
                    fontsize = 120;
                    height = 280;
                }
                else
                {
                    fontsize = 70;
                    height = 330;
                }

                g.DrawString($"{forecast.location.name}", new Font("Century Gothic", fontsize), Brushes.White, new PointF(10, height));
                g.DrawString($"{forecast.current.temp_c}", new Font("Myriad Pro", 200), Brushes.White, new PointF(5, 20));

                Directory.CreateDirectory($"C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\temp_pics\\{chatId}");
                _bitmap.Save($"C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\temp_pics\\{chatId}\\image.png", ImageFormat.Png);

                g.Dispose();
                _bitmap.Dispose();

                using (FileStream fl = new FileStream($"C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\temp_pics\\{chatId}\\image.png", FileMode.Open))
                {
                    Message sentMessage = await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: InputFile.FromStream(fl),
                    caption: forecastmessage,
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
                }

                Directory.Delete($"C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\temp_pics\\{chatId}", true);
            }
            catch (Exception e)
            {
                var exceptionHandler = new ExceptionHandler(botClient, chatId, cancellationToken, e);
                Message sentMessage = await exceptionHandler.Handle();
            }
        } else
        {
            Message startMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Привет, помоги мне составить датасет :)\n Напиши мне город, в котором ты живёшь!",
            cancellationToken: cancellationToken);
        }
    }

    async Task HandleCallbackQueryUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.CallbackQuery is not { } query)
            return;

        if (query.Id is not { } queryId)
            return;

        _callback = !_callback;

        await botClient.AnswerCallbackQueryAsync(queryId, text: "callback recieved");
        Console.WriteLine("Callback");
    }

    private async Task HandleMessageBridge(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (!_callback)
        {
            await HandleWeatherMessageAsync(botClient, update, cancellationToken);
        }
        else
        {
            using CancellationTokenSource cts = new(3000);
            await HandleClothesAddAsync(botClient, update, cts.Token);
            _callback = !_callback;
        }
    }

    private async Task HandleClothesAddAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        var cur_city = messageText;
        var handler = new WeatherHandler.WeatherHandler($"{cur_city}");
        try
        {

            Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Ответ получен",
            cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            var exceptionHandler = new ExceptionHandler(botClient, chatId, cancellationToken, e);
            Message sentMessage = await exceptionHandler.Handle();
        }
    }

    async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ApiRequestException apiRequestException)
        {
            Console.WriteLine(exception.Message);
        }
    }
}
