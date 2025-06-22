using ProjectTgBot.Objects;
using System;
using System.Collections.Generic;
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

namespace ProjectTgBot.UI_Elements
{
    /// <summary>
    /// Логика взаимодействия для TelegramChatPreview.xaml
    /// </summary>
    public partial class TelegramChatPreview : UserControl
    {
        public int Columns { get; set; } = 0;
        public TelegramChatPreview()
        {
            InitializeComponent();
            MessageFromBotBorder.Width = 175;
        }
        public TelegramChatPreview(List<string> buttons, string message, string messageFromUser, ButtonType type = 0)
        {
            InitializeComponent();
            int columns = buttons.Count;
            BotMessage.Text = message;
            UserMessage.Text = messageFromUser;
            
            if (type == ButtonType.Inline)
            {
                for (int i = 0; i < columns; i++)
                {
                    ButtonsContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(300 / columns) });
                }
                for (int i = 0; i < columns; i++)
                {
                    Border border = new Border() { Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)), Height = 25, CornerRadius = new CornerRadius(6), Margin = new Thickness(0.5) };
                    TextBlock textBlock = new TextBlock() { Foreground = new SolidColorBrush(Colors.White), FontSize = 12, Text = buttons[i], VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
                    border.Child = textBlock;
                    Grid.SetColumn(border, i);
                    ButtonsContainer.Children.Add(border);
                }
            }
            else
            {
                ReplyButtonsContainer.Visibility = Visibility.Visible;

                for (int i = 0; i < columns; i++)
                {
                    ReplyButtonsContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new(400 / columns) });
                }
                for (int i = 0; i < columns; i++)
                {
                    Border border = new Border() { Background = new SolidColorBrush(Color.FromRgb(204, 204, 204)), Height = 40, CornerRadius = new CornerRadius(6), Margin = new Thickness(1, 3, 1, 3) };
                    TextBlock textBlock = new TextBlock() { Foreground = new SolidColorBrush(Colors.White), FontSize = 12, Text = buttons[i], VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, FontWeight = FontWeights.Bold };
                    border.Child = textBlock;
                    Grid.SetColumn(border, i);
                    ReplyButtonsContainer.Children.Add(border);
                }
            }
            
        }
    }
}
