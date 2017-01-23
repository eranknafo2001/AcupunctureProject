using AcupunctureProject.database;
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

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        private string folder;
        private Window perent;

        public Main(Window perent)
        {
            InitializeComponent();
            this.perent = perent;
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

        private void newPatientMI_Click(object sender, RoutedEventArgs e)
        {
            NewPatient p = new NewPatient();
            p.Show();
        }

        private void newMeetingMI_Click(object sender, RoutedEventArgs e)
        {
            
            perent.Content = new NewMeeting(perent);
        }

        private void PatientListMI_Click(object sender, RoutedEventArgs e)
        {
            PatientList p = new PatientList();
            p.Show();
        }

        private void pointsListMI_Click(object sender, RoutedEventArgs e)
        {
            List<database.Point> points = Database.Instance.getAllPoints();
            new PointInfo(points[new Random().Next(0, points.Count - 1)]).Show();
        }
    }
}
