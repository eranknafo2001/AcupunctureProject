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
using AcupunctureProject.GUI.Exceptions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AcupunctureProject.GUI
{
	/// <summary>
	/// Interaction logic for NewPatient.xaml
	/// </summary>
	public partial class NewPatient : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private void PropertyChangedEvent([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		private Patient _PatientItem;
		public Patient PatientItem
		{
			get => _PatientItem;
			set
			{
				if (value != _PatientItem)
				{
					_PatientItem = value;
					PropertyChangedEvent();
				}
			}
		}

		public NewPatient()
		{
			DataContext = this;
			InitializeComponent();
			PatientItem = new Patient();
			gender.Items.Add(new ComboBoxItem() { Content = Gender.MALE.MyToString(), DataContext = Gender.MALE });
			gender.Items.Add(new ComboBoxItem() { Content = Gender.FEMALE.MyToString(), DataContext = Gender.FEMALE });
			gender.Items.Add(new ComboBoxItem() { Content = Gender.OTHER.MyToString(), DataContext = Gender.OTHER });
			gender.SelectedIndex = 0;
		}

		private void SaveData()
		{
			if (PatientItem.Name == null || PatientItem.Name == "")
			{
				MessageBox.Show(this, "חייב שם", "בעיה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
			}
			else if ((PatientItem.Cellphone == null || PatientItem.Cellphone == "") && (PatientItem.Telephone == null || PatientItem.Telephone == ""))
			{
				MessageBox.Show(this, "חייב טלפון או פלפון", "בעיה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
			}
			else
			{
				try
				{
					DatabaseConnection.Insert(PatientItem);
				}
				catch (Exception e)
				{
					MessageBox.Show(this, "המטופל קיים", "אזרה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
					throw e;
				}
				return;
			}
			throw new NullValueException();
		}

		private void Censel_Click(object sender, RoutedEventArgs e) => Close();

		private void SaveAndExit_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SaveData();
				Close();
			}
			catch (Exception) { }
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SaveData();
				ClearAll();
			}
			catch (Exception) { }
		}

		private void ClearAll()
		{
			PatientItem.Name = "";
			//PatientItem.Birthday = null;
			PatientItem.Address = "";
			PatientItem.Cellphone = "";
			PatientItem.Telephone = "";
			PatientItem.Email = "";
			gender.SelectedIndex = 0;
			PatientItem.MedicalDescription = "";
		}
	}
}