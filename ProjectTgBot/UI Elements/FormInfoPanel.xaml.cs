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
    /// Логика взаимодействия для FormInfoPanel.xaml
    /// </summary>
    public partial class FormInfoPanel : UserControl
    {
        public FormInfoPanel()
        {
            InitializeComponent();
        }

        private void AddNewStepButton_Click(object sender, RoutedEventArgs e)
        {
            StepsPanel.Children.Add(new StepInfoPanel());
        }

        private void DeleteCommand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CommitChanges_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
