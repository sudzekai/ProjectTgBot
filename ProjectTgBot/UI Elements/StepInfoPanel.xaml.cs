using ProjectTgBot.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для StepInfoPanel.xaml
    /// </summary>
    public partial class StepInfoPanel : UserControl
    {
        public StepInfo StepInfo { get; set; } = new();

        public StepInfoPanel()
        {
            InitializeComponent();
        }

        private void ToggleFilterToggleButton_Click(object sender, RoutedEventArgs e)
        {
            StepInfo.IsFilterEnabled = ToggleFilterToggleButton.IsChecked.Value;
        }

        private void AddNewTelegramBtnButton_Click(object sender, RoutedEventArgs e)
        {
            var buttonInfoPanel = new FormButtonInfoPanel();
            buttonInfoPanel.TextChanged += ButtonInfoPanel_TextChanged;
            ButtonsPanel.Children.Add(buttonInfoPanel);
        }

        private void ButtonInfoPanel_TextChanged(object sender, RoutedEventArgs e)
        {
            StepInfo.Buttons.Clear();

            foreach (FormButtonInfoPanel panel in ButtonsPanel.Children)
            {
                StepInfo.Buttons.Add(new(panel.Text));
            }
        }

        private void DeleteCommand_Click(object sender, RoutedEventArgs e)
        {
            var parentContainer = VisualTreeHelper.GetParent(this) as Panel;

            if (parentContainer != null)
            {
                parentContainer.Children.Remove(this);
            }
        }

        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StepInfo.BotMessage = (sender as TextBox).Text;
        }
    }
}
