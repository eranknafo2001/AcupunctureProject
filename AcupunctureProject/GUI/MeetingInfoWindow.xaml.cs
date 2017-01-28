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
    /// Interaction logic for MeetingInfoWindow.xaml
    /// </summary>
    public partial class MeetingInfoWindow : Window
    {
        public MeetingInfoWindow(Meeting meeting, MeetingListByPatient meetingList)
        {
            InitializeComponent();
            Content = new MeetingInfoPage(this, meeting, meetingList);
        }
    }
}
