using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для ButtonInfoPanel.xaml
    /// </summary>
    public partial class ButtonInfoPanel : UserControl, INotifyPropertyChanged
    {
        private string _buttonContent;
        public string ButtonContent
        {
            get => _buttonContent;
            set
            {
                Set(ref _buttonContent, value);
            }
        }
        
        private string _buttonAnswer;
        public string ButtonAnswer 
        {
            get => _buttonAnswer;
            set
            {
                Set(ref _buttonAnswer, value);
            }
        }

        private bool _isInline;

        public bool IsInline
        {
            get => _isInline;
            set
            {
                Set(ref _isInline, value);
            }
        }

        public bool IsLink { get; set; }

        public ButtonInfoPanel()
        {
            InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            IsLink = (sender as ToggleButton).IsChecked.Value;
            SecondTextBlock.Text = IsLink ? "Ссылка:" : "Ответ:";
            ButtonAnswerTextBox.ToolTip = IsLink ? "Ссылка, по которой перейдет пользователь нажавший на кнопку" : "Сообщение, которое бот отправит при нажатии на кнопку";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            if (PropertyName == "IsInline")
            {
                if (!IsInline)
                {
                    ToggleLinkToggleButton.Visibility = Visibility.Hidden;
                    ButtonContentTextBox.ToolTip = "Текст, который будет изображен на кнопке и отправлен пользователем, при нажатии на неё";
                }
                else
                {
                    ToggleLinkToggleButton.Visibility = Visibility.Visible;
                    ButtonContentTextBox.ToolTip = "Текст, который будет изображен на кнопке";
                }
            }
        }
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var parentContainer = VisualTreeHelper.GetParent(this) as Panel;

            if (parentContainer != null)
            {
                parentContainer.Children.Remove(this);
            }
        }
    }
}
