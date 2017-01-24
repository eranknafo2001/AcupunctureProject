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
    /// Interaction logic for PointInfo.xaml
    /// </summary>
    public partial class PointInfo : Window
    {
        private database.Point point;

        public PointInfo(database.Point point)
        {
            InitializeComponent();
            Title += point.Name;
            this.point = point;
            List<database.Point> pointList = Database.Instance.GetAllPoints();
            for (int i = 0; i < pointList.Count; i++)
                points.Items.Add(new ListViewItem() { Content = pointList[i].ToString(), DataContext = pointList[i] });
            syptomTreeView.Items.Clear();
            List<ConnectionValue<Symptom>> symptomList = Database.Instance.GetAllSymptomRelativeToPoint(point);
            for (int i = 0; i < symptomList.Count; i++)
            {
                TreeViewItem symptom = new TreeViewItem() { Header = symptomList[i].Value.ToString(), DataContext = symptomList[i].Value };
                List<ConnectionValue<database.Point>> pointsRelatedToSymptom = Database.Instance.GetAllPointRelativeToSymptom(symptomList[i].Value);
                for (int j = 0; j < pointsRelatedToSymptom.Count; j++)
                {
                    symptom.Items.Add(new TreeViewItem()
                    {
                        Header = pointsRelatedToSymptom[j].ToString(),
                        DataContext = pointsRelatedToSymptom[j],
                        Foreground = new SolidColorBrush()
                        {
                            Color = Database.Instance.GetLevel(pointsRelatedToSymptom[j].Importance)
                        }
                    });
                }

                List<ConnectionValue<Channel>> channels = Database.Instance.GetAllChannelRelativeToSymptom(symptomList[i].Value);
                for (int j = 0; j < channels.Count; j++)
                {
                    symptom.Items.Add(new TreeViewItem()
                    {
                        Header = channels[j].ToString(),
                        DataContext = channels[j],
                        Foreground = new SolidColorBrush()
                        {
                            Color = Database.Instance.GetLevel(channels[j].Importance)
                        }
                    });
                }
                syptomTreeView.Items.Add(symptom);
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
            name.Text = point.Name;
            minDepth.Text = point.MinNeedleDepth.ToString();
            maxDepth.Text = point.MaxNeedleDepth.ToString();
            place.Text = point.Position;
            note.Text = point.Note;
            comment1.Text = point.Comment1;
            comment2.Text = point.Comment2;
        }

        private void Points_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = (ListViewItem)points.SelectedItem;
            SetAll((database.Point)item.DataContext);
        }

        private void SetAll(database.Point point)
        {
            new PointInfo(point).Show();
        }

        private void SyptomTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)syptomTreeView.SelectedItem;
            if (item == null)
                return;
            if (item.DataContext.GetType() == typeof(ConnectionValue<database.Point>))
            {
                ConnectionValue<database.Point> con = (ConnectionValue<database.Point>)(item.DataContext);
                SetAll(con.Value);
            }
        }

        private void SaveData()
        {
            point.Comment1 = comment1.Text;
            point.Comment2 = comment2.Text;
            point.Name = name.Text;
            point.MaxNeedleDepth = int.Parse(maxDepth.Text);
            point.MinNeedleDepth = int.Parse(minDepth.Text);
            point.Note = note.Text;
            point.Position = place.Text;
            Database.Instance.UpdatePoint(point);
            points.Items.Clear();
            List<database.Point> pointList = Database.Instance.GetAllPoints();
            for (int i = 0; i < pointList.Count; i++)
                points.Items.Add(new ListViewItem() { Content = pointList[i].ToString(), DataContext = pointList[i] });
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
            new FullWindowPic(pointImage.Source).Show();
        }
    }
}
