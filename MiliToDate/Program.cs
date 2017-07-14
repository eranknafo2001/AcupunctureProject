using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiliToDate
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("enter the date in mili: ");
			var date = long.Parse(Console.ReadLine());
			Console.WriteLine($"the date no utc: {DateTime.FromFileTimeUtc(DateTime.FromFileTime(date).ToFileTime())}");
			Console.WriteLine($"the date utc: {DateTime.FromFileTimeUtc(date).ToString()}");
			Console.ReadKey();
		}
	}
}
