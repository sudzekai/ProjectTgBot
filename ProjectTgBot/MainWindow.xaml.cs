using System.Windows;
using System.Windows.Controls;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectTgBot
{
    public partial class MainWindow : Window
    {
        TelegramBotClient bot;
        readonly CancellationTokenSource _cts = new();
        ButtonType buttonType = ButtonType.Reply;
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
                Message = MessageTextBox.Text,
                ButtonType = buttonType
            };

            var commands = new[]
            {
                 new BotCommand { Command = CommandTextBox.Text, Description = BotCommandDescriptionTextBox.Text },
            };

            AddButtonInfoIfNotEmpty(commandInfo, Button1Content.Text, Button1Answer.Text);
            AddButtonInfoIfNotEmpty(commandInfo, Button2Content.Text, Button2Answer.Text);
            AddButtonInfoIfNotEmpty(commandInfo, Button3Content.Text, Button3Answer.Text);
            AddButtonInfoIfNotEmpty(commandInfo, Button4Content.Text, Button4Answer.Text);
            AddButtonInfoIfNotEmpty(commandInfo, Button5Content.Text, Button5Answer.Text);
            MessageBox.Show(commandInfo.ToString());

            try
            {
                bot = new(token: TokenTextBox.Text, cancellationToken: _cts.Token);
            }
            catch
            {
                return;
            }

            bot.SetMyCommands(commands);
            GetMeBot();
            bot.OnMessage += Bot_OnMessage;
            bot.OnUpdate += Bot_OnUpdate;
        }

        private async Task Bot_OnUpdate(Update update)
        {
            foreach (ButtonInfo button in commandInfo.ButtonsInfo)
            {
                if (update.CallbackQuery.Data.Equals(button.Content))
                {
                    await bot.SendMessage(update.CallbackQuery.Message.Chat.Id, button.AnswerOrLink);
                    await bot.AnswerCallbackQuery(update.CallbackQuery.Id);
                    return;
                }
            }
        }

        private async Task GetMeBot()
        {
            var botinfo = await bot.GetMe();
            MessageBox.Show($"{botinfo.Username}");
        }

        private async Task Bot_OnMessage(Telegram.Bot.Types.Message message, Telegram.Bot.Types.Enums.UpdateType type)
        {
            if (message.Text.Equals(commandInfo.Command))
            {
                ReplyMarkup markup;

                if (commandInfo.ButtonType.Equals(ButtonType.Reply))
                {
                    markup = new ReplyKeyboardMarkup();
                    foreach (ButtonInfo button in commandInfo.ButtonsInfo)
                    {
                        (markup as ReplyKeyboardMarkup)?.AddButton(button.Content);
                    }
                }

                else
                {
                    markup = new InlineKeyboardMarkup();
                    foreach (ButtonInfo button in commandInfo.ButtonsInfo)
                    {
                        (markup as InlineKeyboardMarkup)?.AddButton(button.Content);
                    }
                }
                await bot.SendMessage(message.Chat.Id, commandInfo.Message, replyMarkup: markup);
            }
            foreach (ButtonInfo button in commandInfo.ButtonsInfo)
            {
                if (message.Text.Equals(button.Content))
                {
                    await bot.SendMessage(message.Chat.Id, button.AnswerOrLink);
                    return;
                }
            }
        }

        private void AddButtonInfoIfNotEmpty(CommandInfo commandInfo, string content, string answer)
        {
            if (!string.IsNullOrEmpty(content))
            {
                commandInfo.ButtonsInfo.Add(new ButtonInfo(content, answer));
            }
        }

        private void ButtonType_Checked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is RadioButton radioButton && sender is StackPanel panel)
            {
                buttonType = radioButton.Content.ToString() switch
                {
                    "Query" => ButtonType.Query,
                    "Reply" => ButtonType.Reply,
                    _ => ButtonType.Link
                };
                MessageBox.Show(buttonType.ToString());

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