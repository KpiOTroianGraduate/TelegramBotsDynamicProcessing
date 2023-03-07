using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Contracts.Models
{
    public class TelegramMessage
    {
        public string BotId { get; set; } = null!;

        public Update Update { get; set; } = null!;
    }
}
