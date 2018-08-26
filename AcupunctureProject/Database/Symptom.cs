using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AcupunctureProject.Database
{
	public class Symptom : ITable
	{
		[PrimaryKey, Unique, AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Comment { get; set; }
		[ManyToMany(typeof(SymptomPoint))]
		public List<Point> Points { get; set; }
		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<SymptomPoint> PointsConnections { get; set; }
		[ManyToMany(typeof(ChannelSymptom))]
		public List<Channel> Channels { get; set; }
		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<ChannelSymptom> ChannelConnections { get; set; }
		[ManyToMany(typeof(MeetingSymptom))]
		public List<Meeting> Meetings { get; set; }

		public override string ToString() => Name;
	}
}
