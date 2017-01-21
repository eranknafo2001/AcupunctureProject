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
    /// Interaction logic for PatientList.xaml
    /// </summary>
    public partial class PatientList : Window
    {
        public PatientList()
        {
            InitializeComponent();
            patientDataGrid.ItemsSource = Database.Instance.findPatient(searchTextBox.Text);
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            patientDataGrid.ItemsSource = Database.Instance.findPatient(searchTextBox.Text);
        }

        private void patientDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Patient p = (Patient)patientDataGrid.SelectedItem;
            PatientInfo pwin = new PatientInfo(p);
            pwin.Show();
        }
    }
}
