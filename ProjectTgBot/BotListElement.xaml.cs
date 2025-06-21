using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot.Types;

namespace ProjectTgBot
{
    /// <summary>
    /// Логика взаимодействия для BotListElement.xaml
    /// </summary>
    public partial class BotListElement : UserControl
    {
        public static readonly DependencyProperty BotNameProperty =
            DependencyProperty.Register("BotName", typeof(string), typeof(BotListElement), new PropertyMetadata(string.Empty));

        public string BotName
        {
            get { return (string)GetValue(BotNameProperty); }
            set { SetValue(BotNameProperty, value); }
        }

        public string BotStatus
        {
            get { return (string)GetValue(BotStatusProperty); }
            set { SetValue(BotStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BotStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BotStatusProperty =
            DependencyProperty.Register("BotStatus", typeof(string), typeof(BotListElement), new PropertyMetadata(string.Empty));



        public BotListElement()
        {
            this.InitializeComponent();
        }
    }
}
