using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace ProjectTgBot.Objects
{
    public class FormInfo
    {
        public string FormName { get; set; } = "";
        public string StartMessage { get; set; } = "";
        public List<StepInfo> StepsInfo { get; set; } = [];

        public override string ToString()
        {
            return $"FormName: {FormName}\nStartMessage: {StartMessage}\nStepsInfo:{string.Join("\n", StepsInfo)}";
        }
    }
}
