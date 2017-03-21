using System;
using System.Collections.Generic;
using ex = Bytescout.Spreadsheet;
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
using DTreatment = AcupunctureProject.Database.Treatment;
using static System.Math;
using AcupunctureProject.Database;


namespace AcupunctureProject.GUI
{
    /// <summary>
    /// Interaction logic for Treatment.xaml
    /// </summary>
    public partial class Treatment : Window
    {
        public Treatment(DTreatment treatment)
        {
            InitializeComponent();
            var s = new ex.Spreadsheet();
            s.LoadFromFile(treatment.Path);
            var w = s.Worksheet(0);
            for (int i = 0; i <= w.NotEmptyRowMax; i++)
                WinGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            for (int i = 0; i <= w.NotEmptyColumnMax; i++)
                WinGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            var done = new bool[w.NotEmptyRowMax + 1, w.NotEmptyColumnMax + 1];
            for (int i = 0; i <= w.NotEmptyRowMax; i++)
                for (int j = 0; j <= w.NotEmptyColumnMax; j++)
                    done[i, j] = false;
            for (int i = 0; i <= w.NotEmptyRowMax; i++)
            {
                for (int j = 0; j <= w.NotEmptyColumnMax; j++)
                {
                    if (!done[i, j])
                    {
                        var cell = w.Cell(i, j);
                        if (!cell.Merged)
                        {
                            var l = new TextBox();
                            if (cell.Value != null)
                                l.AppendText(cell.Value.ToString());
                            l.IsReadOnly = true;
                            l.TextAlignment = TextAlignment.Center;
                            l.TextWrapping = TextWrapping.Wrap;
                            Grid.SetRow(l, i);
                            Grid.SetColumn(l, j);
                            WinGrid.Children.Add(l);
                        }
                        else
                        {
                            for (int k = cell.MergedWith.TopRowIndex; k < cell.MergedWith.BottomRowIndex; k++)
                                for (int d = cell.MergedWith.LeftColumnIndex; d < cell.MergedWith.RightColumnIndex; d++)
                                    done[k, d] = true;
                            var l = new TextBox();
                            if (cell.Value != null)
                            {
                                l.AppendText(cell.Value.ToString());
                                l.IsReadOnly = true;
                                l.TextAlignment = TextAlignment.Center;
                                l.TextWrapping = TextWrapping.Wrap;
                                Grid.SetRow(l, i);
                                Grid.SetRowSpan(l, Abs(cell.MergedWith.TopRowIndex - cell.MergedWith.BottomRowIndex) + 1);
                                Grid.SetColumn(l, j);
                                Grid.SetColumnSpan(l, Abs(cell.MergedWith.RightColumnIndex - cell.MergedWith.LeftColumnIndex) + 1);
                                WinGrid.Children.Add(l);
                            }
                        }
                    }
                }
            }

        }
    }
}
