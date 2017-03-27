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
using DTreatment = AcupunctureProject.Database.Treatment;

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for NewTreatment.xaml
    /// </summary>
    public partial class NewTreatment : Window
    {
        public DTreatment TreatmentItem { get; set; }

        public NewTreatment(DTreatment treatment = null)
        {
            if (treatment?.Name == null && treatment?.Path == null)
                treatment = new DTreatment() { Name = "", Path = "" };
            TreatmentItem = treatment;
            InitializeComponent();
        }

        private void SaveData()
        {
            TreatmentItem.Name = Name.Text;
            TreatmentItem.Path = Path.Text;
            DatabaseConnection.Instance.Set(TreatmentItem);
        }

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
                Path.Text = FileDialog.FileName.Replace(Folder, "");
            }
        }
    }
}
