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
        public bool IsLink = false;

        public ButtonInfo(string content, string answerOrLink, bool isLink)
        {
            Content = content;
            AnswerOrLink = answerOrLink;
            IsLink = isLink;
        }

        public override string ToString()
        {
            return $"(content: {Content}, answer or link: {AnswerOrLink}, IsLink: {IsLink})";
        }
    }

    enum ButtonType { Reply, Inline }
}
