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
    }
}
