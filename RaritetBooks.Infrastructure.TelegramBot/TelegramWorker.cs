using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RaritetBooks.Infrastructure.TelegramBot;

public class TelegramWorker : BackgroundService
{
    private readonly TelegramOptions _telegramOptions;
    private readonly ILogger<TelegramWorker> _logger;

    public TelegramWorker(
        IOptions<TelegramOptions> telegramOptions,
        ILogger<TelegramWorker> logger)
    {
        _telegramOptions = telegramOptions.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_telegramOptions.Token != null)
        {
            var botClient = new TelegramBotClient(_telegramOptions.Token);

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = []
            };
        
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await botClient.ReceiveAsync(
                        updateHandler: HandleUpdateAsync,
                        pollingErrorHandler: HandleErrorAsync,
                        receiverOptions: receiverOptions,
                        cancellationToken: stoppingToken);  
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while receive telegram message");
                }
            }
        }
    }
    
    private async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;
        
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        var inlineKeyboardA = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("*Menu 1.1", "https://habr.com/"),
                    InlineKeyboardButton.WithCallbackData("*Menu 1.2", "button1"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("*Menu 1.3", "button2"),
                    InlineKeyboardButton.WithCallbackData("*Menu 1.4", "button3"),
                },
            });
        
        var inlineKeyboardB = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("*Menu 2.1", "https://habr.com/"),
                    InlineKeyboardButton.WithCallbackData("*Menu 2.2", "button1"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("*Menu 2.3", "button2"),
                    InlineKeyboardButton.WithCallbackData("*Menu 2.4", "button3"),
                },
            });

        switch (messageText)
        {
            case "1":
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Chapter *1: 1.1, 1.2",
                    replyMarkup: inlineKeyboardA,
                    cancellationToken: cancellationToken);
                break;
            
            case "2":
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Chapter *2: 2.1, 2.2",
                    replyMarkup: inlineKeyboardB,
                    cancellationToken: cancellationToken);
                break;
        }
        
        _logger.LogInformation("Received a '{messageText}' message in chat {chatId}.", messageText, chatId);
    }

    private Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        
        _logger.LogInformation(errorMessage);
        return Task.CompletedTask;
    }
}