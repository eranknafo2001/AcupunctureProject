using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AcupunctureProject.Database
{
	public static partial class Ex
	{
		public static string MyToString(this ResultValue r)
		{
			switch (r)
			{
				case ResultValue.NOT_SET:
					return "לא מוגדר";
				case ResultValue.BETTER:
					return "יותר טוב";
				case ResultValue.NO_CHANGE:
					return "אין שינוי";
				case ResultValue.WORSE:
					return "החמיר";
				default:
					return null;
			}
		}
	}
	public enum ResultValue
	{
		NOT_SET, BETTER, WORSE, NO_CHANGE
	}
	public class Meeting : ITable
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[ForeignKey(typeof(Patient))]
		public int PatientId { get; set; }
		public string Purpose { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public string Summery { get; set; }
		public string ResultDescription { get; set; }
		public ResultValue Result { get; set; }
		[ForeignKey(typeof(Diagnostic))]
		public int DiagnosticId { get; set; }
		[ManyToOne]
		public Diagnostic Diagnostic { get; set; }
		[ManyToOne]
		public Patient Patient { get; set; }
		[ManyToMany(typeof(MeetingSymptom))]
		public List<Symptom> Symptoms { get; set; }
		[ManyToMany(typeof(MeetingPoint))]
		public List<Point> Points { get; set; }
		[ManyToMany(typeof(ChannelMeeting))]
		public List<Channel> Channels { get; set; }
		[ManyToMany(typeof(TreatmentsMeetings))]
		public List<Treatment> Treatments { get; set; }

		public override int GetHashCode() => Id.GetHashCode();
	}
}
