using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AcupunctureProject.GUI
{
	/// <summary>
	/// Interaction logic for Diagnostic.xaml
	/// </summary>
	public partial class DiagnosticList : Window, INotifyPropertyChanged
	{
		private Patient patient;

		private Diagnostic _SelectedItem;
		public Diagnostic SelectedItem
		{
			get => _SelectedItem;
			set
			{
				if(value!=_SelectedItem)
				{
					_SelectedItem = value;
					CallPropertyChanged();
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void CallPropertyChanged([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		public DiagnosticList(Patient patient)
		{
			DataContext = this;
			InitializeComponent();
			DatabaseConnection.GetChildren(patient);
			this.patient = patient;
			Data.ItemsSource = patient.Diagnostics;
			DatabaseConnection.TableChangedEvent += TableChanged;
		}

		~DiagnosticList() => DatabaseConnection.TableChangedEvent -= TableChanged;

		private void TableChanged(Type t, object I)
		{
			if (t != typeof(Diagnostic))
				return;
			DatabaseConnection.GetChildren(patient);
			Data.ItemsSource = patient.Diagnostics;
		}

		private void Button_Click(object sender, RoutedEventArgs e) =>
			new DiagnosticInfo(patient).Show();

		private void Data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender.GetType() != typeof(DataGrid))
				return;
			Diagnostic item = (Diagnostic)((DataGrid)sender).SelectedItem;
			if (item != null)
				new DiagnosticInfo(item).Show();
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			if (SelectedItem == null)
				return;
			DatabaseConnection.Delete(SelectedItem);
		}
	}
}
