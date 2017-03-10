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

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for PointInfo.xaml
    /// </summary>
    public partial class PointInfo : Window
    {
        private Database.Point point;

        private List<Symptom> symptomToRemove;
        private List<ConnectionValue<Symptom>> symptomToAdd;

        public PointInfo(Database.Point point)
        {
            InitializeComponent();
            Title += point.Name;
            this.point = point;
            symptomToAdd = new List<ConnectionValue<Symptom>>();
            symptomToRemove = new List<Symptom>();
            ReloadSymptomSearchList();
            List<Database.Point> points = DatabaseConnection.Instance.GetAllPoints();
            for (int i = 0; i < points.Count; i++)
                pointsList.Items.Add(new ListViewItem() { Content = points[i].ToString(), DataContext = points[i] });
            syptomTreeView.Items.Clear();
            List<ConnectionValue<Symptom>> symptomList = DatabaseConnection.Instance.GetAllSymptomRelativeToPoint(point);
            for (int i = 0; i < symptomList.Count; i++)
            {
                AddItemToSymptomTree(symptomList[i].Value);
            }
            string[] tempFolder = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');
            var folder = "";
            for (int i = 0; i < tempFolder.Length - 1; i++)
            {
                folder += tempFolder[i] + "\\";
            }
            try
            {
                BitmapImage backimagesource = new BitmapImage();
                backimagesource.BeginInit();
                backimagesource.UriSource = new System.Uri(folder + "images\\" + point.Name + ".jpg");
                backimagesource.EndInit();
                pointImage.Source = backimagesource;
            }
            catch (Exception)
            {
            }
            name.Content= point.Name;
            minDepth.Text = point.MinNeedleDepth.ToString();
            maxDepth.Text = point.MaxNeedleDepth.ToString();
            place.Text = point.Position;
            note.Text = point.Note;
            comment1.Text = point.Comment1;
            comment2.Text = point.Comment2;
        }

        private void Points_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = (ListViewItem)pointsList.SelectedItem;
            if (item == null)
                return;
            SetAll((Database.Point)item.DataContext);
        }

        private void SetAll(Database.Point point)
        {
            new PointInfo(point).Show();
        }

        private void SyptomTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)syptomTreeView.SelectedItem;
            if (item == null)
                return;
            if (item.DataContext.GetType() == typeof(ConnectionValue<Database.Point>))
            {
                ConnectionValue<Database.Point> con = (ConnectionValue<Database.Point>)(item.DataContext);
                SetAll(con.Value);
            }
        }

        private void SaveData()
        {
            point.Comment1 = comment1.Text;
            point.Comment2 = comment2.Text;
            point.MaxNeedleDepth = int.Parse(maxDepth.Text);
            point.MinNeedleDepth = int.Parse(minDepth.Text);
            point.Note = note.Text;
            point.Position = place.Text;
            DatabaseConnection.Instance.UpdatePoint(point);
            for (int i = 0; i < symptomToAdd.Count; i++)
            {
                int j = 0;
                while (j < symptomToRemove.Count)
                {
                    if (symptomToAdd[i].Value.Equals(symptomToRemove[j]))
                    {
                        symptomToRemove.RemoveAt(j);
                        symptomToAdd.RemoveAt(i);
                        continue;
                    }
                    j++;
                }
            }
            for (int i = 0; i < symptomToAdd.Count; i++)
                DatabaseConnection.Instance.InsertSymptomPointRelation(symptomToAdd[i].Value, point, symptomToAdd[i].Importance, symptomToAdd[i].Comment);
            for (int i = 0; i < symptomToRemove.Count; i++)
                DatabaseConnection.Instance.DeleteSymptomPointRelation(symptomToRemove[i], point);
        }

        private void Censel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
        }

        private void PointImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new FullWindowPic(pointImage.Source, (int)(pointImage.ActualWidth * 2), (int)(pointImage.ActualHeight * 2)).Show();
        }

        private void PointsSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Database.Point> points = DatabaseConnection.Instance.FindPoint(pointsSearch.Text);
            pointsList.Items.Clear();
            for (int i = 0; i < points.Count; i++)
            {
                pointsList.Items.Add(new ListViewItem() { Content = points[i].ToString(), DataContext = points[i] });
            }
        }

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
            symptomSearchList.Items.Clear();
            for (int i = 0; i < sl.Count; i++)
                symptomSearchList.Items.Add(new ListViewItem() { Content = sl[i].ToString(), DataContext = sl[i] });
        }

        private void SelectSymptom()
        {
            ListViewItem item = (ListViewItem)symptomSearchList.SelectedItem;
            if (item == null)
                return;
            for (int i = 0; i < syptomTreeView.Items.Count; i++)
                if (item.DataContext.Equals(((TreeViewItem)syptomTreeView.Items[i]).DataContext))
                    return;

            symptomToAdd.Add(new ConnectionValue<Symptom>((Symptom)item.DataContext, symptomSearchImportece.SelectedIndex, ""));
            AddItemToSymptomTree((Symptom)item.DataContext);
            SetSymptomSearchListVisability(false);
        }

        private void AddItemToSymptomTree(Symptom sym)
        {
            TreeViewItem symptom = new TreeViewItem() { Header = sym.ToString(), DataContext = sym };
            List<ConnectionValue<Database.Point>> pointsRelatedToSymptom = DatabaseConnection.Instance.GetAllPointRelativeToSymptom(sym);
            for (int j = 0; j < pointsRelatedToSymptom.Count; j++)
            {
                symptom.Items.Add(new TreeViewItem()
                {
                    Header = pointsRelatedToSymptom[j].ToString(),
                    DataContext = pointsRelatedToSymptom[j],
                    Foreground = new SolidColorBrush()
                    {
                        Color = DatabaseConnection.Instance.GetLevel(pointsRelatedToSymptom[j].Importance)
                    }
                });
            }

            List<ConnectionValue<Channel>> channels = DatabaseConnection.Instance.GetAllChannelRelativeToSymptom(sym);
            for (int j = 0; j < channels.Count; j++)
            {
                symptom.Items.Add(new TreeViewItem()
                {
                    Header = channels[j].ToString(),
                    DataContext = channels[j],
                    Foreground = new SolidColorBrush()
                    {
                        Color = DatabaseConnection.Instance.GetLevel(channels[j].Importance)
                    }
                });
            }
            syptomTreeView.Items.Add(symptom);
        }

        private void SymptomSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadSymptomSearchList();
        }

        private void SymptomSearchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SelectSymptom();
        }

        private void SymptomSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectSymptom();
        }

        private void DeleteSymptom_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)syptomTreeView.SelectedItem;
            if (item == null)
                return;
            while (item.Parent.GetType() == typeof(TreeViewItem))
                item = (TreeViewItem)item.Parent;
            symptomToRemove.Add((Symptom)item.DataContext);
            syptomTreeView.Items.Remove(item);
        }

        private void SymptomSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            SetSymptomSearchListVisability(true);
        }

        private void SymptomSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            SetSymptomSearchListVisability(false);
        }

        private void SymptomSearchList_GotFocus(object sender, RoutedEventArgs e)
        {
            SetSymptomSearchListVisability(true);
        }

        private void SymptomSearchList_LostFocus(object sender, RoutedEventArgs e)
        {
            SetSymptomSearchListVisability(false);
        }
    }
}
