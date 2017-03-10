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
    /// Interaction logic for Diagnostic.xaml
    /// </summary>
    public partial class DiagnosticList : Window
    {
        private Patient patient;

        public DiagnosticList(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            var list = Database.Instance.GetAllDiagnosticByPatient(patient);
            Data.Items.Clear();
            Data.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DiagnosticInfo(patient).Show();
        }

        private void Data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender.GetType() != typeof(DataGrid))
                return;
            Diagnostic item = (Diagnostic)((DataGrid)sender).SelectedItem;
            new DiagnosticInfo(item).Show();
        }

        //private void Delete_Click(object sender, RoutedEventArgs e)
        //{
        //}
    }
}
