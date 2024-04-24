﻿using System;
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

        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { "Help me", "Call me ☎️" },
        })
        {
            ResizeKeyboard = true
        };

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
            await HandleClothesAddAsync(botClient, update, cancellationToken);
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
