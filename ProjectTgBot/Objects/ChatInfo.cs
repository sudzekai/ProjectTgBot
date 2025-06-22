using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTgBot.Objects
{
    class ChatInfo
    {
        public ChatInfo(string activeForm, long id, string previousMessage, List<string> data)
        {
            ActiveForm = activeForm;
            Id = id;
            PreviousMessage = previousMessage;
            Data = data;
        }

        public string ActiveForm { get; set; } = "";
        public long Id { get; set; } = 0;
        public string PreviousMessage { get; set; } = "";
        public List<string> Data { get; set; } = [];
    }
}
