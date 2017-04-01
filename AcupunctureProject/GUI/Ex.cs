using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AcupunctureProject.GUI
{
    public static class Ex
    {
        public static List<T> ToList<T>(this ItemCollection i, Func<object, T> f)
        {
            var o = new List<T>();
            foreach (var k in i)
            {
                o.Add(f(k));
            }
            return o;
        }

        public static List<K> ToList<T, K>(this IEnumerable<T> i, Func<T, K> f)
        {
            var o = new List<K>();
            foreach (var k in i)
                o.Add(f(k));
            return o;
        }

        public static List<object> ToList(this ItemCollection i)
        {
            var o = new List<object>();
            foreach (var k in i)
                o.Add(k);
            return o;
        }
    }
}
