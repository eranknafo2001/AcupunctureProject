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
            //name.BorderBrush = Brushes.Gray;
            //address.BorderBrush = Brushes.Gray;
            //email.BorderBrush = Brushes.Gray;
            //hestory.BorderBrush = Brushes.Gray;
            //cellphone.BorderBrush = Brushes.Gray;
            //telphone.BorderBrush = Brushes.Gray;
            //berthday.BorderBrush = Brushes.Gray;
            //gender.BorderBrush = Brushes.Gray;
        }

        private bool saveData()
        {
            bool secses = true;

            if (name.Text == null || name.Text == "")
            {
                MessageBox.Show(this, "חייב שם", "בעיה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                secses = false;
            }

            if (gender.SelectedIndex == -1)
            {
                MessageBox.Show(this, "חייב מין", "בעיה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                secses = false;
            }

            if ((cellphone.Text == null || cellphone.Text == "") && (telphone.Text == null || telphone.Text == ""))
            {
                MessageBox.Show(this, "חייב טלפון או פלפון", "בעיה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                secses = false;
            }
            
            if (!secses)
            {
                return false;
            }

            try
            {
                Database.Instance.insertPatient(new Patient(name.Text, telphone.Text, cellphone.Text, (DateTime)berthday.SelectedDate, Patient.Gender.FromValue(gender.SelectedIndex), address.Text, email.Text, hestory.Text));
            }
            catch (database.Exceptions.UniqueNameException e)
            {
                MessageBox.Show(this, "המטופל קיים", "אזרה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                throw e;
            }
            return true;
        }

        private void censel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void saveAndExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (saveData())
                    Close();
            }
            catch (database.Exceptions.UniqueNameException) { }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (saveData())
                    clearAll();
            }
            catch (database.Exceptions.UniqueNameException) { }
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