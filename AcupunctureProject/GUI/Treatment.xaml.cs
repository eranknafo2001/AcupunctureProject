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
using DTreatment = AcupunctureProject.Database.Treatment;

using AcupunctureProject.Database;


namespace AcupunctureProject.GUI
{
	/// <summary>
	/// Interaction logic for Treatment.xaml
	/// </summary>
	public partial class Treatment : Window
	{
		public DTreatment TreatmentItem { get; set; }
		public Treatment(DTreatment treatment)
		{
			TreatmentItem = treatment;
			InitializeComponent();
			Title = treatment.Name;
			Excel.Path = TreatmentItem.Path;
		}
	}
}
