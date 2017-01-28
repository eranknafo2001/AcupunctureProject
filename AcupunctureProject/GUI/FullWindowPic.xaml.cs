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
    /// Interaction logic for FullWindowPic.xaml
    /// </summary>
    public partial class FullWindowPic : Window
    {
        public FullWindowPic(ImageSource source, int width, int height)
        {
            InitializeComponent();
            image.Source = source;
            Width = width;
            Height = height;
        }
    }
}
