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
using DPoint = AcupunctureProject.Database.Point;

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for PointInfo.xaml
    /// </summary>
    public partial class PointInfo : Window
    {
        private DPoint point;

        private List<Symptom> SymptomToRemove { get; set; }
        private List<SymptomPoint> SymptomToAdd { get; set; }

        public PointInfo(DPoint point)
        {
            InitializeComponent();
            Title += point.Name;
            Main.UpdatePoints += new Main.EventHandler(Update);
            this.point = point;
            SymptomToAdd = new List<SymptomPoint>();
            SymptomToRemove = new List<Symptom>();
            ReloadSymptomSearchList();
            pointsList.ItemsSource = Main.AllPoints.ToList(p => new ListViewItem() { Content = p.ToString(), DataContext = p });
            syptomTreeView.Items.Clear();
            DatabaseConnection.Instance.GetChildren(point);
            List<Symptom> symptomList = DatabaseConnection.Instance.GetChildren(point.Symptoms);
            foreach (var sym in symptomList)
                AddItemToSymptomTree(sym);
            if (System.IO.File.Exists(point.Image))
                pointImage.Source = new BitmapImage(new Uri(point.Image));
            name.Content = point.Name;
            minDepth.Text = point.MinNeedleDepth.ToString();
            maxDepth.Text = point.MaxNeedleDepth.ToString();
            place.Text = point.Position;
            note.Text = point.Note;
            comment1.Text = point.Comment1;
            comment2.Text = point.Comment2;
        }

        ~PointInfo() => Main.UpdatePoints -= new Main.EventHandler(Update);

        private void Points_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = (ListViewItem)pointsList.SelectedItem;
            if (item == null)
                return;
            SetAll((DPoint)item.DataContext);
        }

        private void SetAll(DPoint point) => new PointInfo(point).Show();

        private void SyptomTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (TreeViewItem)syptomTreeView.SelectedItem;
            if (item == null)
                return;
            if (item.DataContext.GetType() == typeof(DPoint))
                SetAll((DPoint)item.DataContext);
        }

        private void SaveData()
        {
            point.Comment1 = comment1.Text;
            point.Comment2 = comment2.Text;
            point.MaxNeedleDepth = int.Parse(maxDepth.Text);
            point.MinNeedleDepth = int.Parse(minDepth.Text);
            point.Note = note.Text;
            point.Position = place.Text;
            for (int i = 0; i < SymptomToAdd.Count; i++)
            {
                int j = 0;
                while (j < SymptomToRemove.Count)
                {
                    if (SymptomToAdd[i].Symptom.Equals(SymptomToRemove[j]))
                    {
                        SymptomToRemove.RemoveAt(j);
                        SymptomToAdd.RemoveAt(i);
                        continue;
                    }
                    j++;
                }
            }
                point.SymptomConnections.AddRange(SymptomToAdd);
            foreach (var sym in SymptomToRemove)
                point.SymptomConnections.RemoveAll(s => s.SymptomId == sym.Id);
            DatabaseConnection.Instance.Update(point);
        }

        private void Censel_Click(object sender, RoutedEventArgs e) => Close();

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e) => SaveData();

        private void PointImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            new FullWindowPic(pointImage.Source, (int)(pointImage.ActualWidth * 2),
                                                 (int)(pointImage.ActualHeight * 2)).Show();

        private void PointsSearch_TextChanged(object sender, TextChangedEventArgs e) => Update();

        private void SetSymptomSearchListVisability(bool val)
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

        private void ReloadSymptomSearchList()
        {
            List<Symptom> sl = DatabaseConnection.Instance.FindSymptom(symptomSearch.Text);
            symptomSearchList.ItemsSource = sl.ToList(s => new ListViewItem() { Content = s.ToString(), DataContext = s });
        }

        private void SelectSymptom()
        {
            ListViewItem item = (ListViewItem)symptomSearchList.SelectedItem;
            if (item == null)
                return;
            for (int i = 0; i < syptomTreeView.Items.Count; i++)
                if (item.DataContext.Equals(((TreeViewItem)syptomTreeView.Items[i]).DataContext))
                    return;

            SymptomToAdd.Add(new SymptomPoint() { Symptom = (Symptom)item.DataContext, Importance = symptomSearchImportece.SelectedIndex, Comment = "" });
            AddItemToSymptomTree((Symptom)item.DataContext);
            SetSymptomSearchListVisability(false);
        }

        private void AddItemToSymptomTree(Symptom sym)
        {
            TreeViewItem symptom = new TreeViewItem() { Header = sym.ToString(), DataContext = sym };
            foreach (var con in sym.PointsConnections)
            {
                DatabaseConnection.Instance.GetChildren(con);
                symptom.Items.Add(new TreeViewItem()
                {
                    Header = con.Point.ToString(),
                    DataContext = con.Point,
                    Foreground = new SolidColorBrush()
                    {
                        Color = DatabaseConnection.Instance.GetLevel(con.Importance)
                    }
                });
            }
            foreach (var cancon in sym.ChannelConnections)
            {
                DatabaseConnection.Instance.GetChildren(cancon);
                symptom.Items.Add(new TreeViewItem()
                {
                    Header = cancon.Channel.ToString(),
                    DataContext = cancon.Channel,
                    Foreground = new SolidColorBrush()
                    {
                        Color = DatabaseConnection.Instance.GetLevel(cancon.Importance)
                    }
                });
            }
            syptomTreeView.Items.Add(symptom);
        }

        private void SymptomSearch_TextChanged(object sender, TextChangedEventArgs e) => ReloadSymptomSearchList();

        private void SymptomSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SelectSymptom();
        }

        private void SymptomSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e) => SelectSymptom();

        private void DeleteSymptom_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)syptomTreeView.SelectedItem;
            if (item == null)
                return;
            while (item.Parent.GetType() == typeof(TreeViewItem))
                item = (TreeViewItem)item.Parent;
            SymptomToRemove.Add((Symptom)item.DataContext);
            syptomTreeView.Items.Remove(item);
        }

        private void SymptomSearch_GotFocus(object sender, RoutedEventArgs e) => SetSymptomSearchListVisability(true);

        private void SymptomSearch_LostFocus(object sender, RoutedEventArgs e) => SetSymptomSearchListVisability(false);

        private void SymptomSearchList_GotFocus(object sender, RoutedEventArgs e) => SetSymptomSearchListVisability(true);

        private void SymptomSearchList_LostFocus(object sender, RoutedEventArgs e) => SetSymptomSearchListVisability(false);

        private void Update() =>
                pointsList.ItemsSource = (from s in Main.AllPoints
                                          where s.Name.ToLower().Contains(pointsSearch.Text.ToLower())
                                          select s)
                                          .ToList(p=> new ListViewItem()
                                          {
                                              Content = p.ToString(),
                                              DataContext = p
                                          });
    }
}
