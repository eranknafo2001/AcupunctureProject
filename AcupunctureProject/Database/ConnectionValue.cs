using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AcupunctureProject.Database
{
	public interface IConnectionValue : ITable
	{
		int Importance { get; set; }
		string Comment { get; set; }
	}

	public class SymptomPoint : IConnectionValue
	{
		[PrimaryKey, Unique, AutoIncrement]
		public int Id { get; set; }
		[ForeignKey(typeof(Symptom))]
		public int SymptomId { get; set; }
		[ManyToOne]
		public Symptom Symptom { get; set; }
		[ForeignKey(typeof(Point))]
		public int PointId { get; set; }
		[ManyToOne]
		public Point Point { get; set; }
		public int Importance { get; set; }
		public string Comment { get; set; }
	}
	public class ChannelSymptom : IConnectionValue
	{
		[PrimaryKey, Unique, AutoIncrement]
		public int Id { get; set; }
		[ForeignKey(typeof(Symptom))]
		public int SymptomId { get; set; }
		[ManyToOne]
		public Symptom Symptom { get; set; }
		[ForeignKey(typeof(Channel))]
		public int ChannelId { get; set; }
		[ManyToOne]
		public Channel Channel { get; set; }
		public int Importance { get; set; }
		public string Comment { get; set; }
	}
	public class MeetingPoint
	{
		[ForeignKey(typeof(Meeting))]
		public int MeetingId { get; set; }

		[ForeignKey(typeof(Point))]
		public int PointId { get; set; }
	}
	public class MeetingSymptom
	{
		[ForeignKey(typeof(Meeting))]
		public int MeetingId { get; set; }

		[ForeignKey(typeof(Symptom))]
		public int SymptomId { get; set; }
	}
	public class ChannelMeeting
	{
		[ForeignKey(typeof(Meeting))]
		public int MeetingId { get; set; }

		[ForeignKey(typeof(Channel))]
		public int ChannelId { get; set; }
	}

	public class TreatmentsMeetings
	{
		[ForeignKey(typeof(Meeting))]
		public int MeetingId { get; set; }

		[ForeignKey(typeof(Treatment))]
		public int TreatmentId { get; set; }
	}
}
