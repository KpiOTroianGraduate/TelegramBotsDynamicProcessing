using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Base;
using Services.Interfaces;
using Telegram.Bot.Types;

namespace Services;

public class TelegramService : ServiceBase<TelegramService>, ITelegramService
{
    private readonly HttpClient _httpClient;
    private readonly ISqlUnitOfWork _unitOfWork;

    public TelegramService(ISqlUnitOfWork unitOfWork, ILogger<TelegramService> logger, HttpClient? httpClient = null) :
        base(logger)
    {
        _unitOfWork = unitOfWork;
        _httpClient = httpClient ?? new HttpClient();
    }

    public async Task ProcessMessageAsync(string botId, Update update)
    {
        await _unitOfWork.UserRepository.AddUserAsync().ConfigureAwait(false);
        await Task.Yield();
        //try
        //{
        //    var bot = new TelegramBotClient(botId, _httpClient);

        //    var isBotAvailable = await bot.TestApiAsync().ConfigureAwait(false);

        //    if (!isBotAvailable)
        //    {
        //        throw new ApiRequestException($"Bot ${botId} is unavailable");
        //    }

        //    var telegramMessage = new TelegramMessage
        //    {
        //        BotId = botId,
        //        Update = update
        //    };


        //    var factory = new ConnectionFactory
        //    {
        //        UserName = "guest",
        //        Password = "guest",
        //        Port = 5672,
        //        VirtualHost = "/"
        //    };

        //    var connection = factory.CreateConnection();
        //    var channel = connection.CreateModel();

        //    var queuesParams = channel.SetupQueueNames("test");
        //    channel.QueueDeclareWithRepeat(queuesParams, 5);

        //    var telegramMessageJson = JsonConvert.SerializeObject(telegramMessage);
        //    var body = Encoding.UTF8.GetBytes(telegramMessageJson);
        //    channel.BasicPublish(string.Empty, queuesParams.MainQueueName, null, body);
        //}
        //catch
        //{
        //    Logger.LogError($"ProcessMessage, ${botId}");
        //    throw;
        //}
    }
}