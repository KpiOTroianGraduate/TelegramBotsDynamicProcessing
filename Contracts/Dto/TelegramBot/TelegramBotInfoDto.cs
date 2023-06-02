using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.TelegramBot
{
    public class TelegramBotInfoDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;

        public string? LastName { get; set; }

        public string? Username { get; set; }

        public string? Avatar { get; set; }
        public string? Token { get; set; }

        public bool IsActive { get; set; }

        public Guid UserId { get; set; }
    }
}
