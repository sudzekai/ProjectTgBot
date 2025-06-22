using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTgBot.Objects
{
    class FormButtonInfo
    {
        public string Message { get; set; } = "";
        public string Content { get; set; } = "";

        public FormButtonInfo(string content, string message)
        {
            Message = message;
            Content = content;
        }

        public override string ToString()
        {
            return $"(content: {Content})";
        }
    }
}
