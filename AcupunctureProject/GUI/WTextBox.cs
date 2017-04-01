using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AcupunctureProject.GUI
{
    class BTextBox : TextBox
    {
        public BTextBox()
        {
            TextWrapping = TextWrapping.WrapWithOverflow;
            KeyDown += new KeyEventHandler((i, e) =>
              {
                  if (e.Key.Equals(Key.Enter))
                  {
                      int temp = SelectionStart;
                      Text = Text.Remove(temp, SelectionLength);
                      Text = Text.Insert(temp, "\n");
                      SelectionStart = temp + 1;
                  }
              });
        }
    }
}
