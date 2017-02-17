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
using System.Windows.Shapes;

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for MoreInfoOnPatient.xaml
    /// </summary>
    public partial class MoreInfoOnPatient : Window
    {
        public MoreInfoOnPatient()
        {
            InitializeComponent();
        }

        private void Enter(object sender, KeyEventArgs e)
        {
            if (sender.GetType() != typeof(TextBox))
                return;
            if(e.Key==Key.Enter)
            {
                TextBox textBox = (TextBox)sender;
                int temp = textBox.SelectionStart;
                textBox.Text = textBox.Text.Remove(temp, textBox.SelectionLength);
                textBox.Text = textBox.Text.Insert(temp, "\n");
                textBox.SelectionLength = 0;
                textBox.SelectionStart = temp + 1;
            }
        }
    }
}
