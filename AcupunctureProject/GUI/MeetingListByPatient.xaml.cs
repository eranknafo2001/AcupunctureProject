﻿using System;
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
    /// Interaction logic for MeetingListByPatient.xaml
    /// </summary>
    public partial class MeetingListByPatient : Window
    {
        private Patient patient;

        public MeetingListByPatient(Patient patient)
        {
            InitializeComponent();
            Title += patient.Name;
            this.patient = patient;
            meetingsDataGrid.ItemsSource = DatabaseConnection.Instance.GetAllMeetingsRelativeToPatientOrderByDate(patient);
        }

        private void MeetingsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Meeting item = (Meeting)meetingsDataGrid.SelectedItem;
            if (item == null)
                return;
            new MeetingInfoWindow(item,this).Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Meeting item = (Meeting)meetingsDataGrid.SelectedItem;
            if (item == null)
                return;
            List<Database.Point> points = DatabaseConnection.Instance.GetAllPointsRelativeToMeeting(item);
            for (int i = 0; i < points.Count; i++)
                DatabaseConnection.Instance.DeleteMeetingPoint(item, points[i]);
            List<Symptom> symptoms = DatabaseConnection.Instance.GetAllSymptomRelativeToMeeting(item);
            for (int i = 0; i < symptoms.Count; i++)
                DatabaseConnection.Instance.DeleteSymptomMeetingRelation(symptoms[i], item);
            DatabaseConnection.Instance.DeleteMeeting(item);
            meetingsDataGrid.ItemsSource = DatabaseConnection.Instance.GetAllMeetingsRelativeToPatientOrderByDate(patient);
        }

        public void UpdateData()
        {
            meetingsDataGrid.ItemsSource = DatabaseConnection.Instance.GetAllMeetingsRelativeToPatientOrderByDate(patient);
        }
    }
}
