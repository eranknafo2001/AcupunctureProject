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
    public partial class PatientInfo : Window
    {

        private class GenderItems
        {
            public static ComboBoxItem MALE = new ComboBoxItem() { Content = Patient.Gender.MALE };
            public static ComboBoxItem FEMALE = new ComboBoxItem() { Content = Patient.Gender.FEMALE };
            public static ComboBoxItem OTHER = new ComboBoxItem() { Content = Patient.Gender.OTHER };
        }

        private Patient patient;

        public PatientInfo(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            name.Text = patient.Name;
            berthday.SelectedDate = patient.Birthday;
            address.Text = patient.Address;
            cellphone.Text = patient.Cellphone;
            telphone.Text = patient.Telephone;
            email.Text = patient.Email;
            gender.Items.Add(GenderItems.MALE);
            gender.Items.Add(GenderItems.FEMALE);
            gender.Items.Add(GenderItems.OTHER);
            gender.SelectedIndex = patient.Gend.Value;
            hestory.Text = patient.MedicalDescription;
        }

        private void saveData()
        {
            patient.Name = name.Text;
            patient.Birthday = (DateTime)berthday.SelectedDate;
            patient.Address = address.Text;
            patient.Cellphone = cellphone.Text;
            patient.Telephone = telphone.Text;
            patient.MedicalDescription = hestory.Text;
            ComboBoxItem gendItem = (ComboBoxItem)gender.SelectedItem;
            patient.Gend = (Patient.Gender)gendItem.DataContext;
            patient.Email = email.Text;
            Database.Instance.updatePatient(patient);
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
        }

        private void openMeetings_Click(object sender, RoutedEventArgs e)
        {
            MeetingListByPatient m = new MeetingListByPatient(patient);
            m.Show();
        }
    }
}
