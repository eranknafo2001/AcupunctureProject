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
    /// Interaction logic for MeetingInfo.xaml
    /// </summary>
    public partial class MeetingInfo : Window
    {
        private Patient selectedPatient;
        private Meeting meeting;

        public MeetingInfo(Meeting meeting)
        {
            InitializeComponent();
            this.meeting = meeting;
            selectedPatient = Database.Instance.getPatientRelativeToMeeting(meeting);
            patientSearchTextBox.Text = selectedPatient.Name;
            date.SelectedDate = meeting.Date;
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
            List<Symptom> symptoms = Database.Instance.getAllSymptomRelativeToMeeting(meeting);
            for (int i = 0; i < symptoms.Count; i++)
                addItemToSymptomTree(symptoms[i]);
            refindConnectedPoint();
            List<database.Point> pointsThatUsed = Database.Instance.getAllPointRelativeToMeeting(meeting);
            for (int i = 0; i < pointsThatUsed.Count; i++)
                pointThatUsed.Items.Add(new ListBoxItem() { Content = pointsThatUsed[i].ToString(), DataContext = pointsThatUsed[i] });
            notes.Text = meeting.Description;
            resoltSummeryTextBox.Text = meeting.ResultDescription;
            resolt.SelectedItem = meeting.Result;
            summeryTextBox.Text = meeting.Summery;

        }

        private void SymptomSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            RelaodSymptomList();
        }

        private void Censel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RelaodSymptomList()
        {
            symptomSearchList.Items.Clear();
            List<Symptom> items = Database.Instance.findSymptom(symptomSearch.Text);
            for (int i = 0; i < items.Count; i++)
                symptomSearchList.Items.Add(new ListViewItem() { Content = items[i].ToString(), DataContext = items[i] });
        }

        private void setpointThatUsedSearchListViability(bool val)
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

        private void selectSymptom()
        {
            ListViewItem item = (ListViewItem)symptomSearchList.SelectedItem;
            for (int i = 0; i < symptomTreeView.Items.Count; i++)
            {
                TreeViewItem tempItem = (TreeViewItem)symptomTreeView.Items[i];
                Symptom temps = (Symptom)tempItem.DataContext;
                Symptom temps2 = (Symptom)item.DataContext;
                if (temps.Id == temps2.Id)
                    return;
            }
            addItemToSymptomTree((Symptom)item.DataContext);
            refindConnectedPoint();
            symptomSearch.Clear();
            RelaodSymptomList();
        }

        private void refindConnectedPoint()
        {
            Dictionary<Type, Dictionary<int, List<TreeViewItem>>> count = new Dictionary<Type, Dictionary<int, List<TreeViewItem>>>();
            for (int i = 0; i < symptomTreeView.Items.Count; i++)
            {
                TreeViewItem symItem = (TreeViewItem)symptomTreeView.Items[i];
                for (int j = 0; j < symItem.Items.Count; j++)
                {
                    TreeViewItem chItem = (TreeViewItem)symItem.Items[j];
                    if (chItem.DataContext.GetType() == typeof(ConnectionValue<database.Point>))
                    {
                        ConnectionValue<database.Point> point = (ConnectionValue<database.Point>)chItem.DataContext;
                        if (!count.ContainsKey(chItem.DataContext.GetType()))
                            count.Add(chItem.DataContext.GetType(), new Dictionary<int, List<TreeViewItem>>());
                        if (!count[chItem.DataContext.GetType()].ContainsKey(point.Value.Id))
                            count[chItem.DataContext.GetType()].Add(point.Value.Id, new List<TreeViewItem>());
                        count[chItem.DataContext.GetType()][point.Value.Id].Add(chItem);
                    }
                    else if (chItem.DataContext.GetType() == typeof(ConnectionValue<Channel>))
                    {
                        ConnectionValue<Channel> point = (ConnectionValue<Channel>)chItem.DataContext;
                        if (!count.ContainsKey(chItem.DataContext.GetType()))
                            count.Add(chItem.DataContext.GetType(), new Dictionary<int, List<TreeViewItem>>());
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
            Database.Instance.insertSymptomMeetingRelation(symptom, meeting);
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
            RelaodSymptomList();
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
            if (item == null)
                return;
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
            meeting.Date = (DateTime)date.SelectedDate;
            meeting.Description = notes.Text;
            meeting.Summery = summeryTextBox.Text;
            meeting.ResultDescription = resoltSummeryTextBox.Text;
            ComboBoxItem resoltItem = (ComboBoxItem)resolt.SelectedItem;
            meeting.Result = (Meeting.ResultValue)resoltItem.DataContext;
            Database.Instance.updateMeeting(meeting);
        }

        private void saveAndExit_Click(object sender, RoutedEventArgs e)
        {
            saveData();
            Close();
        }

        private void openPatientButton_Click(object sender, RoutedEventArgs e)
        {
            PatientInfo p = new PatientInfo(selectedPatient);
            p.Show();
        }

        private void reloadPointThatUsedSearchList()
        {
            pointThatUsedSearchList.Items.Clear();
            List<database.Point> points = Database.Instance.findPoint(pointThatUsedSearch.Text);
            for (int i = 0; i < points.Count; i++)
            {
                pointThatUsedSearchList.Items.Add(new ListViewItem() { Content = points[i].ToString(), DataContext = points[i] });
            }
            pointThatUsedSearchList.SelectedIndex = 0;
        }

        private void pointThatUsedSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            setpointThatUsedSearchListViability(true);
            reloadPointThatUsedSearchList();
        }

        private void pointThatUsedSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            setpointThatUsedSearchListViability(false);
        }

        private void pointThatUsedSearchList_LostFocus(object sender, RoutedEventArgs e)
        {
            setpointThatUsedSearchListViability(false);
        }

        private void pointThatUsedSearchList_GotFocus(object sender, RoutedEventArgs e)
        {
            setpointThatUsedSearchListViability(true);
        }

        private void pointThatUsedSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            reloadPointThatUsedSearchList();
        }

        private void pointThatUsedSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                selectPointThatUsed();
        }

        private void selectPointThatUsed()
        {
            ListViewItem item = (ListViewItem)pointThatUsedSearchList.SelectedItem;
            bool isThereACopy = false;
            for (int i = 0; i < pointThatUsed.Items.Count && !isThereACopy; i++)
            {
                ListBoxItem tempItem = (ListBoxItem)pointThatUsed.Items[i];
                isThereACopy = tempItem.Content == item.Content;
            }

            if (isThereACopy)
                return;
            pointThatUsed.Items.Add(new ListBoxItem() { Content = item.Content, DataContext = item.DataContext });
            Database.Instance.insertMeetingPointRelation(meeting, (database.Point)item.DataContext);
        }

        private void pointThatUsed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                selectPointThatUsed();
        }

        private void pointThatUsedSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectPointThatUsed();
        }

        private void pointThatUsedDelete_Click(object sender, RoutedEventArgs e)
        {
            if (pointThatUsed.SelectedIndex == -1)
                return;
            pointThatUsed.Items.RemoveAt(pointThatUsed.SelectedIndex);
            ListBoxItem item = (ListBoxItem)pointThatUsed.SelectedItem;
            Database.Instance.deleteMeetingPoint(meeting, (database.Point)item.DataContext);
        }

        private void symptomTreeDelete_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)symptomTreeView.SelectedItem;
            if (item == null)
                return;
            while (item.Parent.GetType() == typeof(TreeViewItem))
                item = (TreeViewItem)item.Parent;
            if (symptomTreeView.Items.Contains(item))
                symptomTreeView.Items.Remove(item);
            Database.Instance.deleteSymptomMeetingRelation((Symptom)item.DataContext, meeting);
        }
    }
}
