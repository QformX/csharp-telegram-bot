using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TokenHandler;
using WeatherHandler;

namespace BotHandler;

public class BotHandler
{
    public BotHandler() { }
    public async void Start()
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
            UpdateType.CallbackQuery => HandleCallbackQueryUpdateAsync(botClient, update.CallbackQuery, cancellationToken),
            UpdateType.Message => HandleWeatherMessageAsync(botClient, update, cancellationToken)
        });
    }

    private static async Task HandleWeatherMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        InlineKeyboardMarkup inlineKeyboard = new(new[]
{
            // first row
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Описать одежду", callbackData: ""),
            }
        });

        var chatId = message.Chat.Id;
        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        var cur_city = messageText;
        var handler = new WeatherHandler.WeatherHandler($"{cur_city}");
        try
        {
            var forecast = handler.GetForecast();
            var forecastmessage = forecast.BuildMessage();

            Message sentMessage = await botClient.SendPhotoAsync(
            chatId: chatId,
            photo: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg"),
            caption: forecastmessage,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            var exceptionHandler = new ExceptionHandler(botClient, chatId, cancellationToken, e);
            Message sentMessage = await exceptionHandler.Handle();
        }
    }

    async Task HandleCallbackQueryUpdateAsync(ITelegramBotClient botClient, CallbackQuery update, CancellationToken cancellationToken)
    {
        Console.WriteLine("Callback");
    }

    async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ApiRequestException apiRequestException)
        {
            Console.WriteLine(exception.Message);
        }
    }
}
