using AcupunctureProject.Database;
using DColor = AcupunctureProject.Database.Color;
using DPoint = AcupunctureProject.Database.Point;
using DTreatment = AcupunctureProject.Database.Treatment;
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
using Color = System.Windows.Media.Color;
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
            resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.NOT_SET.MyToString(), DataContext = ResultValue.NOT_SET });
            resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.BETTER.MyToString(), DataContext = ResultValue.BETTER });
            resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.WORSE.MyToString(), DataContext = ResultValue.WORSE });
            resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.NO_CHANGE.MyToString(), DataContext = ResultValue.NO_CHANGE });
            resolt.SelectedIndex = 0;
            level0.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(0) };
            level1.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(1) };
            level2.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(2) };
            level3.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(3) };
            level4.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(4) };
            level5.Background = new SolidColorBrush() { Color = DatabaseConnection.Instance.GetLevel(5) };
            foreach (var i in Main.AllTreatments)
                TreatmentSearchList.Items.Add(new ListViewItem() { Content = i.ToString(), DataContext = i });
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
            patientSearchList.ItemsSource = DatabaseConnection.Instance.FindPatient(patientSearchTextBox.Text).MyCast(t => new ListViewItem() { Content = t.ToStringInSearch(), DataContext = t });
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
            copyFromLastMeeting.IsEnabled = true;
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
            SetPatientListVisibility(false);
        }

        private void PatientSearchList_GotFocus(object sender, RoutedEventArgs e)
        {
            SetPatientListVisibility(true);
        }

        private void RelaodSymptomList() => symptomSearchList.ItemsSource = DatabaseConnection.Instance.FindSymptom(symptomSearch.Text).MyCast(item => new ListViewItem() { Content = item.ToString(), DataContext = item });

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

        private void PatientSearchList_LostFocus(object sender, RoutedEventArgs e) => SetPatientListVisibility(false);

        private void PatientSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                SelectPatient();
        }

        private void SelectSymptom()
        {
            var item = (ListViewItem)symptomSearchList.SelectedItem;
            if (item == null)
                return;
            foreach (var sym in symptomTreeView.Items)
            {
                if (((Symptom)((TreeViewItem)sym).DataContext).Id == ((Symptom)item.DataContext).Id)
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
                    if (chItem.DataContext.GetType() == typeof(DPoint))
                    {
                        var point = (DPoint)chItem.DataContext;
                        if (!count[chItem.DataContext.GetType()].ContainsKey(point.Id))
                            count[chItem.DataContext.GetType()].Add(point.Id, new List<TreeViewItem>());
                        count[chItem.DataContext.GetType()][point.Id].Add(chItem);
                    }
                    else if (chItem.DataContext.GetType() == typeof(Channel))
                    {
                        var point = (Channel)chItem.DataContext;
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

        private void AddItemToSymptomTree(Symptom symptom)
        {
            TreeViewItem sym = new TreeViewItem() { Header = symptom.ToString(), DataContext = symptom, IsExpanded = true };
            DatabaseConnection.Instance.GetChildren(symptom);
            foreach (var con in symptom.PointsConnections)
            {
                DatabaseConnection.Instance.GetChildren(con);
                sym.Items.Add(new TreeViewItem()
                {
                    Header = con.Point.ToString(),
                    DataContext = con.Point,
                    Foreground = new SolidColorBrush()
                    {
                        Color = DatabaseConnection.Instance.GetLevel(con.Importance)
                    }
                });
            }

            foreach (var con in symptom.ChannelConnections)
            {
                DatabaseConnection.Instance.GetChildren(con);
                sym.Items.Add(new TreeViewItem()
                {
                    Header = con.Channel.ToString(),
                    DataContext = con.Channel,
                    Foreground = new SolidColorBrush()
                    {
                        Color = DatabaseConnection.Instance.GetLevel(con.Importance)
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
            if (item.DataContext.GetType() == typeof(DPoint))
            {
                var con = (DPoint)item.DataContext;
                PointInfo p = new PointInfo(con);
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
            var meeting = new Meeting()
            {
                Patient = selectedPatient,
                Date = (DateTime)date.SelectedDate,
                Description = notes.Text,
                Summery = summeryTextBox.Text,
                ResultDescription = resoltSummeryTextBox.Text,
                Result = (ResultValue)((ComboBoxItem)resolt.SelectedItem).DataContext,
                Symptoms = new List<Symptom>(),
                Points = new List<DPoint>()
            };
            foreach (var s in symptomTreeView.Items)
                meeting.Symptoms.Add((Symptom)((TreeViewItem)s).DataContext);
            foreach (var p in pointThatUsed.Items)
                meeting.Points.Add((DPoint)((ListBoxItem)p).DataContext);
            foreach (var t in TreatmentList.Items)
                meeting.Treatments.Add((DTreatment)((ListBoxItem)t).DataContext);
            meeting.Diagnostic = selectedPatient.Diagnostics.OrderByDescending(d => d.CreationDate).FirstOrDefault();
            DatabaseConnection.Instance.InsertWithChildren(meeting);
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
            pointThatUsedSearchList.ItemsSource =
                Main.AllPoints.FindAll(p => p.Name.ToLower().Contains(pointThatUsedSearch.Text.ToLower()))
                .MyCast(point => new ListViewItem() { Content = point.ToString(), DataContext = point });
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
            AddItemToPointThatUsed((DPoint)item.DataContext);
            SetPointThatUsedSearchListViability(false);
        }

        private void AddItemToPointThatUsed(DPoint point)
        {
            foreach (var item in pointThatUsed.Items)
            {
                if (((ListBoxItem)item).Content.Equals(point.ToString()))
                    return;
            }
            pointThatUsed.Items.Add(new ListBoxItem() { Content = point.ToString(), DataContext = point });
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

        private void PointThatUsedSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e) => SelectPointThatUsed();

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
            new PointInfo((DPoint)item.DataContext).Show();
        }

        private void CopyFromLastMeeting_Click(object sender, RoutedEventArgs e)
        {
            Meeting meeting = DatabaseConnection.Instance.GetTheLastMeeting(selectedPatient);
            if (meeting == null)
            {
                MessageBox.Show("אין לו פגישות", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading);
                return;
            }
            symptomTreeView.Items.Clear();
            foreach (var sym in meeting.Symptoms)
                AddItemToSymptomTree(sym);
            pointThatUsed.ItemsSource = meeting.Points.MyCast(point => new ListBoxItem() { Content = point.ToString(), DataContext = point });
            notes.Text = meeting.Description;
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
        }

        private void TreatmentSearchTextBox_TextChanged(object sender, TextChangedEventArgs e) =>
            TreatmentSearchList.ItemsSource =
            Main.AllTreatments
            .FindAll(t => t.Name.Contains(TreatmentSearchTextBox.Text))
            .MyCast(i => new ListViewItem() { Content = i.ToString(), DataContext = i });

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
