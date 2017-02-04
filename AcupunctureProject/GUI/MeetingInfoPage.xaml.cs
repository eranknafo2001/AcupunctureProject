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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AcupunctureProject.database;

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for MeetingInfoPage.xaml
    /// </summary>
    public partial class MeetingInfoPage : Page
    {
        private Window perent;
        private Patient selectedPatient;
        private Meeting meeting;
        private List<database.Point> pointsToAdd;
        private List<database.Point> pointsToRemove;
        private List<Symptom> symptomsToAdd;
        private List<Symptom> symptomsToRemove;
        private MeetingListByPatient meetingList;

        private MeetingInfoPage()
        {
            InitializeComponent();
        }

        public MeetingInfoPage(Window perent, Meeting meeting, MeetingListByPatient meetingList = null) : this()
        {
            this.perent = perent;
            this.meeting = meeting;
            this.meetingList = meetingList;
            if (meetingList != null)
                perent.Title += Database.Instance.GetPatientRelativeToMeeting(meeting).Name;
            pointsToAdd = new List<database.Point>();
            pointsToRemove = new List<database.Point>();
            symptomsToAdd = new List<Symptom>();
            symptomsToRemove = new List<Symptom>();
            selectedPatient = Database.Instance.GetPatientRelativeToMeeting(meeting);
            patientSearchTextBox.Text = selectedPatient.Name;
            date.SelectedDate = meeting.Date;
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.NOT_SET.ToString(), DataContext = Meeting.ResultValue.NOT_SET });
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.BETTER.ToString(), DataContext = Meeting.ResultValue.BETTER });
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.WORSE.ToString(), DataContext = Meeting.ResultValue.WORSE });
            resolt.Items.Add(new ComboBoxItem() { Content = Meeting.ResultValue.NO_CHANGE.ToString(), DataContext = Meeting.ResultValue.NO_CHANGE });
            resolt.SelectedIndex = meeting.Result.Value;
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
            List<Symptom> symptoms = Database.Instance.GetAllSymptomRelativeToMeeting(meeting);
            for (int i = 0; i < symptoms.Count; i++)
                SddItemToSymptomTree(symptoms[i]);
            FindConnectedPoint();
            List<database.Point> pointsThatUsed = Database.Instance.GetAllPointsRelativeToMeeting(meeting);
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
            perent.Close();
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
            SddItemToSymptomTree((Symptom)item.DataContext);
            symptomsToAdd.Add((Symptom)item.DataContext);
            FindConnectedPoint();
            SetSymptomListVisibility(false);
            symptomSearch.Clear();
            RelaodSymptomList();
        }

        private void FindConnectedPoint()
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

        private void SddItemToSymptomTree(Symptom symptom)
        {
            TreeViewItem sym = new TreeViewItem() { Header = symptom.ToString(), DataContext = symptom };
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

        private void SymptomSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                SelectSymptom();
        }

        private void SymptomSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                SelectSymptom();
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
                new PointInfo(con.Value).Show();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
        }

        private void SaveData()
        {
            meeting.Date = (DateTime)date.SelectedDate;
            meeting.Description = notes.Text;
            meeting.Summery = summeryTextBox.Text;
            meeting.ResultDescription = resoltSummeryTextBox.Text;
            meeting.Result = Meeting.ResultValue.FromValue(resolt.SelectedIndex);
            Database.Instance.UpdateMeeting(meeting);

            for (int i = 0; i < symptomsToAdd.Count; i++)
            {
                int j = 0;
                while (j < symptomsToRemove.Count)
                {
                    if (symptomsToAdd[i].Id == symptomsToRemove[j].Id)
                    {
                        symptomsToAdd.RemoveAt(i);
                        symptomsToRemove.RemoveAt(j);
                        continue;
                    }
                    j++;
                }
            }

            for (int i = 0; i < symptomsToRemove.Count; i++)
                Database.Instance.DeleteSymptomMeetingRelation(symptomsToRemove[i], meeting);
            for (int i = 0; i < symptomsToAdd.Count; i++)
                Database.Instance.InsertSymptomMeetingRelation(symptomsToAdd[i], meeting);
            symptomsToAdd.Clear();
            symptomsToRemove.Clear();

            for (int i = 0; i < pointsToAdd.Count; i++)
            {
                int j = 0;
                while (j < pointsToRemove.Count)
                {
                    if (pointsToAdd[i].Name == pointsToRemove[j].Name)
                    {
                        pointsToRemove.RemoveAt(j);
                        pointsToAdd.RemoveAt(i);
                        continue;
                    }
                    j++;
                }
            }

            for (int i = 0; i < pointsToRemove.Count; i++)
                Database.Instance.DeleteMeetingPoint(meeting, pointsToRemove[i]);
            for (int i = 0; i < pointsToAdd.Count; i++)
                Database.Instance.InsertMeetingPointRelation(meeting, pointsToAdd[i]);
            pointsToAdd.Clear();
            pointsToRemove.Clear();

            if (meetingList != null)
                meetingList.UpdateData();
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
            pointThatUsedSearchList.Items.Clear();
            List<database.Point> points = Database.Instance.FindPoint(pointThatUsedSearch.Text);
            for (int i = 0; i < points.Count; i++)
            {
                pointThatUsedSearchList.Items.Add(new ListViewItem() { Content = points[i].ToString(), DataContext = points[i] });
            }
            pointThatUsedSearchList.SelectedIndex = 0;
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
            pointsToAdd.Add((database.Point)item.DataContext);
            SetpointThatUsedSearchListViability(false);
        }

        private void PointThatUsed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SelectPointThatUsed();
        }

        private void PointThatUsedSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectPointThatUsed();
        }

        private void PointThatUsedDelete_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)pointThatUsed.SelectedItem;
            if (item == null)
                return;
            pointThatUsed.Items.RemoveAt(pointThatUsed.SelectedIndex);
            pointsToRemove.Add((database.Point)item.DataContext);
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
            symptomsToRemove.Add((Symptom)item.DataContext);
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

        private void ResoltSummeryTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddNewLine(resoltSummeryTextBox);
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
