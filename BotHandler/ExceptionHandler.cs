using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotHandler
{
    public class ExceptionHandler
    {
        private ITelegramBotClient _botClient;
        private long _chatId;
        private CancellationToken _cancellationToken;
        private Exception _exception;
        public ExceptionHandler(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken, Exception exception) 
        {
            _botClient = botClient;
            _chatId = chatId;
            _cancellationToken = cancellationToken;
            _exception = exception;
        }

        public Task<Message> Handle()
        {
            return _botClient.SendTextMessageAsync(
            chatId: _chatId,
            text: _exception.Message,
            cancellationToken: _cancellationToken);
        }
    }
}
