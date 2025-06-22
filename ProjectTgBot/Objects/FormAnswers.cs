using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTgBot.Objects
{
    class FormAnswers
    {
        public FormAnswers(long chatId, string formName, List<string> answers)
        {
            ChatId = chatId;
            FormName = formName;
            Answers = answers;
        }

        public long ChatId { get; set; } = 0;
        public string FormName { get; set; } = "";
        public List<string> Answers { get; set; } = [];
    }
}
