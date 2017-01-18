using AcupunctureProject.database;
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

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for newMeeting.xaml
    /// </summary>
    public partial class NewMeeting : Page
    {
        private Window perent;
        private object selectedPatient;

        public NewMeeting(Window perent)
        {
            InitializeComponent();
            this.perent = perent;
        }

        private void symptomSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            symptomSearchList.Items.Clear();
            symptomSearchList.Items.Add(new ListBoxItem() { Content = "te1st" });
        }

        private void censel_Click(object sender, RoutedEventArgs e)
        {
            perent.Content = new Main(perent);
        }

        private void patientSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            reloadPatientList();
        }

        private void reloadPatientList()
        {
            patientSearchList.Items.Clear();
            List<Patient> p = Database.Instance.findPatient(patientSearchTextBox.Text);
            for (int i = 0; i < p.Count; i++)
                patientSearchList.Items.Add(new ListViewItem() { Content = p[i].ToString(), DataContext = p[i] });
            patientSearchList.SelectedIndex = 0;
        }

        private void patientSearchTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            patientSearchList.Visibility = Visibility.Hidden;
            patientSearchList.Height = 0;
        }

        private void patientSearchTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            patientSearchList.Height = 100;
            patientSearchList.Margin = new Thickness(symptomSearch.Margin.Left, symptomSearch.Margin.Top + symptomSearch.ExtentHeight, 0, 0);
            patientSearchList.Visibility = Visibility.Visible;
            reloadPatientList();
        }

        private void symptomSearch_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            symptomSearchList.Height = 100;
            symptomSearchList.Margin = new Thickness(symptomSearch.Margin.Left, symptomSearch.Margin.Top + symptomSearch.ExtentHeight, 0, 0);
            symptomSearchList.Visibility = Visibility.Visible;
        }

        private void symptomSearch_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            symptomSearchList.Height = 0;
            symptomSearchList.Visibility = Visibility.Hidden;
        }

        private void patientSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectPatient();
        }

        private void selectPatient()
        {
            //ListViewItem item = (ListViewItem)patientSearchList.SelectedItem;
            //selectedPatient = item.Content;
            patientSearchTextBox.IsEnabled = false;
            openPatientButton.IsEnabled = true;
            date.IsEnabled = true;
            symptomSearch.IsEnabled = true;
            symptomTreeView.IsEnabled = true;
            symptomTreeDelete.IsEnabled = true;
            pointsThatUsed.IsEnabled = true;
            notes.IsEnabled = true;
            summeryTextBox.IsEnabled = true;
            resoltSummeryTextBox.IsEnabled = true;
            resolt.IsEnabled = true;
            save.IsEnabled = true;
            saveAndExit.IsEnabled = true;
        }

        private void openPatientButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Down))
            {
                if (patientSearchList.SelectedIndex != patientSearchList.Items.Count - 1)
                    patientSearchList.SelectedIndex++;
            }
            else if (e.Key.Equals(Key.Up))
            {
                if (patientSearchList.SelectedIndex != 0)
                    patientSearchList.SelectedIndex--;
            }
            else if (e.Key.Equals(Key.Enter))
            {
                selectPatient();
            }
        }
    }
}
