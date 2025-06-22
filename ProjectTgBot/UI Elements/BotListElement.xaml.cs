using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectTgBot.UI_Elements
{
    /// <summary>
    /// Логика взаимодействия для BotListElement.xaml
    /// </summary>
    public partial class BotListElement : UserControl, INotifyPropertyChanged
    {
        public string BotName
        {
            get { return (string)GetValue(BotNameProperty); }
            set { SetValue(BotNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BotName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BotNameProperty =
            DependencyProperty.Register("BotName", typeof(string), typeof(BotListElement), new PropertyMetadata(string.Empty));

        private bool _isRunning;

        public bool IsRunning
        {
            get => _isRunning;
            set { Set(ref _isRunning, value); }

        }

        // Using a DependencyProperty as the backing store for IsRunning.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register("IsRunning", typeof(bool), typeof(BotListElement), new PropertyMetadata(false));

        public BotListElement()
        {
            InitializeComponent();
            BotStatusTextBlock.Text = "Бот выключен";

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (IsRunning)
            {
                StatusBackground.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1d3712"));
                BotStatusTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#318617"));
                BotStatusTextBlock.Text = "Бот включен";
            }
            else if (!IsRunning)
            {
                StatusBackground.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2a1215"));
                BotStatusTextBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d31b1b"));
                BotStatusTextBlock.Text = "Бот выключен";

            }
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
