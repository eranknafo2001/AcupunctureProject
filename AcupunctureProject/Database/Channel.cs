using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AcupunctureProject.Database
{
	public class Channel : ITable
	{
		[PrimaryKey]
		public int Id { get; set; }
		[Unique,NotNull]
		public string Name { get; set; }
		[Unique, NotNull]
		public string Rt { get; set; }
		public int MainPoint { get; set; }
		public int EvenPoint { get; set; }
		public string Path { get; set; }
		public string Role { get; set; }
		public string Comments { get; set; }
		[ManyToMany(typeof(ChannelSymptom))]
		public List<Symptom> Symptoms { get; set; }
		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<ChannelSymptom> SymptomConnections { get; set; }
		[ManyToMany(typeof(ChannelMeeting))]
		public List<Meeting> Meetings { get; set; }

		public override string ToString()
		{
			return Id + " - " + Rt + " (" + Name + ")";
		}
	}
}
