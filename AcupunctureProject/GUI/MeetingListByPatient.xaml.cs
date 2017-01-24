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
using AcupunctureProject.database;

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for MeetingListByPatient.xaml
    /// </summary>
    public partial class MeetingListByPatient : Window
    {
        public MeetingListByPatient(Patient patient)
        {
            InitializeComponent();
            Title += patient.Name;
            meetingsDataGrid.ItemsSource = Database.Instance.GetAllMeetingsRelativeToPatient(patient);
        }

        private void MeetingsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Meeting item= (Meeting) meetingsDataGrid.SelectedItem;
            if (item == null)
                return;
            new MeetingInfoWindow(item).Show();
        }
    }
}
