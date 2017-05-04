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
			patientDataGrid.ItemsSource = DatabaseConnection.FindPatient(searchTextBox.Text);
			DatabaseConnection.TableChangedEvent += new TableChangedEventHendler(UpdateData);
		}

		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			patientDataGrid.ItemsSource = DatabaseConnection.FindPatient(searchTextBox.Text);
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
			DatabaseConnection.GetChildren(patient);
			foreach (var meeting in patient.Meetings)
				DatabaseConnection.Delete(meeting);
			DatabaseConnection.Delete(patient);
			patientDataGrid.ItemsSource = DatabaseConnection.FindPatient(searchTextBox.Text);
		}

		~PatientList() => DatabaseConnection.TableChangedEvent -= new TableChangedEventHendler(UpdateData);

		private void UpdateData(Type t, object i)
		{
			if (t == typeof(Patient))
				patientDataGrid.ItemsSource = DatabaseConnection.FindPatient(searchTextBox.Text);
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
