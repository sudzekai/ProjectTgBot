using ProjectTgBot.Objects;
using ProjectTgBot.UI_Elements;
using System.Text;
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

        List<CommandInfo> commandsInfo;
        List<FormInfo> formsInfo;

        BotListElement activeBot = new();

        public MainWindow()
        {
            InitializeComponent();
            botListElement.IsRunning = false;
        }

        private void RunBotButton_Click(object sender, RoutedEventArgs e)
        {
            commandsInfo = new();
            formsInfo = new();
            List<BotCommand> commands = [];
            foreach (CommandInfoPanel telegramCommand in CommandsPanel.Children)
            {
                commands.Add(new(telegramCommand.Command, telegramCommand.CommandDescription));
                CommandInfo telegramCommandInfo = new() { Command = telegramCommand.Command, ButtonType = telegramCommand.ButtonType, Message = EscapeMarkdown(telegramCommand.BotAnswer), CommandDescription = telegramCommand.CommandDescription };
                foreach (ButtonInfoPanel panel in telegramCommand.ButtonsPanel.Children)
                {
                    commandsInfo.Select(x => x.Command = telegramCommand.Command);
                    AddButtonInfoIfNotEmpty(telegramCommandInfo, panel.ButtonContent, EscapeMarkdown(panel.ButtonAnswer), panel.IsLink);
                }
                commandsInfo.Add(telegramCommandInfo);
            }

            MessageBox.Show(string.Join("\n\n", commandsInfo));

            try
            {
                bot = new(token: TokenTextBox.Text, cancellationToken: _cts.Token);
            }
            catch
            {
                return;
            }
            botListElement.IsRunning = true;

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
            CommandInfo command;
            try
            {
                command = commandsInfo.First(x =>
                {
                    foreach (ButtonInfo button in x.ButtonsInfo)
                    {
                        if (button.Content == update.CallbackQuery.Data)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                );
            }
            catch (InvalidOperationException)
            {
                return;
            }

            foreach (ButtonInfo button in command.ButtonsInfo)
            {
                if (update.CallbackQuery.Data.Equals(button.Content) && !button.IsLink)
                {
                    await bot.SendMessage(update.CallbackQuery.Message.Chat.Id, button.AnswerOrLink, parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
                    await bot.AnswerCallbackQuery(update.CallbackQuery.Id);

                    return;
                }
            }
        }

        private async Task GetMeBot()
        {
            var botinfo = await bot.GetMe();
            MainBodyNameTextBlock.Text = botinfo.Username;

            MessageBox.Show($"Бот запущен: @{botinfo.Username}");
        }

        private async Task Bot_OnMessage(Message message, Telegram.Bot.Types.Enums.UpdateType type)
        {
            CommandInfo command;
            try
            {
                command = commandsInfo.First(x => x.Command == message.Text);
            }
            catch (InvalidOperationException)
            {
                StringBuilder text = new();
                foreach (var info in commandsInfo)
                {
                    text.AppendLine(@$"*{info.Command}* \- {info.CommandDescription}");
                }
                await bot.SendMessage(message.From.Id, $"Я не знаю такой команды\nКоманды:\n{text}", parseMode:Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
                return;
            }
            
            if (message.Text.Equals(command.Command))
            {
                ReplyMarkup markup;

                if (command.ButtonType.Equals(ButtonType.Reply))
                {
                    markup = new ReplyKeyboardMarkup();
                    foreach (ButtonInfo button in command.ButtonsInfo)
                    {
                        (markup as ReplyKeyboardMarkup)?.AddButton(button.Content);
                    }
                }

                else
                {
                    markup = new InlineKeyboardMarkup();
                    foreach (ButtonInfo button in command.ButtonsInfo)
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
                await bot.SendMessage(message.Chat.Id, command.Message, replyMarkup: markup, parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
            }
            foreach (ButtonInfo button in command.ButtonsInfo)
            {
                if (message.Text.Equals(button.Content))
                {
                    await bot.SendMessage(message.Chat.Id, button.AnswerOrLink, parseMode:Telegram.Bot.Types.Enums.ParseMode.MarkdownV2);
                    return;
                }
            }
        }
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            activeBot = (sender as BotListElement);
            MainBodyNameTextBlock.Text = (sender as BotListElement).BotName;
            BotListStackPanel.Visibility = Visibility.Collapsed;
            BotSettingsWindow.Visibility = Visibility.Visible;
        }

        private void BotListButton_Click(object sender, RoutedEventArgs e)
        {
            activeBot.BotName = MainBodyNameTextBlock.Text;
            MainBodyNameTextBlock.Text = "Боты";
            BotSettingsWindow.Visibility = Visibility.Collapsed;
            BotListStackPanel.Visibility = Visibility.Visible;
        }

        private void botListElement_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            botListElement.IsRunning = !botListElement.IsRunning;
        }

        private void AddNewTelegramCommandButton_Click(object sender, RoutedEventArgs e)
        {
            if (CommandsPanel.Children.Count == 0 || (!string.IsNullOrWhiteSpace((CommandsPanel.Children[CommandsPanel.Children.Count-1] as CommandInfoPanel).Command) && !string.IsNullOrWhiteSpace((CommandsPanel.Children[CommandsPanel.Children.Count - 1] as CommandInfoPanel).BotAnswer)))
            {
                CommandsPanel.Children.Add(new CommandInfoPanel());
            }
        }
        private void AddButtonInfoIfNotEmpty(CommandInfo commandInfo, string content, string answer, bool isLink)
        {
            if (!string.IsNullOrEmpty(content))
            {
                commandInfo.ButtonsInfo.Add(new ButtonInfo(content, answer, isLink));
            }
        }

        static string EscapeMarkdown(string text)
        {
            if (text.StartsWith('f'))
            {
                text = text.TrimStart('f');
                return text;
            }
            char[] reservedChars = new char[] { '*', '_', '[', ']', '(', ')', '!', '#', '>', '-', '+', '*', '|', '`' };
            StringBuilder escapedText = new StringBuilder();

            foreach (char c in text)
            {
                if (Array.Exists(reservedChars, element => element == c))
                {
                    escapedText.Append('\\');
                }
                escapedText.Append(c);
            }

            return escapedText.ToString();
        }

        private void AddNewTelegramFormButton_Click(object sender, RoutedEventArgs e)
        {
            FormsPanel.Children.Add(new FormInfoPanel());
        }
    }
}