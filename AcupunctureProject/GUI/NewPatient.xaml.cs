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
using AcupunctureProject.GUI.Exceptions;

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

        private bool SaveData()
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
                throw new NullValueException();
            }

            try
            {
                DatabaseConnection.Instance.InsertPatient(new Patient(name.Text, telphone.Text, cellphone.Text, (DateTime)berthday.SelectedDate, Patient.Gender.FromValue(gender.SelectedIndex), address.Text, email.Text, hestory.Text));
            }
            catch (Database.Exceptions.UniqueNameException e)
            {
                MessageBox.Show(this, "המטופל קיים", "אזרה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                throw e;
            }
            return true;
        }

        private void Censel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveData();
                    Close();
            }
            catch (Database.Exceptions.UniqueNameException) { }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveData();
                    ClearAll();
            }
            catch (Database.Exceptions.UniqueNameException) { }
        }

        private void ClearAll()
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

        private void Hestory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddNewLine(hestory);
        }

        private void AddNewLine(TextBox textBox)
        {
            int temp = textBox.SelectionStart;
            textBox.Text = textBox.Text.Remove(temp, textBox.SelectionLength);
            textBox.Text = textBox.Text.Insert(temp, "\n");
            textBox.SelectionStart = temp + 1;
        }
    }
}