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

namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for newMeeting.xaml
    /// </summary>
    public partial class NewMeeting : Page
    {
        private Window perent;

        public NewMeeting(Window perent)
        {
            InitializeComponent();
            this.perent = perent;
        }

        private void symptomSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            symptomSearchList.Items.Clear();
            symptomSearchList.Items.Add(new ListBoxItem() { Content = "te1st", ContentStringFormat = "" });
        }

        private void censel_Click(object sender, RoutedEventArgs e)
        {
            perent.Content = new Main(perent);
        }

        private void patientSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            patientSearchList.Items.Clear();
            patientSearchList.Items.Add(new ListViewItem() { Content = "te1st", ContentStringFormat = "" });
        }

        private void patientSearchTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            patientSearchList.Visibility = Visibility.Hidden;
        }

        private void patientSearchTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            patientSearchList.Margin = new Thickness(symptomSearch.Margin.Left, symptomSearch.Margin.Top + symptomSearch.ExtentHeight, 0, 0);
            patientSearchList.Visibility = Visibility.Visible;
        }

        private void symptomSearch_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            symptomSearchList.Margin = new Thickness(symptomSearch.Margin.Left, symptomSearch.Margin.Top + symptomSearch.ExtentHeight, 0, 0);
            symptomSearchList.Visibility = Visibility.Visible;
        }

        private void symptomSearch_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            symptomSearchList.Visibility = Visibility.Hidden;
        }
    }
}
