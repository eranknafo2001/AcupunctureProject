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
    /// Interaction logic for NewPatient.xaml
    /// </summary>
    public partial class NewPatient : Window
    {
        public NewPatient()
        {
            InitializeComponent();
            gender.Items.Add(new ComboBoxItem() { Content = Patient.Gender.MALE.ToString(), DataContext = Patient.Gender.MALE });
            gender.Items.Add(new ComboBoxItem() { Content = Patient.Gender.FEMALE.ToString(), DataContext = Patient.Gender.FEMALE });
            gender.Items.Add(new ComboBoxItem() { Content = Patient.Gender.OTHER.ToString(), DataContext = Patient.Gender.OTHER });
        }

        private void saveData()
        {
            Database.Instance.insertPatient(new Patient(name.Text, telphone.Text, cellphone.Text, (DateTime)berthday.SelectedDate, Patient.Gender.FromValue(gender.SelectedIndex), address.Text, email.Text, hestory.Text));
        }

        private void censel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void saveAndExit_Click(object sender, RoutedEventArgs e)
        {
            saveData();
            Close();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            saveData();
            clearAll();
        }

        private void clearAll()
        {
            name.Clear();
            berthday.SelectedDate = null;
            address.Clear();
            cellphone.Clear();
            telphone.Clear();
            email.Clear();
            gender.SelectedIndex = -1;
            hestory.Clear();
        }
    }
}
