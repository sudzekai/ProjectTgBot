using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectTgBot
{
    public partial class MainWindow : Window
    {
        TelegramBotClient bot;
        readonly CancellationTokenSource _cts = new();
        private readonly ButtonType[] buttonTypes = new ButtonType[5];
        CommandInfo commandInfo;

        public MainWindow()
        {
            InitializeComponent();
            BotSettingsWindow.Visibility = Visibility.Collapsed;
            BotListStackPanel.Visibility = Visibility.Visible;
        }

        private void RunBotButton_Click(object sender, RoutedEventArgs e)
        {
            commandInfo = new CommandInfo
            {
                Command = CommandTextBox.Text,
                Message = MessageTextBox.Text
            };

            AddButtonInfoIfNotEmpty(commandInfo, Button1Content.Text, buttonTypes[0], Button1Answer.Text);
            AddButtonInfoIfNotEmpty(commandInfo, Button2Content.Text, buttonTypes[1], Button2Answer.Text);
            AddButtonInfoIfNotEmpty(commandInfo, Button3Content.Text, buttonTypes[2], Button3Answer.Text);
            AddButtonInfoIfNotEmpty(commandInfo, Button4Content.Text, buttonTypes[3], Button4Answer.Text);
            AddButtonInfoIfNotEmpty(commandInfo, Button5Content.Text, buttonTypes[4], Button5Answer.Text);

            MessageBox.Show(commandInfo.ToString());

            try
            {
                bot = new(token: TokenTextBox.Text, cancellationToken: _cts.Token);
            }
            catch
            {
                return;
            }

            bot.OnMessage += Bot_OnMessage;
        }

        private async Task Bot_OnMessage(Telegram.Bot.Types.Message message, Telegram.Bot.Types.Enums.UpdateType type)
        {
            if (message.Text.Contains(commandInfo.Command))
            {
                ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup();
                foreach (ButtonInfo button in commandInfo.ButtonsInfo)
                {
                    markup.AddButton(button.Content);
                }
                await bot.SendMessage(message.Chat.Id, commandInfo.Message, replyMarkup: markup);
            }
            foreach (ButtonInfo button in commandInfo.ButtonsInfo)
            {
                if (message.Text.Equals(button.Content))
                {
                    await bot.SendMessage(message.Chat.Id, button.AnswerOrLink);
                }
            }
        }

        private void AddButtonInfoIfNotEmpty(CommandInfo commandInfo, string content, ButtonType type, string answer)
        {
            if (!string.IsNullOrEmpty(content))
            {
                commandInfo.ButtonsInfo.Add(new ButtonInfo(content, type, answer));
            }
        }

        private void ButtonType_Checked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is RadioButton radioButton && sender is StackPanel panel)
            {
                var buttonType = radioButton.Content.ToString() switch
                {
                    "Query" => ButtonType.Query,
                    "Reply" => ButtonType.Reply,
                    _ => ButtonType.Link
                };

                if (panel.Name.Length > 0 && char.IsDigit(panel.Name.Last()))
                {
                    int index = panel.Name.Last() - '1';
                    if (index >= 0 && index < buttonTypes.Length)
                    {
                        buttonTypes[index] = buttonType;
                    }
                }
            }
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BotListStackPanel.Visibility = Visibility.Collapsed;
            BotSettingsWindow.Visibility = Visibility.Visible;
        }

        private void BotListButton_Click(object sender, RoutedEventArgs e)
        {
            BotSettingsWindow.Visibility = Visibility.Collapsed;
            BotListStackPanel.Visibility = Visibility.Visible;
        }
    }
}