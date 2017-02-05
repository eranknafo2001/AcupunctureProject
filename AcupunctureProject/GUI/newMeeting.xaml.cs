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
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.NOT_SET.ToString(), DataContext = Meeting.ResultValue.NOT_SET });
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.BETTER.ToString(), DataContext = Meeting.ResultValue.BETTER });
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.WORSE.ToString(), DataContext = Meeting.ResultValue.WORSE });
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.NO_CHANGE.ToString(), DataContext = Meeting.ResultValue.NO_CHANGE });
            resolt.SelectedIndex = 0;
            level0.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(0) };
            level1.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(1) };
            level2.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(2) };
            level3.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(3) };
            level4.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(4) };
            level5.Background = new SolidColorBrush() { Color = Database.Instance.GetLevel(5) };
        }

        private void SymptomSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            RelaodSymptomList();
        }

        private void Censel_Click(object sender, RoutedEventArgs e)
        {
            perent.Close();
        }

        private void PatientSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadPatientList();
        }

        private void ReloadPatientList()
        {
            patientSearchList.Items.Clear();
            List<Patient> p = Database.Instance.FindPatient(patientSearchTextBox.Text);
            for (int i = 0; i < p.Count; i++)
                patientSearchList.Items.Add(new ListViewItem() { Content = p[i].ToStringInSearch(), DataContext = p[i] });
            patientSearchList.SelectedIndex = 0;
        }

        private void PatientSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectPatient();
        }

        private void SelectPatient()
        {
            ListViewItem item = (ListViewItem)patientSearchList.SelectedItem;
            if (item == null)
                return;
            selectedPatient = (Patient)item.DataContext;
            patientSearchTextBox.Text = selectedPatient.Name;
            patientSearchTextBox.IsEnabled = false;
            openPatientButton.IsEnabled = true;
            date.IsEnabled = true;
            symptomSearch.IsEnabled = true;
            symptomTreeView.IsEnabled = true;
            symptomTreeDelete.IsEnabled = true;
            pointThatUsedSearch.IsEnabled = true;
            pointThatUsed.IsEnabled = true;
            notes.IsEnabled = true;
            summeryTextBox.IsEnabled = true;
            resoltSummeryTextBox.IsEnabled = true;
            pointThatUsedDelete.IsEnabled = true;
            resolt.IsEnabled = true;
            save.IsEnabled = true;
            saveAndExit.IsEnabled = true;
            SetPatientListVisibility(false);
        }

        private void PatientSearchList_GotFocus(object sender, RoutedEventArgs e)
        {
            SetPatientListVisibility(true);
        }

        private void RelaodSymptomList()
        {
            symptomSearchList.Items.Clear();
            List<Symptom> items = Database.Instance.FindSymptom(symptomSearch.Text);
            for (int i = 0; i < items.Count; i++)
                symptomSearchList.Items.Add(new ListViewItem() { Content = items[i].ToString(), DataContext = items[i] });
        }

        private void SetpointThatUsedSearchListViability(bool val)
        {
            if (val)
            {
                pointThatUsedSearchList.Margin = new Thickness(pointThatUsedSearch.Margin.Left, pointThatUsedSearch.Margin.Top + pointThatUsedSearch.ExtentHeight, 0, 0);
                pointThatUsedSearchList.Visibility = Visibility.Visible;
                pointThatUsedSearchList.MaxHeight = 100;
            }
            else
            {
                pointThatUsedSearchList.Visibility = Visibility.Hidden;
                pointThatUsedSearchList.MaxHeight = 0;
            }
        }

        private void SetSymptomListVisibility(bool val)
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

        private void SetPatientListVisibility(bool val)
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

        private void PatientSearchTextBox_KeyDown(object sender, KeyEventArgs e)
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
                SelectPatient();
            }
        }

        private void PatientSearchList_LostFocus(object sender, RoutedEventArgs e)
        {
            SetPatientListVisibility(false);
        }

        private void PatientSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                SelectPatient();
        }

        private void SelectSymptom()
        {
            ListViewItem item = (ListViewItem)symptomSearchList.SelectedItem;
            if (item == null)
                return;
            for (int i = 0; i < symptomTreeView.Items.Count; i++)
            {
                TreeViewItem tempItem = (TreeViewItem)symptomTreeView.Items[i];
                Symptom temps = (Symptom)tempItem.DataContext;
                Symptom temps2 = (Symptom)item.DataContext;
                if (temps.Id == temps2.Id)
                    return;
            }
            AddItemToSymptomTree((Symptom)item.DataContext);
            RefindConnectedPoint();
            SetSymptomListVisibility(false);
            symptomSearch.Clear();
            RelaodSymptomList();
        }

        private void RefindConnectedPoint()
        {
            Dictionary<Type, Dictionary<int, List<TreeViewItem>>> count = new Dictionary<Type, Dictionary<int, List<TreeViewItem>>>();
            for (int i = 0; i < symptomTreeView.Items.Count; i++)
            {
                TreeViewItem symItem = (TreeViewItem)symptomTreeView.Items[i];
                for (int j = 0; j < symItem.Items.Count; j++)
                {
                    TreeViewItem chItem = (TreeViewItem)symItem.Items[j];
                    if (!count.ContainsKey(chItem.DataContext.GetType()))
                        count.Add(chItem.DataContext.GetType(), new Dictionary<int, List<TreeViewItem>>());
                    if (chItem.DataContext.GetType() == typeof(ConnectionValue<database.Point>))
                    {
                        ConnectionValue<database.Point> point = (ConnectionValue<database.Point>)chItem.DataContext;
                        if (!count[chItem.DataContext.GetType()].ContainsKey(point.Value.Id))
                            count[chItem.DataContext.GetType()].Add(point.Value.Id, new List<TreeViewItem>());
                        count[chItem.DataContext.GetType()][point.Value.Id].Add(chItem);
                    }
                    else if (chItem.DataContext.GetType() == typeof(ConnectionValue<Channel>))
                    {
                        ConnectionValue<Channel> point = (ConnectionValue<Channel>)chItem.DataContext;
                        if (!count[chItem.DataContext.GetType()].ContainsKey(point.Value.Id))
                            count[chItem.DataContext.GetType()].Add(point.Value.Id, new List<TreeViewItem>());
                        count[chItem.DataContext.GetType()][point.Value.Id].Add(chItem);
                    }
                }
            }

            List<Type> typeKeys = count.Keys.ToList();
            Random rand = new Random();
            for (int i = 0; i < typeKeys.Count; i++)
            {
                Type typeKey = typeKeys[i];
                List<int> intKeys = count[typeKey].Keys.ToList();
                for (int j = 0; j < intKeys.Count; j++)
                {
                    int intKey = intKeys[j];
                    if (count[typeKey][intKey].Count > 1)
                    {
                        Color c = new Color() { A = 128, R = (byte)rand.Next(0, 255), G = (byte)rand.Next(0, 255), B = (byte)rand.Next(0, 255) };
                        for (int z = 0; z < count[typeKey][intKey].Count; z++)
                        {
                            count[typeKey][intKey][z].Background = new SolidColorBrush(c);
                        }
                    }
                }
            }
        }

        private void AddItemToSymptomTree(Symptom symptom)
        {
            TreeViewItem sym = new TreeViewItem() { Header = symptom.ToString(), DataContext = symptom, IsExpanded = true };
            List<ConnectionValue<database.Point>> points = Database.Instance.GetAllPointRelativeToSymptom(symptom);
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
            List<ConnectionValue<Channel>> channels = Database.Instance.GetAllChannelRelativeToSymptom(symptom);
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

        private void SymptomSearchList_LostFocus(object sender, RoutedEventArgs e)
        {
            SetSymptomListVisibility(false);
        }

        private void SymptomSearchList_GotFocus(object sender, RoutedEventArgs e)
        {
            SetSymptomListVisibility(true);
        }

        private void SymptomSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            SetSymptomListVisibility(false);
        }

        private void SymptomSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            SetSymptomListVisibility(true);
            RelaodSymptomList();
        }

        private void PatientSearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SetPatientListVisibility(true);
            ReloadPatientList();
        }

        private void PatientSearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetPatientListVisibility(false);
        }

        private void SymptomSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                SelectSymptom();
            else if (e.Key == Key.Escape)
            {
                Focus();
                SetSymptomListVisibility(false);
            }
        }

        private void SymptomSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                SelectSymptom();
            else if (e.Key == Key.Escape)
            {
                Focus();
                SetSymptomListVisibility(false);
            }
        }

        private void SymptomSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectSymptom();
        }

        private void SymptomTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)symptomTreeView.SelectedItem;
            if (item == null)
                return;
            if (item.DataContext.GetType() == typeof(ConnectionValue<database.Point>))
            {
                ConnectionValue<database.Point> con = (ConnectionValue<database.Point>)item.DataContext;
                PointInfo p = new PointInfo(con.Value);
                p.Show();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Meeting meeting = SaveData();
            perent.Content = new MeetingInfoPage(perent, meeting);
        }

        private Meeting SaveData()
        {
            ComboBoxItem resoltitem = (ComboBoxItem)resolt.SelectedItem;
            Meeting meeting = Database.Instance.InsertMeeting(new Meeting(selectedPatient.Id, "", (DateTime)date.SelectedDate, notes.Text, summeryTextBox.Text, resoltSummeryTextBox.Text, (Meeting.ResultValue)resoltitem.DataContext));
            for (int i = 0; i < symptomTreeView.Items.Count; i++)
            {
                TreeViewItem item = (TreeViewItem)symptomTreeView.Items[i];
                Database.Instance.InsertSymptomMeetingRelation((Symptom)item.DataContext, meeting);
            }

            for (int i = 0; i < pointThatUsed.Items.Count; i++)
            {
                ListBoxItem item = (ListBoxItem)pointThatUsed.Items[i];
                Database.Instance.InsertMeetingPointRelation(meeting, (database.Point)item.DataContext);
            }

            return meeting;
        }

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            perent.Close();
        }

        private void OpenPatientButton_Click(object sender, RoutedEventArgs e)
        {
            PatientInfo p = new PatientInfo(selectedPatient);
            p.Show();
        }

        private void ReloadPointThatUsedSearchList()
        {
            pointThatUsedSearchList.SelectedIndex = 0;
            pointThatUsedSearchList.Items.Clear();
            List<database.Point> points = Database.Instance.FindPoint(pointThatUsedSearch.Text);
            for (int i = 0; i < points.Count; i++)
            {
                pointThatUsedSearchList.Items.Add(new ListViewItem() { Content = points[i].ToString(), DataContext = points[i] });
            }
        }

        private void PointThatUsedSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            SetpointThatUsedSearchListViability(true);
            ReloadPointThatUsedSearchList();
        }

        private void PointThatUsedSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            SetpointThatUsedSearchListViability(false);
        }

        private void PointThatUsedSearchList_LostFocus(object sender, RoutedEventArgs e)
        {
            SetpointThatUsedSearchListViability(false);
        }

        private void PointThatUsedSearchList_GotFocus(object sender, RoutedEventArgs e)
        {
            SetpointThatUsedSearchListViability(true);
        }

        private void PointThatUsedSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadPointThatUsedSearchList();
        }

        private void PointThatUsedSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SelectPointThatUsed();
            else if (e.Key == Key.Escape)
            {
                Focus();
                SetPatientListVisibility(false);
            }
        }

        private void SelectPointThatUsed()
        {
            ListViewItem item = (ListViewItem)pointThatUsedSearchList.SelectedItem;
            if (item == null)
                return;
            for (int i = 0; i < pointThatUsed.Items.Count; i++)
            {
                ListBoxItem tempItem = (ListBoxItem)pointThatUsed.Items[i];
                if (tempItem.Content.Equals(item.Content))
                    return;
            }
            pointThatUsed.Items.Add(new ListBoxItem() { Content = item.Content, DataContext = item.DataContext });
            SetpointThatUsedSearchListViability(false);
        }

        private void PointThatUsed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SelectPointThatUsed();
            else if (e.Key == Key.Escape)
            {
                Focus();
                SetPatientListVisibility(false);
            }
        }

        private void PointThatUsedSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectPointThatUsed();
        }

        private void PointThatUsedDelete_Click(object sender, RoutedEventArgs e)
        {
            if (pointThatUsed.SelectedIndex != -1)
                pointThatUsed.Items.RemoveAt(pointThatUsed.SelectedIndex);
        }

        private void SymptomTreeDelete_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)symptomTreeView.SelectedItem;
            if (item == null)
                return;
            while (item.Parent.GetType() == typeof(TreeViewItem))
                item = (TreeViewItem)item.Parent;
            if (symptomTreeView.Items.Contains(item))
                symptomTreeView.Items.Remove(item);
        }

        private void ResoltSummeryTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddNewLine(resoltSummeryTextBox);
        }

        private void AddNewLine(TextBox textBox)
        {
            int temp = textBox.SelectionStart;
            textBox.Text = textBox.Text.Remove(temp, textBox.SelectionLength);
            textBox.Text = textBox.Text.Insert(temp, "\n");
            textBox.SelectionStart = temp + 1;
        }

        private void Notes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddNewLine(notes);
        }

        private void SummeryTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddNewLine(summeryTextBox);
        }

        private void PointThatUsed_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (pointThatUsed.SelectedIndex == -1)
                return;
            ListBoxItem item = (ListBoxItem)pointThatUsed.SelectedItem;
            new PointInfo((database.Point)item.DataContext).Show();
        }
    }
}
