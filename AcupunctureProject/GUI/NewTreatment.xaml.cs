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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AcupunctureProject.Database;
using DTreatment = AcupunctureProject.Database.Treatment;

namespace AcupunctureProject.GUI
{
	/// <summary>
	/// Interaction logic for NewTreatment.xaml
	/// </summary>
	public partial class NewTreatment : Window, INotifyPropertyChanged
	{
		DTreatment _TreatmentItem;
		public DTreatment TreatmentItem
		{
			get => _TreatmentItem;
			set
			{
				if (_TreatmentItem != value)
				{
					_TreatmentItem = value;
					PropertyChangedEvent();
				}
			}
		}

		public NewTreatment(DTreatment treatment = null)
		{
			DataContext = this;
			if (treatment?.Name == null && treatment?.Path == null)
				TreatmentItem = new DTreatment()
				{
					Name = "",
					Path = ""
				};
			else
			{
				TreatmentItem = treatment;
			}
			InitializeComponent();
		}

		private void SaveData() => DatabaseConnection.Instance.Set(TreatmentItem);


		private void Censel_Click(object sender, RoutedEventArgs e) => Close();

		private void Save_Click(object sender, RoutedEventArgs e) => SaveData();

		private void SaveAndClose_Click(object sender, RoutedEventArgs e)
		{
			SaveData();
			Close();
		}

		private void SelectFile_Click(object sender, RoutedEventArgs e)
		{
			var FileDialog = new Microsoft.Win32.OpenFileDialog()
			{
				DefaultExt = ".xlsx",
				Filter = "Excel Files (*.xlsx)|*.xlsx"
			};
			if (FileDialog.ShowDialog() == true)
			{
				var Folder = System.Reflection.Assembly.GetEntryAssembly().Location;
				Folder = Folder.Remove(Folder.LastIndexOf('\\') + 1);
				if (TreatmentItem.Name == null || TreatmentItem.Name == "")
				{
					var file = FileDialog.FileName;
					file = file.Remove(0, file.LastIndexOf('\\')+1);
					file = file.Remove(file.LastIndexOf('.'));
					TreatmentItem.Name = file;
				}
				TreatmentItem.Path = FileDialog.FileName.Replace(Folder, "");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void PropertyChangedEvent([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
