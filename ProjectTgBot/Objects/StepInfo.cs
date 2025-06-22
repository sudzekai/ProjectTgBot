using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTgBot.Objects
{
    public class StepInfo
    {
        public string BotMessage { get; set; } = "";
        public List<FormButtonInfo> Buttons { get; set; } = [];
        public bool IsFilterEnabled { get; set; } = false;

        public StepInfo() { }

        public override string ToString()
        {
            return $"(BotMessage: {BotMessage}\nButtons: {string.Join("\n", Buttons)}\nIsFilterEnabled: {IsFilterEnabled}";
        }
    }
}
