using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTgBot.Objects
{
    class BotUser
    {
        public BotUser(long id, List<string> data)
        {
            Id = id;
            Data = data;
        }

        public long Id { get; set; } = 0;
        public string PreviousMessage { get; set; } = "";
        public List<string> Data { get; set; } = [];

    }
}
