using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using MColor = System.Windows.Media.Color;
namespace AcupunctureProject.Database
{
	public class Color : ITable
	{
		[PrimaryKey, Unique]
		public int Id { get; set; }
		public byte R { get; set; }
		public byte G { get; set; }
		public byte B { get; set; }

		public MColor GetColor()
		{
			return new MColor() { R = R, G = G, B = B, A = 255 };
		}
	}
}
