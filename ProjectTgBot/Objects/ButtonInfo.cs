using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTgBot.Objects
{
    internal class ButtonInfo
    {
        public string Content = "";
        public string AnswerOrLink = "";
        public bool IsLink = false;
        public bool IsFromStart = false;

        public ButtonInfo(string content, string answerOrLink, bool isLink = false, bool isFormStart = false)
        {
            Content = content;
            AnswerOrLink = answerOrLink;
            IsLink = isLink;
            IsFromStart = isFormStart;
        }

        public override string ToString()
        {
            return $"(content: {Content}, answer or link: {AnswerOrLink}, IsLink: {IsLink})";
        }
    }
}
