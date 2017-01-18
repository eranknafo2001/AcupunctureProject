
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
            this.point = point;
            List<database.Point> pointList = Database.Instance.getAllPoints();
            for (int i = 0; i < pointList.Count; i++)
                points.Items.Add(new ListViewItem() { Content = pointList[i].ToString(), DataContext = pointList[i] });
            setAll(point);
        }

        private void points_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = (ListViewItem)points.SelectedItem;
            setAll((database.Point)item.DataContext);
        }

        private void setAll(database.Point point)
        {
            this.point = point;
            syptomTreeView.Items.Clear();
            List<ConnectionValue<Symptom>> symptomList = Database.Instance.getAllSymptomRelativeToPoint(point);
            for (int i = 0; i < symptomList.Count; i++)
            {
                TreeViewItem symptom = new TreeViewItem() { Header = symptomList[i].Value.ToString(), DataContext = symptomList[i].Value };
                List<ConnectionValue<database.Point>> pointsRelatedToSymptom = Database.Instance.getAllPointRelativeToSymptom(symptomList[i].Value);
                for (int j = 0; j < pointsRelatedToSymptom.Count; j++)
                {
                    symptom.Items.Add(new TreeViewItem() { Header = pointsRelatedToSymptom[j].Value.ToString(), DataContext = pointsRelatedToSymptom[j].Value });
                }
                syptomTreeView.Items.Add(symptom);
            }
            string[] tempFolder = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');
            var folder = "";
            for (int i = 0; i < tempFolder.Length - 1; i++)
            {
                folder += tempFolder[i] + "\\";
            }
            BitmapImage backimagesource = new BitmapImage();
            backimagesource.BeginInit();
            backimagesource.UriSource = new System.Uri(folder + "images\\" + point.Name + ".jpg");
            backimagesource.EndInit();
            pointImage.Source = backimagesource;
            name.Text = point.Name;
            minDepth.Text = point.MinNeedleDepth.ToString();
            maxDepth.Text = point.MaxNeedleDepth.ToString();
            place.Text = point.Position;
            note.Text = point.Note;
            comment1.Text = point.Comment1;
            comment2.Text = point.Comment2;
        }

        private void syptomTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)syptomTreeView.SelectedItem;
            if (item == null)
                return;
            if (item.DataContext.GetType() == typeof(database.Point))
            {
                setAll((database.Point)item.DataContext);
            }
        }
        
        private void saveData()
        {
            point.Comment1 = comment1.Text;
            point.Comment2 = comment2.Text;
            point.Name = name.Text;
            point.MaxNeedleDepth = int.Parse(maxDepth.Text);
            point.MinNeedleDepth = int.Parse(minDepth.Text);
            point.Note = note.Text;
            point.Position = place.Text;
            Database.Instance.updatePoint(point);

            List<database.Point> pointList = Database.Instance.getAllPoints();
            for (int i = 0; i < pointList.Count; i++)
                points.Items.Add(new ListViewItem() { Content = pointList[i].ToString(), DataContext = pointList[i] });
        }

        private void censel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void saveAndExit_Click(object sender, RoutedEventArgs e)
        {
            saveData();
            Close();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            saveData();
        }
    }
}
