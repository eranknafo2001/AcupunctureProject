
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
        public PointInfo(database.Point point)
        {
            InitializeComponent();
            List<database.Point> pointList = Database.Instance.getAllPoints();
            for (int i = 0; i < pointList.Count; i++)
                points.Items.Add(new ListViewItem() { Content = pointList[i] });
            List<ConnectionValue<Symptom>> symptomList = Database.Instance.getAllSymptomRelativeToPoint(point);
            for (int i = 0; i < symptomList.Count; i++)
            {
                TreeViewItem symptom = new TreeViewItem() { Name = symptomList[i].Value.ToString(), DataContext = symptomList[i].Value };
                List<ConnectionValue<database.Point>> pointsRelatedToSymptom = Database.Instance.getAllPointRelativeToSymptom(symptomList[i].Value);
                for (int j = 0; j < pointsRelatedToSymptom.Count; j++)
                {
                    symptom.Items.Add(new TreeViewItem() { Name = pointsRelatedToSymptom[j].Value.ToString(), DataContext = pointsRelatedToSymptom[j].Value });
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
    }
}
