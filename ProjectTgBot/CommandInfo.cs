using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTgBot
{
    internal class CommandInfo
    {
        public string Command = "";
        public List<ButtonInfo> ButtonsInfo = [];
        public string Message = "";
        public ButtonType ButtonType = ButtonType.Reply;

        public override string ToString()
        {
            return $"command: {Command},\nType:{ButtonType},\n Buttons: {string.Join(", ", ButtonsInfo)}, \nMessage: {Message}";
        }
    }
}
