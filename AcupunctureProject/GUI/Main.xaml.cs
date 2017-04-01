using AcupunctureProject.Database;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using DPoint = AcupunctureProject.Database.Point;
using DTreatment = AcupunctureProject.Database.Treatment;

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        private static string Folder { get; set; }
        private Window Perent { get; set; }

        public delegate void EventHandler();

        private static List<DPoint> _AllPoints;
        public static List<DPoint> AllPoints
        {
            get => _AllPoints;
            set
            {
                _AllPoints = value;
                UpdatePoints?.Invoke();
            }
        }
        public static event EventHandler UpdatePoints;

        private static List<DTreatment> _AllTreatments;
        public static List<DTreatment> AllTreatments
        {
            get => _AllTreatments;
            set
            {
                _AllTreatments = value;
                UpdateTreatments?.Invoke();
            }
        }
        public static event EventHandler UpdateTreatments;

        public Main(Window perent)
        {
            InitializeComponent();
            AllPoints = DatabaseConnection.Instance.GetAllPoints();
            if (AllPoints[0].Image.EndsWith(".png"))
            {
                foreach (var point in AllPoints)
                {
                    point.Image = point.Image.Replace(".png", ".jpg");
                    DatabaseConnection.Instance.Update(point);
                }
            }
            AllTreatments = DatabaseConnection.Instance.GetAllTreatments();
            DatabaseConnection.Instance.TableChangedEvent += new DatabaseConnection.TableChanged(
                (t, i) =>
                {
                    if (t == typeof(DPoint))
                        AllPoints = DatabaseConnection.Instance.GetAllPoints();
                    else if (t == typeof(DTreatment))
                        AllTreatments = DatabaseConnection.Instance.GetAllTreatments();
                });
            Perent = perent;
            Folder = System.Reflection.Assembly.GetEntryAssembly().Location;
            Folder = Folder.Remove(Folder.LastIndexOf('\\') + 1);
            try { BackImage.Source = new BitmapImage(new Uri(Folder + "images\\backpic.jpg")); } catch (Exception) { }
        }

        private void NewPatientMI_Click(object sender, RoutedEventArgs e)
        {
            new NewPatient().Show();
        }

        private void NewMeetingMI_Click(object sender, RoutedEventArgs e)
        {
            new NewMeetingWindow().Show();
        }

        private void PatientListMI_Click(object sender, RoutedEventArgs e) => new PatientList().Show();

        private void PointsListMI_Click(object sender, RoutedEventArgs e) => new PointInfo(AllPoints[new Random().Next(0, AllPoints.Count - 1)]).Show();

        private void SettingMI_Click(object sender, RoutedEventArgs e)
        {
            //new SettingWindow().Show();
        }

        private void Updates_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(Folder + "UpdateApp.exe", Folder)
                {
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    Verb = "runas"
                });
                Application.Current.Shutdown();
            }
            catch (Win32Exception)
            {
                MessageBox.Show("לא יכול לגשת לקבצים", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading);
            }
        }

        private void NewTreatment_Click(object sender, RoutedEventArgs e)
        {
            new NewTreatment().Show();
        }

        private void TreatmentList_Click(object sender, RoutedEventArgs e) => new TreatmentList().Show();
    }
}
