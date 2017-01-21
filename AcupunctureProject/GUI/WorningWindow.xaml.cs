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
    /// Interaction logic for YasNoWindow.xaml
    /// </summary>
    public partial class WorningWindow : Window
    {
        private bool finished;

        public WorningWindow(string title, string text)
        {
            InitializeComponent();
            Title = title;
            myText.Text = text;
            finished = false;
        }

        public void ShowAndWait()
        {
            Show();
            while (!finished) ;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            finished = true;
        }
    }
}
