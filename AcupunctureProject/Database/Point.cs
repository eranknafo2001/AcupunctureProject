using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AcupunctureProject.Database
{
	public class Point : ITable
	{
		[PrimaryKey, Unique, AutoIncrement]
		public int Id { get; set; }
		[Unique, NotNull]
		public string Name { get; set; }
		public int MinNeedleDepth { get; set; }
		public int MaxNeedleDepth { get; set; }
		public string NeedleDescription { get; set; }
		public string Position { get; set; }
		public int Importance { get; set; }
		public string Comment1 { get; set; }
		public string Comment2 { get; set; }
		public string Note { get; set; }
		public string Image { get; set; }

		[ManyToMany(typeof(SymptomPoint))]
		public List<Symptom> Symptoms { get; set; }

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<SymptomPoint> SymptomConnections { get; set; }

		[ManyToMany(typeof(MeetingPoint))]
		public List<Meeting> Meetings { get; set; }

		public override string ToString() => Name;
	}
}
