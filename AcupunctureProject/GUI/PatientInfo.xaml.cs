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
	/// Interaction logic for NewPatient.xaml
	/// </summary>
	public partial class PatientInfo : Window
	{
		private Patient patient;
		private PatientList perent;

		public PatientInfo(Patient patient, PatientList perent = null)
		{
			InitializeComponent();
			this.patient = patient;
			this.perent = perent;
			Title += patient.Name;
			name.Text = patient.Name;
			berthday.SelectedDate = patient.Birthday;
			address.Text = patient.Address;
			cellphone.Text = patient.Cellphone;
			telphone.Text = patient.Telephone;
			email.Text = patient.Email;
			gender.Items.Clear();
			gender.Items.Add(new ComboBoxItem() { Content = Gender.MALE.MyToString(), DataContext = Gender.MALE });
			gender.Items.Add(new ComboBoxItem() { Content = Gender.FEMALE.MyToString(), DataContext = Gender.FEMALE });
			gender.Items.Add(new ComboBoxItem() { Content = Gender.OTHER.MyToString(), DataContext = Gender.OTHER });
			gender.SelectedIndex = (int)patient.Gend;
			hestory.Text = patient.MedicalDescription;
		}

		private void SaveData()
		{
			patient.Name = name.Text;
			patient.Birthday = (DateTime)berthday.SelectedDate;
			patient.Address = address.Text;
			patient.Cellphone = cellphone.Text;
			patient.Telephone = telphone.Text;
			patient.MedicalDescription = hestory.Text;
			patient.Gend = (Gender)gender.DataContext;
			patient.Email = email.Text;
			DatabaseConnection.Instance.Update(patient);
		}

		private void Censel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void SaveAndExit_Click(object sender, RoutedEventArgs e)
		{
			SaveData();
			Close();
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			SaveData();
		}

		private void OpenMeetings_Click(object sender, RoutedEventArgs e)
		{
			MeetingListByPatient m = new MeetingListByPatient(patient);
			m.Show();
		}

		private void Hestory_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				AddNewLine(hestory);
		}

		private void AddNewLine(TextBox textBox)
		{
			int temp = textBox.SelectionStart;
			textBox.Text = textBox.Text.Remove(temp, textBox.SelectionLength);
			textBox.Text = textBox.Text.Insert(temp, "\n");
			textBox.SelectionStart = temp + 1;
		}

		private void openDiagnostic_Click(object sender, RoutedEventArgs e)
		{
			new DiagnosticList(patient).Show();
		}
	}
}
