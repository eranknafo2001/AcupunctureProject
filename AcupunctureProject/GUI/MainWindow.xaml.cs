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

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string folder;
        public MainWindow()
        {
            InitializeComponent();
            string[] tempFolder = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');
            folder = "";
            for (int i = 0; i < tempFolder.Length - 1; i++)
            {
                folder += tempFolder[i] + "\\";
            }
            BitmapImage backimagesource = new BitmapImage();
            backimagesource.BeginInit();
            backimagesource.UriSource = new Uri(folder + "images\\backpic.jpg");
            backimagesource.EndInit();
            backImage.Source = backimagesource;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NewPatient p = new NewPatient();
            p.Show();
        }
    }
}
