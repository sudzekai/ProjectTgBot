using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectTgBot.UI_Elements
{
    /// <summary>
    /// Логика взаимодействия для CommandInfoPanel.xaml
    /// </summary>
    public partial class CommandInfoPanel : UserControl, INotifyPropertyChanged
    {
        public ButtonType ButtonType { get; set; } = 0;

        public string Command
        {
            get => _command;
            set
            {
                Set(ref _command, value);
            }
        }

        public string CommandDescription 
        {
            get => _commandDescription;
            set
            {
                Set(ref _commandDescription, value);
            }
        }

        public string BotAnswer
        {
            get => _botAnswer;
            set
            {
                Set(ref _botAnswer, value);
            }
        }

        public WrapPanel ButtonsInfoWrapPanel = new();

        private string _command;

        private string _commandDescription;

        private string _botAnswer;


        public CommandInfoPanel()
        {
            InitializeComponent();
        }

        private void AddNewTelegramBtnButton_Click(object sender, RoutedEventArgs e)
        {
            if ((ButtonsPanel.Children.Count == 0 || !string.IsNullOrWhiteSpace((ButtonsPanel.Children[ButtonsPanel.Children.Count - 1] as ButtonInfoPanel).ButtonContent) && ButtonsPanel.Children.Count < 6))
            {
                ButtonsPanel.Children.Add(new ButtonInfoPanel() { IsInline = ButtonType.Equals(ButtonType.Inline) });
            }
        }

        private void ButtonType_Checked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is RadioButton radioButton && sender is StackPanel panel && (e.OriginalSource as RadioButton).Content is not null)
            {
                ButtonType = radioButton.Content.ToString() switch
                {
                    "Inline" => ButtonType.Inline,
                    _ => ButtonType.Reply,
                };

                foreach (ButtonInfoPanel buttonPanel in ButtonsPanel.Children)
                {
                    buttonPanel.IsInline = ButtonType.Equals(ButtonType.Inline);
                }

            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;


        private void CommitChanges_Click(object sender, RoutedEventArgs e)
        {
            ButtonsInfoWrapPanel = ButtonsPanel;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
