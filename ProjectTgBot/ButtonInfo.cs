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
        public ButtonType Type = ButtonType.Reply;
        public string AnswerOrLink = "";

        public ButtonInfo(string content, ButtonType type, string answerOrLink)
        {
            Content = content;
            Type = type;
            AnswerOrLink = answerOrLink;
        }

        public override string ToString()
        {
            return $"\n(content: {Content}, type: {Type}, answer or link: {AnswerOrLink})";
        }
    }

    enum ButtonType { Reply, Link, Query }
}
