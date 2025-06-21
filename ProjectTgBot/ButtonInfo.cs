using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTgBot
{
    internal class ButtonInfo
    {
        public string Content = "";
        public string AnswerOrLink = "";

        public ButtonInfo(string content, string answerOrLink)
        {
            Content = content;
            AnswerOrLink = answerOrLink;
        }

        public override string ToString()
        {
            return $"\n(content: {Content}, answer or link: {AnswerOrLink})";
        }
    }

    enum ButtonType { Reply, Link, Query }
}
