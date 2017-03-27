using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ex = Bytescout.Spreadsheet;
using static System.Math;
using System.Windows;

namespace AcupunctureProject.GUI
{
    public class ExcelGrid : Grid
    {
        private string _Path;
        public string Path
        {
            get => _Path;
            set
            {
                _Path = value;
                Rephresh();
            }
        }

        public void Rephresh()
        {
            if (_Path != null)
                Rephresh(Path);
        }

        public void Rephresh(string p)
        {
            var s = new ex.Spreadsheet();
            s.LoadFromFile(p);
            var w = s.Worksheet(0);
            for (int i = 0; i <= w.NotEmptyRowMax; i++)
                RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            for (int i = 0; i <= w.NotEmptyColumnMax; i++)
                ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
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
                            SetRow(l, i);
                            SetColumn(l, j);
                            Children.Add(l);
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
                                SetRow(l, i);
                                SetRowSpan(l, Abs(cell.MergedWith.TopRowIndex - cell.MergedWith.BottomRowIndex) + 1);
                                SetColumn(l, j);
                                SetColumnSpan(l, Abs(cell.MergedWith.RightColumnIndex - cell.MergedWith.LeftColumnIndex) + 1);
                                Children.Add(l);
                            }
                        }
                    }
                }
            }

        }
    }
}
