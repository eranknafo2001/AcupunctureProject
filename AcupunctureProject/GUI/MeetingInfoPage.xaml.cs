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
using AcupunctureProject.Database;
using DPoint = AcupunctureProject.Database.Point;
using DColor = AcupunctureProject.Database.Color;
using Color = System.Windows.Media.Color;
using DTreatment = AcupunctureProject.Database.Treatment;

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for MeetingInfoPage.xaml
    /// </summary>
    public partial class MeetingInfoPage : Page
    {
        private Window Perent;
        private Patient SelectedPatient;
        private Meeting Meeting;
        private List<DPoint> PointsToAdd;
        private List<DPoint> PointsToRemove;
        private List<Symptom> SymptomsToAdd;
        private List<Symptom> SymptomsToRemove;
        private List<DTreatment> TreatmentsToAdd;
        private List<DTreatment> TreatmentsToRemove;
        private MeetingListByPatient MeetingList;

        private MeetingInfoPage()
        {
            InitializeComponent();
        }

        public MeetingInfoPage(Window perent, Meeting meeting, MeetingListByPatient meetingList = null) : this()
        {
            Perent = perent;
            Meeting = meeting;
            MeetingList = meetingList;
            if (meetingList != null)
                perent.Title += meeting.Patient.Name;
            PointsToAdd = new List<DPoint>();
            PointsToRemove = new List<DPoint>();
            SymptomsToAdd = new List<Symptom>();
            SymptomsToRemove = new List<Symptom>();
            SelectedPatient = meeting.Patient;
            patientSearchTextBox.Text = SelectedPatient.Name;
            date.SelectedDate = meeting.Date;
            resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.NOT_SET.MyToString(), DataContext = ResultValue.NOT_SET });
            resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.BETTER.MyToString(), DataContext = ResultValue.BETTER });
            resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.WORSE.MyToString(), DataContext = ResultValue.WORSE });
            resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.NO_CHANGE.MyToString(), DataContext = ResultValue.NO_CHANGE });
            resolt.SelectedIndex = (int)meeting.Result;
            level0.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(0) };
            level1.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(1) };
            level2.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(2) };
            level3.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(3) };
            level4.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(4) };
            level5.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(5) };
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
            TreatmentList.IsEnabled = true;
            TreatmentSearchTextBox.IsEnabled = true;
            List<Symptom> symptoms = meeting.Symptoms;
            for (int i = 0; i < symptoms.Count; i++)
                SddItemToSymptomTree(symptoms[i]);
            FindConnectedPoint();
            List<DPoint> pointsThatUsed = meeting.Points;
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
            Perent.Close();
        }

        private void RelaodSymptomList() => 
            symptomSearchList.ItemsSource = 
                DatabaseConnection.Instance.FindSymptom(symptomSearch.Text)
                .ToList(item => new ListViewItem() { Content = item.ToString(), DataContext = item });

        private void SetPointThatUsedSearchListViability(bool val)
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
            SymptomsToAdd.Add((Symptom)item.DataContext);
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
                    if (chItem.DataContext.GetType() == typeof(DPoint))
                    {
                        var point = (DPoint)chItem.DataContext;
                        if (!count.ContainsKey(chItem.DataContext.GetType()))
                            count.Add(chItem.DataContext.GetType(), new Dictionary<int, List<TreeViewItem>>());
                        if (!count[chItem.DataContext.GetType()].ContainsKey(point.Id))
                            count[chItem.DataContext.GetType()].Add(point.Id, new List<TreeViewItem>());
                        count[chItem.DataContext.GetType()][point.Id].Add(chItem);
                    }
                    else if (chItem.DataContext.GetType() == typeof(Channel))
                    {
                        var point = (Channel)chItem.DataContext;
                        if (!count.ContainsKey(chItem.DataContext.GetType()))
                            count.Add(chItem.DataContext.GetType(), new Dictionary<int, List<TreeViewItem>>());
                        if (!count[chItem.DataContext.GetType()].ContainsKey(point.Id))
                            count[chItem.DataContext.GetType()].Add(point.Id, new List<TreeViewItem>());
                        count[chItem.DataContext.GetType()][point.Id].Add(chItem);
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
            TreeViewItem sym = new TreeViewItem() { Header = symptom.ToString(), DataContext = symptom, IsExpanded = true };
            foreach (var symcon in symptom.PointsConnections)
            {
                sym.Items.Add(new TreeViewItem()
                {
                    Header = symcon.Point.ToString(),
                    DataContext = symcon.Point,
                    Foreground = new SolidColorBrush()
                    {
                        Color = DatabaseConnection.Instance.GetLevel(symcon.Importance)
                    }
                });
            }
            foreach (var symchan in symptom.ChannelConnections)
            {
                sym.Items.Add(new TreeViewItem()
                {
                    Header = symchan.Channel.ToString(),
                    DataContext = symchan.Channel,
                    Foreground = new SolidColorBrush()
                    {
                        Color = DatabaseConnection.Instance.GetLevel(symchan.Importance)
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
            if (item.DataContext.GetType() == typeof(DPoint))
            {
                var con = (DPoint)item.DataContext;
                new PointInfo(con).Show();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
        }

        private void SaveData()
        {
            Meeting.Date = (DateTime)date.SelectedDate;
            Meeting.Description = notes.Text;
            Meeting.Summery = summeryTextBox.Text;
            Meeting.ResultDescription = resoltSummeryTextBox.Text;
            Meeting.Result = (ResultValue)resolt.SelectedIndex;


            for (int i = 0; i < SymptomsToAdd.Count; i++)
            {
                int j = 0;
                while (j < SymptomsToRemove.Count)
                {
                    if (SymptomsToAdd[i].Id == SymptomsToRemove[j].Id)
                    {
                        SymptomsToAdd.RemoveAt(i);
                        SymptomsToRemove.RemoveAt(j);
                        continue;
                    }
                    j++;
                }
            }

            foreach (var sym in SymptomsToRemove)
                Meeting.Symptoms.Remove(sym);
            foreach (var sym in SymptomsToAdd)
                Meeting.Symptoms.Add(sym);
            SymptomsToAdd.Clear();
            SymptomsToRemove.Clear();

            for (int i = 0; i < PointsToAdd.Count; i++)
            {
                int j = 0;
                while (j < PointsToRemove.Count)
                {
                    if (PointsToAdd[i].Name == PointsToRemove[j].Name)
                    {
                        PointsToRemove.RemoveAt(j);
                        PointsToAdd.RemoveAt(i);
                        continue;
                    }
                    j++;
                }
            }

            foreach (var point in PointsToRemove)
                Meeting.Points.Remove(point);
            foreach (var point in PointsToAdd)
                Meeting.Points.Add(point);
            PointsToAdd.Clear();
            PointsToRemove.Clear();

            DatabaseConnection.Instance.Update(Meeting);
        }

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            Perent.Close();
        }

        private void OpenPatientButton_Click(object sender, RoutedEventArgs e)
        {
            PatientInfo p = new PatientInfo(SelectedPatient);
            p.Show();
        }

        private void ReloadPointThatUsedSearchList()
        {
            pointThatUsedSearchList.ItemsSource =
                Main.AllPoints
                    .FindAll(p => p.Name.ToLower().Contains(pointThatUsedSearch.Text.ToLower()))
                    .ToList(point => new ListViewItem()
                    {
                        Content = point.ToString(),
                        DataContext = point
                    });
            pointThatUsedSearchList.SelectedIndex = 0;
        }

        private void PointThatUsedSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            SetPointThatUsedSearchListViability(true);
            ReloadPointThatUsedSearchList();
        }

        private void PointThatUsedSearch_LostFocus(object sender, RoutedEventArgs e) => SetPointThatUsedSearchListViability(false);

        private void PointThatUsedSearchList_LostFocus(object sender, RoutedEventArgs e) => SetPointThatUsedSearchListViability(false);

        private void PointThatUsedSearchList_GotFocus(object sender, RoutedEventArgs e) => SetPointThatUsedSearchListViability(true);

        private void PointThatUsedSearch_TextChanged(object sender, TextChangedEventArgs e) => ReloadPointThatUsedSearchList();

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
            AddItemToPointThatUsed((DPoint)item.DataContext);
            SetPointThatUsedSearchListViability(false);
        }

        private void PointThatUsed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SelectPointThatUsed();
        }

        private void PointThatUsedSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e) => SelectPointThatUsed();

        private void PointThatUsedDelete_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)pointThatUsed.SelectedItem;
            if (item == null)
                return;
            pointThatUsed.Items.RemoveAt(pointThatUsed.SelectedIndex);
            PointsToRemove.Add((DPoint)item.DataContext);
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
            SymptomsToRemove.Add((Symptom)item.DataContext);
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
            new PointInfo((DPoint)item.DataContext).Show();
        }

        private void PointThatUsed_Drop(object sender, DragEventArgs e)
        {
            string[] formats = e.Data.GetFormats();
            for (int i = 0; i < formats.Length; i++)
            {
                object data = e.Data.GetData(formats[i]);
                if (data == null)
                    return;
                if (data.GetType() == typeof(TreeViewItem))
                {
                    TreeViewItem item = (TreeViewItem)data;
                    if (item.DataContext.GetType() == typeof(DPoint))
                        AddItemToPointThatUsed((DPoint)item.DataContext);
                    //else if (item.DataContext.GetType() == typeof(ConnectionValue<DPoint>))) ;
                }
            }
        }

        private void SymptomTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (symptomTreeView.SelectedItem != null && e.LeftButton.Equals(MouseButtonState.Pressed))
                DragDrop.DoDragDrop(symptomTreeView, (TreeViewItem)symptomTreeView.SelectedItem, DragDropEffects.Copy);
        }

        private void AddItemToPointThatUsed(DPoint point)
        {
            for (int i = 0; i < pointThatUsed.Items.Count; i++)
            {
                ListBoxItem tempItem = (ListBoxItem)pointThatUsed.Items[i];
                if (tempItem.Content.Equals(point.ToString()))
                    return;
            }
            pointThatUsed.Items.Add(new ListBoxItem() { Content = point.ToString(), DataContext = point });
        }

        private void TreatmentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender.GetType() != typeof(ListBox))
                return;
            var s = (ListBox)sender;
            if (s.SelectedIndex == -1)
                return;
            new Treatment((DTreatment)((ListBoxItem)s.SelectedItem).DataContext).Show();
        }

        private void TreatmentList_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender.GetType() != typeof(ListBox))
                return;
            var s = (ListBox)sender;
            if (s.SelectedIndex == -1)
                return;
            if (e.Key.Equals(Key.Enter))
                new Treatment((DTreatment)((ListBoxItem)s.SelectedItem).DataContext).Show();
        }

        private void TreatmentSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e) => SelectTreatment();

        private void TreatmentSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                SelectTreatment();
        }

        private void SelectTreatment()
        {
            if (TreatmentSearchList.SelectedIndex == -1)
                return;
            TreatmentList.Items.Add(new ListBoxItem()
            {
                Content = ((ListViewItem)TreatmentSearchList.SelectedItem).Content,
                DataContext = ((ListViewItem)TreatmentSearchList.SelectedItem).DataContext
            });
            TreatmentsToAdd.Add((DTreatment)((ListViewItem)TreatmentSearchList.SelectedItem).DataContext);
        }

        private void TreatmentSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender.GetType() != typeof(TextBox))
                return;
            TreatmentSearchList.ItemsSource =
                Main.AllTreatments
                .FindAll(t => t.Name.Contains(((TextBox)sender).Text))
                .ToList(i => new ListViewItem()
                {
                    Content = i.ToString(),
                    DataContext = i
                });
        }

        private void TreatmentSearchVisFalse(object sender, RoutedEventArgs e) => SetTreatmentSearchListViability(false);

        private void TreatmentSearchVisTrue(object sender, RoutedEventArgs e) => SetTreatmentSearchListViability(true);

        private void SetTreatmentSearchListViability(bool val)
        {
            if (val)
            {
                TreatmentSearchList.Margin = new Thickness(TreatmentSearchTextBox.Margin.Left, TreatmentSearchTextBox.Margin.Top + TreatmentSearchTextBox.ExtentHeight, 0, 0);
                TreatmentSearchList.Visibility = Visibility.Visible;
                TreatmentSearchList.Height = 100;
            }
            else
            {
                TreatmentSearchList.Visibility = Visibility.Collapsed;
                TreatmentSearchList.Height = 0;
            }
        }

        private void TreatmentSearchTextBox_KeyDown(object sender, KeyEventArgs e) => SelectTreatment();
    }
}
