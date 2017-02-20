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
    /// Interaction logic for DiagnosticList.xaml
    /// </summary>
    public partial class DiagnosticList : Window
    {
        public DiagnosticList(Patient patient)
        {
            InitializeComponent();
            data.ItemsSource = Database.Instance.GetAllDiagnosticByPatient(patient);
        }

        private void data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void data_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {

        }

        relaode
    }
}
