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
    /// Логика взаимодействия для FormButtonInfoPanel.xaml
    /// </summary>
    public partial class FormButtonInfoPanel : UserControl
    {
        public string Text {  get; set; }
        public FormButtonInfoPanel()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler TextChanged;

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonContentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = (sender as TextBox).Text;
            TextChanged?.Invoke(this, e);
        }
    }
}
