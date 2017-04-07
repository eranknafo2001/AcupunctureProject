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


namespace System
{
	public delegate void VoidFunc<in T>(T i1);
	public delegate void VoidFunc<in T, in T2>(T i1,T2 i2);
	public delegate void VoidFunc<in T, in T2, in T3>(T i1, T2 i2, T3 i3);
	public delegate void VoidFunc<in T, in T2, in T3, in T4>(T i1, T2 i2, T3 i3,T4 i4);
	public delegate void VoidFunc<in T, in T2, in T3, in T4, in T5>(T i1, T2 i2, T3 i3,T4 i4,T5 i5);
	public delegate void VoidFunc<in T, in T2, in T3, in T4, in T5, in T6>(T i1, T2 i2, T3 i3, T4 i4, T5 i5,T6 i6);
	public delegate void VoidFunc<in T, in T2, in T3, in T4, in T5, in T6, in T7>(T i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7);
}