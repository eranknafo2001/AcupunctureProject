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
using Xceed.Wpf.Toolkit;
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
        private Patient selectedPatient;

        public NewMeeting(Window perent)
        {
            InitializeComponent();
            this.perent = perent;
            date.SelectedDate = DateTime.Today;
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.BETTER.ToString(), DataContext = Meeting.ResultValue.BETTER });
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.WORSE.ToString(), DataContext = Meeting.ResultValue.WORSE });
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.NO_CHANGE.ToString(), DataContext = Meeting.ResultValue.NO_CHANGE });
            level0.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(0) };
            level1.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(1) };
            level2.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(2) };
            level3.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(3) };
            level4.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(4) };
            level5.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(5) };
        }

        private void symptomSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            relaodSymptomList();
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
                patientSearchList.Items.Add(new ListViewItem() { Content = p[i].ToStringInSearch(), DataContext = p[i] });
            patientSearchList.SelectedIndex = 0;
        }

        private void patientSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectPatient();
        }

        private void selectPatient()
        {
            ListViewItem item = (ListViewItem)patientSearchList.SelectedItem;
            selectedPatient = (Patient)item.DataContext;
            patientSearchTextBox.Text = selectedPatient.Name;
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
            setPatientListVisibility(false);
        }

        private void patientSearchList_GotFocus(object sender, RoutedEventArgs e)
        {
            setPatientListVisibility(true);
        }

        private void relaodSymptomList()
        {
            symptomSearchList.Items.Clear();
            List<Symptom> items = Database.Instance.findSymptom(symptomSearch.Text);
            for (int i = 0; i < items.Count; i++)
                symptomSearchList.Items.Add(new ListViewItem() { Content = items[i].ToString(), DataContext = items[i] });
        }

        private void setSymptomListVisibility(bool val)
        {
            if (val)
            {
                symptomSearchList.Margin = new Thickness(symptomSearch.Margin.Left, symptomSearch.Margin.Top + symptomSearch.ExtentHeight, 0, 0);
                symptomSearchList.Visibility = Visibility.Visible;
                symptomSearchList.MaxHeight = 100;
            }
            else
            {
                symptomSearchList.Visibility = Visibility.Hidden;
                symptomSearchList.MaxHeight = 0;
            }
        }

        private void setPatientListVisibility(bool val)
        {
            if (val)
            {
                patientSearchList.Margin = new Thickness(symptomSearch.Margin.Left, symptomSearch.Margin.Top + symptomSearch.ExtentHeight, 0, 0);
                patientSearchList.Visibility = Visibility.Visible;
                patientSearchList.MaxHeight = 100;
            }
            else
            {
                patientSearchList.Visibility = Visibility.Hidden;
                patientSearchList.MaxHeight = 0;
            }
        }

        private void patientSearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Down))
            {
                if (patientSearchList.SelectedIndex == patientSearchList.Items.Count - 1)
                    patientSearchList.SelectedIndex = 0;
                else
                    patientSearchList.SelectedIndex = patientSearchList.SelectedIndex + 1;
            }
            else if (e.Key.Equals(Key.Up))
            {
                if (patientSearchList.SelectedIndex == 0)
                    patientSearchList.SelectedIndex = patientSearchList.Items.Count - 1;
                else
                    patientSearchList.SelectedIndex = patientSearchList.SelectedIndex - 1;
            }
            else if (e.Key.Equals(Key.Enter))
            {
                selectPatient();
            }
        }

        private void patientSearchList_LostFocus(object sender, RoutedEventArgs e)
        {
            setPatientListVisibility(false);
        }

        private void patientSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                selectPatient();
        }

        private void selectSymptom()
        {
            ListViewItem item = (ListViewItem)symptomSearchList.SelectedItem;
            addItemToSymptomTree((Symptom)item.DataContext);
            symptomSearch.Clear();
            relaodSymptomList();
        }

        private void addItemToSymptomTree(Symptom symptom)
        {
            TreeViewItem sym = new TreeViewItem() { Header = symptom.ToString(), DataContext = symptom };
            List<ConnectionValue<database.Point>> points = Database.Instance.getAllPointRelativeToSymptom(symptom);
            for (int i = 0; i < points.Count; i++)
            {
                sym.Items.Add(new TreeViewItem()
                {
                    Header = points[i].ToString(),
                    DataContext = points[i],
                    Foreground = new SolidColorBrush()
                    {
                        Color = Database.Instance.GetLevel(points[i].Importance)
                    }
                });
            }
            List<ConnectionValue<Channel>> channels = Database.Instance.getAllChannelRelativeToSymptom(symptom);
            for (int i = 0; i < channels.Count; i++)
            {
                sym.Items.Add(new TreeViewItem()
                {
                    Header = channels[i].ToString(),
                    DataContext = channels[i],
                    Foreground = new SolidColorBrush()
                    {
                        Color = Database.Instance.GetLevel(channels[i].Importance)
                    }
                });
            }
            symptomTreeView.Items.Add(sym);
        }

        private void symptomSearchList_LostFocus(object sender, RoutedEventArgs e)
        {
            setSymptomListVisibility(false);
        }

        private void symptomSearchList_GotFocus(object sender, RoutedEventArgs e)
        {
            setSymptomListVisibility(true);
        }

        private void symptomSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            setSymptomListVisibility(false);
        }

        private void symptomSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            setSymptomListVisibility(true);
            relaodSymptomList();
        }

        private void patientSearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            setPatientListVisibility(true);
            reloadPatientList();
        }

        private void patientSearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            setPatientListVisibility(false);
        }

        private void symptomSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                selectSymptom();
        }

        private void symptomSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                selectSymptom();
        }

        private void symptomSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectSymptom();
        }

        private void symptomTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)symptomTreeView.SelectedItem;
            if (item.DataContext.GetType() == typeof(ConnectionValue<database.Point>))
            {
                ConnectionValue<database.Point> con = (ConnectionValue<database.Point>)item.DataContext;
                PointInfo p = new PointInfo(con.Value);
                p.Show();
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            saveData();
        }

        private void saveData()
        {
            ComboBoxItem resoltitem = (ComboBoxItem)resolt.SelectedItem;
            Database.Instance.insertMeeting(new Meeting(selectedPatient.Id, "", (DateTime)date.SelectedDate, notes.Text, summeryTextBox.Text, resoltSummeryTextBox.Text, (Meeting.ResultValue)resoltitem.DataContext));
        }

        private void saveAndExit_Click(object sender, RoutedEventArgs e)
        {
            saveData();
            perent.Content = new Main(perent);
        }

        private void openPatientButton_Click(object sender, RoutedEventArgs e)
        {
            PatientInfo p = new PatientInfo(selectedPatient);
            p.Show();
        }
    }
}
