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
using AcupunctureProject.Database;

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for PatientList.xaml
    /// </summary>
    public partial class PatientList : Window
    {
        public PatientList()
        {
            InitializeComponent();
            patientDataGrid.ItemsSource = DatabaseConnection.Instance.FindPatient(searchTextBox.Text);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            patientDataGrid.ItemsSource = DatabaseConnection.Instance.FindPatient(searchTextBox.Text);
        }

        private void PatientDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Patient p = (Patient)patientDataGrid.SelectedItem;
            if (p == null)
                return;
            new PatientInfo(p, this).Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = (Patient)patientDataGrid.SelectedItem;
            if (patient == null)
                return;
            List<Meeting> meetings = DatabaseConnection.Instance.GetAllMeetingsRelativeToPatientOrderByDate(patient);
            for (int i = 0; i < meetings.Count; i++)
            {
                List<Database.Point> points = DatabaseConnection.Instance.GetAllPointsRelativeToMeeting(meetings[i]);
                for (int j = 0; j < points.Count; j++)
                    DatabaseConnection.Instance.DeleteMeetingPoint(meetings[i], points[j]);
                List<Symptom> symptoms = DatabaseConnection.Instance.GetAllSymptomRelativeToMeeting(meetings[i]);
                for (int j = 0; j < symptoms.Count; j++)
                    DatabaseConnection.Instance.DeleteSymptomMeetingRelation(symptoms[j], meetings[i]);
                DatabaseConnection.Instance.DeleteMeeting(meetings[i]);
            }
            DatabaseConnection.Instance.DeletePatient(patient);
            patientDataGrid.ItemsSource = DatabaseConnection.Instance.FindPatient(searchTextBox.Text);
        }

        public void UpdateData()
        {
            patientDataGrid.ItemsSource = DatabaseConnection.Instance.FindPatient(searchTextBox.Text);
        }

        private void openMeetingsList_Click(object sender, RoutedEventArgs e)
        {
            Patient p = (Patient)patientDataGrid.SelectedItem;
            if (p == null)
                return;
            new MeetingListByPatient(p).Show();
        }
    }
}
