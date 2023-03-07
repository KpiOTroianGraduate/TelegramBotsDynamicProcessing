using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Services.Interfaces
{
    public interface ITelegramService
    {
        public Task ProcessMessage(string botId, Update update);
    }
}
