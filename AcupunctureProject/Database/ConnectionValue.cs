using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AcupunctureProject.Database
{
    public class SymptomPoint : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Symptom))]
        public int SymptomId { get; set; }
        [ForeignKey(typeof(Point))]
        public int PointId { get; set; }
        public int Imaportance { get; set; }
        public string Comment { get; set; }
    }
    public class ChannelSymptom : ITable
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Symptom))]
        public int SymptomId { get; set; }
        [ForeignKey(typeof(Channel))]
        public int ChannelId { get; set; }
        public int Imaportance { get; set; }
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
        [ForeignKey(typeof(Channel))]
        public int ChannelId { get; set; }

        [ForeignKey(typeof(Meeting))]
        public int Meeting { get; set; }
    }
}
