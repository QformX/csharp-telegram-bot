using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.VisualBasic;

namespace BotHandler
{
    internal class ClothesAddHandler
    {
        private ITelegramBotClient _botClient;
        private CancellationToken _cancellationToken;
        private Update _update;
        public ClothesAddHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _botClient = botClient;
            _cancellationToken = cancellationToken;
            _update = update;
        }

        public async Task Handle()
        {
            if (_update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"Принял: {messageText}",
            cancellationToken: _cancellationToken);
        }
    }
}
