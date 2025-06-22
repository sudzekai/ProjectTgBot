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
    /// Логика взаимодействия для StepInfoPanel.xaml
    /// </summary>
    public partial class StepInfoPanel : UserControl
    {
        public StepInfoPanel()
        {
            InitializeComponent();
        }

        private void ToggleFilterToggleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddNewTelegramBtnButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonsPanel.Children.Add(new FormButtonInfoPanel());
        }

        private void CommitChanges_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteCommand_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
