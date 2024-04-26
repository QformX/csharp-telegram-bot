using Microsoft.VisualBasic;
using System.Drawing;
using System.Drawing.Imaging;
using Telegram.Bot;
using WeatherHandler;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using BotHandler;

namespace PhotoHandler
{
    public class PhotoHandler
    {
        private static Bitmap _bitmap = new Bitmap("C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\PhotoHandler\\forecast_day.png");
        private static int _fontsize;
        private static int _height;

        public static async Task SendPhotoAsync(ITelegramBotClient botClient, Weather forecast, string message, long chatId, InlineKeyboardMarkup inlineKeyboard, CancellationToken cancellationToken)
        {
            try
            {
                Graphics g = Graphics.FromImage(_bitmap);

                if (forecast.location.name.Length < 10)
                {
                    _fontsize = 120;
                    _height = 300;
                }
                else
                {
                    _fontsize = 70;
                    _height = 330;
                }

                g.DrawString($"{forecast.location.name}", new Font("Century Gothic", 70), Brushes.White, new PointF(10, 300));
                g.DrawString($"{forecast.current.temp_c}", new Font("Myriad Pro", 200), Brushes.White, new PointF(5, 20));

                _bitmap.Save($"C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\PhotoHandler\\temp_pics\\{chatId}\\image.png", ImageFormat.Png);

                g.Dispose();
                _bitmap.Dispose();

                using (FileStream fl = new FileStream($"C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\PhotoHandler\\temp_pics\\{chatId}\\image.png", FileMode.Open))
                {
                    Message sentMessage = await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: InputFile.FromStream(fl),
                    caption: message,
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
                }

                Directory.Delete($"C:\\Users\\QForm\\Desktop\\csharp-telegram-bot\\PhotoHandler\\temp_pics\\{chatId}\\image.png");
            }
            catch (Exception e)
            {
                var exceptionHandler = new ExceptionHandler(botClient, chatId, cancellationToken, e);
                Message sentMessage = await exceptionHandler.Handle();
            }
        }
    }
}