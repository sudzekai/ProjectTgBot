using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTgBot.Objects
{
    public class FormButtonInfo
    {
        public string Title { get; set; } = "";

        public FormButtonInfo(string message)
        {
            Title = message;
        }

        public override string ToString()
        {
            return $"Title: {Title}";
        }
    }
}
