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
    public partial class YasNoWindow : Window
    {
        private bool retVal = false;
        private bool finished = false;

        public YasNoWindow(string title,string text)
        {
            Title = text;
            myText.Text = text;
            InitializeComponent();
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            retVal = false;
            finished = true;
        }
        
        public bool ShowAndWait()
        {
            Show();
            while (!finished) ;
            return retVal;
        }

        private void yas_Click(object sender, RoutedEventArgs e)
        {
            retVal = true;
            finished = true;
        }
    }
}
