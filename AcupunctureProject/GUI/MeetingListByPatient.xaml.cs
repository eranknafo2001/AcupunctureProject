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
	/// Interaction logic for MeetingListByPatient.xaml
	/// </summary>
	public partial class MeetingListByPatient : Window
	{
		private Patient patient;

		public MeetingListByPatient(Patient patient)
		{
			InitializeComponent();
			DatabaseConnection.GetChildren(patient);
			Title += patient.Name;
			this.patient = patient;
			meetingsDataGrid.ItemsSource = patient.Meetings;
			DatabaseConnection.TableChangedEvent += UpdateData;
		}

		~MeetingListByPatient() => DatabaseConnection.TableChangedEvent += UpdateData;

		private void MeetingsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Meeting item = (Meeting)meetingsDataGrid.SelectedItem;
			if (item == null)
				return;
			new MeetingInfoWindow(item).Show();
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			Meeting item = (Meeting)meetingsDataGrid.SelectedItem;
			if (item == null)
				return;
			DatabaseConnection.Delete(item);
			meetingsDataGrid.ItemsSource = patient.Meetings.OrderBy(m => m.Date).Reverse();
		}

		private void UpdateData(Type t, object i)
		{
			if (t != typeof(Meeting))
				return;
			DatabaseConnection.GetChildren(patient);
			meetingsDataGrid.ItemsSource = patient.Meetings.OrderBy(m => m.Date).Reverse();
		}

		bool work = true;
		private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			if (work)
			{
				if (sender is TextBox send)
				{
					work = false;
					send.Select(0, 0);
					work = true;
				}
			}
		}
	}
}
