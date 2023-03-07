using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Models;
using DAL.UnitOfWork;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Extensions;
using RabbitMQ.Client.Extensions.RepeatQueues;
using Services.Base;
using Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Services
{
    public class TelegramService: ServiceBase<TelegramService>, ITelegramService
    {
        private readonly HttpClient _httpClient;

        public TelegramService(IUnitOfWorkFactory unitOfWorkFactory, Logger<TelegramService> logger, HttpClient? httpClient = null) : base(unitOfWorkFactory, logger)
        {
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task ProcessMessage(string botId, Update update)
        {
            try
            {
                var bot = new TelegramBotClient(botId, _httpClient);

                var isBotAvailable = await bot.TestApiAsync();

                if (!isBotAvailable)
                {
                    throw new ApiRequestException($"Bot ${botId} is unavailable");
                }

                var telegramMessage = new TelegramMessage
                {
                    BotId = botId,
                    Update = update
                };


                var factory = new ConnectionFactory
                {
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672,
                    VirtualHost = "/"
                };

                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                var queuesParams = channel.SetupQueueNames("test");
                channel.QueueDeclareWithRepeat(queuesParams, 5);

                var telegramMessageJson = JsonConvert.SerializeObject(telegramMessage);
                var body = Encoding.UTF8.GetBytes(telegramMessageJson);
                channel.BasicPublish(string.Empty, queuesParams.MainQueueName, null, body);
            }
            catch
            {
                Logger.LogError($"ProcessMessage, ${botId}");
                throw;
            }
        }
    }
}
