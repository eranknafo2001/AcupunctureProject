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
using DTreatment = AcupunctureProject.Database.Treatment;

namespace AcupunctureProject.GUI
{
	/// <summary>
	/// Interaction logic for TreatmentList.xaml
	/// </summary>
	public partial class TreatmentList : Window, INotifyPropertyChanged
	{
		private List<DTreatment> _AllTreatments;
		public List<DTreatment> AllTreatments
		{
			get => _AllTreatments;
			set
			{
				if (value != _AllTreatments)
				{
					_AllTreatments = value;
					PropChanged();
				}
			}
		}

		public TreatmentList()
		{
			DataContext = this;
			InitializeComponent();
			AllTreatments = Main.AllTreatments;
			Main.UpdateTreatments += new Main.EventHandler(Update);
		}

		~TreatmentList() => Main.UpdateTreatments -= new Main.EventHandler(Update);

		private void Update()
		{
			AllTreatments = Main.AllTreatments;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void PropChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		private void OpenShowing_Click(object sender, RoutedEventArgs e)
		{
			if (dataGrid.SelectedItem != null)
				new Treatment(dataGrid.SelectedItem as DTreatment).Show();
		}

		private void OpenEditing_Click(object sender, RoutedEventArgs e)
		{
			if (dataGrid.SelectedItem != null)
				new NewTreatment(dataGrid.SelectedItem as DTreatment).Show();
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			if (dataGrid.SelectedItem != null)
				Database.DatabaseConnection.Instance.Delete(dataGrid.SelectedItem as DTreatment);
		}
	}
}
