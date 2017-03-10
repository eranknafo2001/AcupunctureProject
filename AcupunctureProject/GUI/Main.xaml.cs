using AcupunctureProject.Database;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.ComponentModel;

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
            BitmapImage BackImageSource = new BitmapImage();
            BackImageSource.BeginInit();
            BackImageSource.UriSource = new Uri(folder + "images\\backpic.jpg");
            BackImageSource.EndInit();
            BackImage.Source = BackImageSource;

            VersionLab.Content += Version.Value;
            VersionLab.Visibility = Version.ShowVersion ? Visibility.Visible : Visibility.Hidden;
        }

        private void NewPatientMI_Click(object sender, RoutedEventArgs e)
        {
            new NewPatient().Show();
        }

        private void NewMeetingMI_Click(object sender, RoutedEventArgs e)
        {
            new NewMeetingWindow().Show();
        }

        private void PatientListMI_Click(object sender, RoutedEventArgs e)
        {
            new PatientList().Show();
        }

        private void PointsListMI_Click(object sender, RoutedEventArgs e)
        {
            List<Database.Point> points = DatabaseConnection.Instance.GetAllPoints();
            new PointInfo(points[new Random().Next(0, points.Count - 1)]).Show();
        }

        private void SettingMI_Click(object sender, RoutedEventArgs e)
        {
            new SettingWindow().Show();
        }

        private void Updates_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo i = new ProcessStartInfo(folder + "UpdateApp.exe", folder);
            i.UseShellExecute = true;
            i.CreateNoWindow = true;
            i.Verb = "runas";
            try
            {
                Process.Start(i);
                Application.Current.Shutdown();
            }
            catch (Win32Exception)
            {
                MessageBox.Show("לא יכול לגשת לקבצים", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading);
            }
        }
    }
}
