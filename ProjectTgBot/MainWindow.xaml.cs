using ProjectTgBot.UI_Elements;
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
            botListElement.IsRunning = true;
        }

        private void RunBotButton_Click(object sender, RoutedEventArgs e)
        {
            var commands = new[]
            {
                 new BotCommand { Command = CommandTextBox.Text, Description = BotCommandDescriptionTextBox.Text },
            };

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
            bot.OnError += Bot_OnError;
        }

        private Task Bot_OnError(Exception exception, Telegram.Bot.Polling.HandleErrorSource source)
        {
            MessageBox.Show(exception.ToString());
            return Task.CompletedTask;
        }

        private async Task Bot_OnUpdate(Update update)
        {
            foreach (ButtonInfo button in commandInfo.ButtonsInfo)
            {
                if (update.CallbackQuery.Data.Equals(button.Content) && !button.IsLink)
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
            MessageBox.Show($"Бот запущен: @{botinfo.Username}");
        }

        private async Task Bot_OnMessage(Message message, Telegram.Bot.Types.Enums.UpdateType type)
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
                        if (button.IsLink)
                        {
                            InlineKeyboardButton inlineButton = new(button.Content)
                            {
                                Url = button.AnswerOrLink
                            };
                            (markup as InlineKeyboardMarkup)?.AddButton(inlineButton);

                        }
                        else
                        {
                            (markup as InlineKeyboardMarkup)?.AddButton(button.Content);

                        }
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

        private void AddButtonInfoIfNotEmpty(CommandInfo commandInfo, string content, string answer, bool isLink)
        {
            if (!string.IsNullOrEmpty(content))
            {
                commandInfo.ButtonsInfo.Add(new ButtonInfo(content, answer, isLink));
            }
        }

        private void ButtonType_Checked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is RadioButton radioButton && sender is StackPanel panel && (e.OriginalSource as RadioButton).Content is not null)
            {
                buttonType = radioButton.Content.ToString() switch
                {
                    "Inline" => ButtonType.Inline,
                    _ => ButtonType.Reply,
                };
                MessageBox.Show(buttonType.ToString());

                foreach (ButtonInfoPanel buttonPanel in ButtonsPanel.Children)
                {
                    buttonPanel.IsInline = buttonType.Equals(ButtonType.Inline);
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

        private void botListElement_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            botListElement.IsRunning = !botListElement.IsRunning;
        }

        private void AddNewTelegramBtnButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonsPanel.Children.Add(new ButtonInfoPanel() { IsInline = buttonType.Equals(ButtonType.Inline) });
        }

        private void AddNewCommand_Click(object sender, RoutedEventArgs e)
        {
            commandInfo = new CommandInfo
            {
                Command = CommandTextBox.Text,
                Message = MessageTextBox.Text,
                ButtonType = buttonType
            };
            foreach (ButtonInfoPanel panel in ButtonsPanel.Children)
            {
                AddButtonInfoIfNotEmpty(commandInfo, panel.ButtonContent, panel.ButtonAnswer, panel.IsLink);
            }
        }

        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}