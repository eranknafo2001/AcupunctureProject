using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace AcupunctureProject.database
{
    public static class Extension
    {
        public static string GetStringL(this SQLiteDataReader reader, string str)
        {
            return reader[str].ToString();
        }

        public static DateTime GetDateTimeL(this SQLiteDataReader reader, string str)
        {
            return (DateTime)reader[str];
        }

        public static int GetIntL(this SQLiteDataReader reader, string str)
        {
            return (int)(Int64)reader[str];
        }
    }

    public class Database
    {
        private static readonly string ID = "ID";
        private static Database database = null;
        public static Database Instance
        {
            get
            {
                if (database == null)
                    database = new Database();
                return database;
            }
        }

        private SQLiteConnection connection;
        #region all SQLiteCommand declirations
        private SQLiteCommand getAllChannelRelativeToSymptomSt;
        private SQLiteCommand getAllPointRelativeToSymptomSt;
        private SQLiteCommand getAllSymptomRelativeToPointSt;

        private SQLiteCommand insertSymptomSt;
        private SQLiteCommand insertMeetingSt;
        private SQLiteCommand insertPatientSt;
        private SQLiteCommand insertPointsSt;
        private SQLiteCommand insertChannelSt;

        private SQLiteCommand updateSymptomSt;
        private SQLiteCommand updateMeetingSt;
        private SQLiteCommand updatePatientSt;
        private SQLiteCommand updatePointSt;
        private SQLiteCommand updateChannelSt;
        private SQLiteCommand updateChannelSymptomRelationSt;
        private SQLiteCommand updatePointSymptomRelationSt;

        private SQLiteCommand deleteSymptomSt;
        private SQLiteCommand deleteMeetingSt;
        private SQLiteCommand deletePatientSt;
        private SQLiteCommand deletePointSt;
        private SQLiteCommand deleteChannelSt;
        private SQLiteCommand deleteSymptomPointRelationSt;
        private SQLiteCommand deleteSymptomMeetingRelationSt;
        private SQLiteCommand deleteSymptomChannelRelationSt;
        private SQLiteCommand deleteMeetingPointSt;

        private SQLiteCommand insertSymptomPointRelationSt;
        private SQLiteCommand insertSymptomMeetingRelationSt;
        private SQLiteCommand insertMeetingPointRelationSt;
        private SQLiteCommand insertSymptomChannelRelationSt;

        private SQLiteCommand getSymptomSt;
        private SQLiteCommand getPointByNameSt;
        private SQLiteCommand getPointByIdSt;
        private SQLiteCommand getChannelByIdSt;

        private SQLiteCommand findSymptomSt;
        private SQLiteCommand findPatientSt;

        private SQLiteCommand getAllMeetingsRelativeToSymptomsSt;

        private SQLiteCommand getAllPointsSt;
        #endregion
        public Database()
        {
            string[] tempFolder = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');
            string folder = "";
            for (int i = 0; i < tempFolder.Length - 1; i++)
            {
                folder += tempFolder[i] + "\\";
            }
            SQLiteConnectionStringBuilder d = new SQLiteConnectionStringBuilder();
            d.DataSource = folder + "database.db";
            connection = new SQLiteConnection(d.ConnectionString);
            //connection = new SQLiteConnection("Data Source=" + folder + "database.db;Version=3;");
            connection.Open();

            getAllPointRelativeToSymptomSt = new SQLiteCommand("select POINTS.* , SYMPTOM_POINTS.IMPORTENCE , SYMPTOM_POINTS.COMMENT from POINTS INNER JOIN SYMPTOM_POINTS ON POINTS.ID = SYMPTOM_POINTS.POINT_ID and SYMPTOM_POINTS.SYMPTOM_ID = @symptomId order by SYMPTOM_POINTS.IMPORTENCE DESC;", connection);
            getAllPointRelativeToSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));

            getAllSymptomRelativeToPointSt = new SQLiteCommand("select SYMPTOM.* , SYMPTOM_POINTS.IMPORTENCE , SYMPTOM_POINTS.COMMENT from SYMPTOM INNER JOIN SYMPTOM_POINTS ON SYMPTOM.ID = SYMPTOM_POINTS.SYMPTOM_ID where SYMPTOM_POINTS.POINT_ID = @pointId order by SYMPTOM_POINTS.IMPORTENCE DESC;", connection);
            getAllSymptomRelativeToPointSt.Parameters.Add(new SQLiteParameter("@pointId"));

            getAllChannelRelativeToSymptomSt = new SQLiteCommand("SELECT CHANNEL.* , SYMPTOM_CHANNEL.IMPORTENCE , SYMPTOM_CHANNEL.COMMENT from CHANNEL INNER JOIN SYMPTOM_CHANNEL ON CHANNEL.ID = SYMPTOM_CHANNEL.CHANNEL_ID where SYMPTOM_CHANNEL.SYMPTOM_ID = @symptomId order by SYMPTOM_CHANNEL.IMPORTENCE DESC;", connection);
            getAllChannelRelativeToSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));


            insertSymptomSt = new SQLiteCommand("insert into SYMPTOM(NAME,COMMENT) values(@name,@comment);", connection);
            insertSymptomSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@comment") });

            insertMeetingSt = new SQLiteCommand("insert into MEETING(PATIENT_ID,PURPOSE,DATE,DESCRIPTION,SUMMERY,RESULT_DESCRIPTION,RESULT_VALUE) values(@patintId,@purpose,@date,@description,@summery,@resultDescription,@resultValue);", connection);
            insertMeetingSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@patientId"), new SQLiteParameter("@purpose"), new SQLiteParameter("@date"), new SQLiteParameter("@description"), new SQLiteParameter("@summery"), new SQLiteParameter("@resultDescription"), new SQLiteParameter("@resultValue") });

            insertPatientSt = new SQLiteCommand("insert into PATIENT(NAME,TELEPHONE,CELLPHONE,BIRTHDAY,GENDER,ADDRESS,EMAIL,MEDICAL_DESCRIPTION) values(@name,@telephone,@cellphone,@birthday,@gender,@address,@email,@medicalDescription);", connection);
            insertPatientSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@telephone"), new SQLiteParameter("@cellphone"), new SQLiteParameter("@birthday"), new SQLiteParameter("@gender"), new SQLiteParameter("@address"), new SQLiteParameter("@email"), new SQLiteParameter("@medicalDescription") });

            insertPointsSt = new SQLiteCommand("insert into POINTS(NAME,MIN_NEEDLE_DEPTH,MAX_NEEDLE_DEPTH,POSITION,IMPORTENCE,COMMENT1,COMMENT2,NOTE,IMAGE) values(@name,@minNeedleDepth,@maxNeedleDepth,@position,@importance,@comment1,@comment2,@note,@image);", connection);
            insertPointsSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@minNeedleDepth"), new SQLiteParameter("@maxNeedleDepth"), new SQLiteParameter("@position"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment1"), new SQLiteParameter("@comment2"), new SQLiteParameter("@note"), new SQLiteParameter("@image") });

            insertSymptomPointRelationSt = new SQLiteCommand("insert into SYMPTOM_POINTS values(@symptomId,@pointId,@importance,@comment);", connection);
            insertSymptomPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });

            insertSymptomMeetingRelationSt = new SQLiteCommand("insert into MEETING_SYMPTOM(MEETING_ID,SYMPTOM_ID) values(@meetingId,@symptomId);", connection);
            insertSymptomMeetingRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@meeting"), new SQLiteParameter("@symptomId") });

            insertMeetingPointRelationSt = new SQLiteCommand("insert into MEETING_POINTS(MEETING_ID,POINT_ID) values(@meetingId,@pointId);", connection);
            insertMeetingPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@meetingId"), new SQLiteParameter("@pointId") });

            insertChannelSt = new SQLiteCommand("insert into CHANNEL values(@channelId,@name,@rt,@mainPoint,@evenPoint,@path,@role,@comments);", connection);
            insertChannelSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@channelId"), new SQLiteParameter("@name"), new SQLiteParameter("@rt"), new SQLiteParameter("@mainPoint"), new SQLiteParameter("@evenPoint"), new SQLiteParameter("@path"), new SQLiteParameter("@role"), new SQLiteParameter("@comments") });

            insertSymptomChannelRelationSt = new SQLiteCommand("insert into SYMPTOM_CHANNEL values(@symptomId,@channelId,@importance,@comment);", connection);
            insertSymptomChannelRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@channelId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });


            updateSymptomSt = new SQLiteCommand("update SYMPTOM set NAME = @name ,COMMENT = @comment where ID = @symptomId;", connection);
            updateSymptomSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@comment") });

            updateMeetingSt = new SQLiteCommand("update MEETING set PATIENT_ID = @patientId ,PURPOSE = @purpose ,DATE = @date ,DESCRIPTION = @description ,SUMMERY = @summery ,RESULT_DESCRIPTION = @resultDescription ,RESULT_VALUE = @resultValue where ID = @meetingId;", connection);
            updateMeetingSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@patientId"), new SQLiteParameter("@purpose"), new SQLiteParameter("@date"), new SQLiteParameter("@description"), new SQLiteParameter("@summery"), new SQLiteParameter("@resultDescription"), new SQLiteParameter("@resultValue") });

            updatePatientSt = new SQLiteCommand("update PATIENT set NAME = @name ,TELEPHONE = @telephone ,CELLPHONE = @cellphone ,BIRTHDAY = @birthday ,GENDER = @gander ,ADDRESS = @address ,EMAIL = @email ,MEDICAL_DESCRIPTION = @medicalDescription where ID = @patientId;", connection);
            updatePatientSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@telephone"), new SQLiteParameter("@cellphone"), new SQLiteParameter("@birthday"), new SQLiteParameter("@gender"), new SQLiteParameter("@address"), new SQLiteParameter("@email"), new SQLiteParameter("@medicalDescription") });

            updatePointSt = new SQLiteCommand("update POINTS set NAME = @name ,MIN_NEEDLE_DEPTH = @minNeedleDepth ,MAX_NEEDLE_DEPTH = @maxNeedleDepth ,POSITION = @position ,IMPORTENCE = @importance ,COMMENT1 = @comment1 ,COMMENT2 = @comment2,NOTE = @note,IMAGE = @image where ID = @pointId;", connection);
            updatePointSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@minNeedleDepth"), new SQLiteParameter("@maxNeedleDepth"), new SQLiteParameter("@position"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment1"), new SQLiteParameter("@comment2"), new SQLiteParameter("@note"), new SQLiteParameter("@image"), new SQLiteParameter("@pointId") });

            updateChannelSt = new SQLiteCommand("update CHANNEL set NAME = @name, RT = @rt ,MAIN_POINT = @mainPoint ,EVEN_POINT = @evenPoint ,PATH = @path ,ROLE = @role ,COMMENT = @comment where ID = @channelId;", connection);
            updateChannelSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@channelId"), new SQLiteParameter("@name"), new SQLiteParameter("@rt"), new SQLiteParameter("@mainPoint"), new SQLiteParameter("@evenPoint"), new SQLiteParameter("@path"), new SQLiteParameter("@role"), new SQLiteParameter("@comments") });

            updateChannelSymptomRelationSt = new SQLiteCommand("update SYMPTOM_CHANNEL set IMPORTENCE = @importance ,COMMENT = @comment where CHANNEL_ID = @channelId and SYMPTOM_ID = @symptomId;", connection);
            updateChannelSymptomRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@channelId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });

            updatePointSymptomRelationSt = new SQLiteCommand("update SYMPTOM_POINTS set IMPORTENCE = @importance ,COMMENT = @comment where POINT_ID = @pointId and SYMPTOM_ID = @symptomId;", connection);
            updatePointSymptomRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });


            deleteSymptomSt = new SQLiteCommand("delete from SYMPTOM where ID = @symptomId;", connection);
            deleteSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));

            deleteMeetingSt = new SQLiteCommand("delete from MEETING where ID = @meetingId;", connection);
            deleteMeetingSt.Parameters.Add(new SQLiteParameter("@meetingId"));

            deletePatientSt = new SQLiteCommand("delete from PATIENT where ID = @patientId;", connection);
            deletePatientSt.Parameters.Add(new SQLiteParameter("@patientId"));

            deletePointSt = new SQLiteCommand("delete from POINTS where ID = @pointId;", connection);
            deletePointSt.Parameters.Add(new SQLiteParameter("@pointId"));

            deleteChannelSt = new SQLiteCommand("delete from CHANNEL where ID = @channelId;", connection);
            deleteChannelSt.Parameters.Add(new SQLiteParameter("@channelId"));

            deleteSymptomPointRelationSt = new SQLiteCommand("delete from SYMPTOM_POINTS where SYMPTOM_ID = @symptomId and POINT_ID = @pointId;", connection);
            deleteSymptomPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId") });

            deleteSymptomMeetingRelationSt = new SQLiteCommand("delete from MEETING_SYMPTOM where SYMPTOM_ID = @symptomId and MEETING_ID = @meetingId;", connection);
            deleteSymptomMeetingRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@meetingId") });

            deleteSymptomChannelRelationSt = new SQLiteCommand("delete from SYMPTOM_CHANNEL where SYMPTOM_ID = @symptomId and CHANNEL_ID = @channelId;", connection);
            deleteSymptomChannelRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@channelId") });

            deleteMeetingPointSt = new SQLiteCommand("delete from MEETING_POINTS where MEETING_ID = @meetingId and POINT_ID = @pointId;", connection);
            deleteMeetingPointSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@meetingId"), new SQLiteParameter("@pointId") });

            getSymptomSt = new SQLiteCommand("select * from SYMPTOM where NAME = @name;", connection);
            getSymptomSt.Parameters.Add(new SQLiteParameter("@name"));

            getPointByNameSt = new SQLiteCommand("select * from POINTS where NAME = @name;", connection);
            getPointByNameSt.Parameters.Add(new SQLiteParameter("@name"));

            getPointByIdSt = new SQLiteCommand("select * from POINTS where ID = @id;", connection);
            getPointByIdSt.Parameters.Add(new SQLiteParameter("@id"));

            getChannelByIdSt = new SQLiteCommand("select * from CHANNEL where ID = @id;", connection);
            getChannelByIdSt.Parameters.Add(new SQLiteParameter("@id"));

            getAllPointsSt = new SQLiteCommand("select * from POINTS;", connection);

            findSymptomSt = new SQLiteCommand("SELECT * FROM SYMPTOM where NAME like '%@name%';", connection);
            findSymptomSt.Parameters.Add(new SQLiteParameter("@name"));

            findPatientSt = new SQLiteCommand("SELECT * FROM PATIENT where NAME like '%@name%';", connection);
            findPatientSt.Parameters.Add(new SQLiteParameter("@name"));

            getAllMeetingsRelativeToSymptomsSt = new SQLiteCommand(connection);
        }

        #region updates
        public void updateSymptom(Symptom symptom)
        {
            updateSymptomSt.Parameters["@name"].Value = symptom.Name;
            updateSymptomSt.Parameters["@comment"].Value = symptom.Comment;
            updateSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            updateSymptomSt.ExecuteNonQuery();
        }

        public void updateMeeting(Meeting meeting)
        {
            updateMeetingSt.Parameters["@patientId"].Value = meeting.PatientId;
            updateMeetingSt.Parameters["@purpese"].Value = meeting.Purpose;
            updateMeetingSt.Parameters["@date"].Value = meeting.Date;
            updateMeetingSt.Parameters["@description"].Value = meeting.Description;
            updateMeetingSt.Parameters["@summery"].Value = meeting.Summery;
            updateMeetingSt.Parameters["@resultDecription"].Value = meeting.ResultDescription;
            updateMeetingSt.Parameters["@resultValue"].Value = meeting.Result.Value;
            updateMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            updateMeetingSt.ExecuteNonQuery();
        }

        public void updatePatient(Patient patient)
        {
            updatePatientSt.Parameters["@name"].Value = patient.Name;
            updatePatientSt.Parameters["@telephone"].Value = patient.Telephone;
            updatePatientSt.Parameters["@Cellphone"].Value = patient.Cellphone;
            updatePatientSt.Parameters["@birthday"].Value = patient.Birthday;
            updatePatientSt.Parameters["@gender"].Value = patient.Gend.Value;
            updatePatientSt.Parameters["@address"].Value = patient.Address;
            updatePatientSt.Parameters["@email"].Value = patient.Email;
            updatePatientSt.Parameters["@medicalDescription"].Value = patient.MedicalDescription;
            updatePatientSt.Parameters["@patientId"].Value = patient.Id;
            updatePatientSt.ExecuteNonQuery();
        }

        public void updatePoint(Point point)
        {
            updatePointSt.Parameters["@name"].Value = point.Name;
            updatePointSt.Parameters["@minNeedleDepth"].Value = point.MinNeedleDepth;
            updatePointSt.Parameters["@maxNeedleDepth"].Value = point.MaxNeedleDepth;
            updatePointSt.Parameters["@position"].Value = point.Position;
            updatePointSt.Parameters["@importance"].Value = point.Importance;
            updatePointSt.Parameters["@comment1"].Value = point.Comment1;
            updatePointSt.Parameters["@comment2"].Value = point.Comment2;
            updatePointSt.Parameters["@note"].Value = point.Note;
            updatePointSt.Parameters["@image"].Value = point.Image;
            updatePointSt.Parameters["@pointId"].Value = point.Id;
            updatePointSt.ExecuteNonQuery();
        }

        public void updateChannel(Channel channel)
        {
            updateChannelSt.Parameters["@name"].Value = channel.Name;
            updateChannelSt.Parameters["@rt"].Value = channel.Rt;
            updateChannelSt.Parameters["@mainPoint"].Value = channel.MainPoint;
            updateChannelSt.Parameters["@evenPoint"].Value = channel.EvenPoint;
            updateChannelSt.Parameters["@path"].Value = channel.Path;
            updateChannelSt.Parameters["@role"].Value = channel.Role;
            updateChannelSt.Parameters["@comments"].Value = channel.Comments;
            updateChannelSt.Parameters["@channelId"].Value = channel.Id;
            updateChannelSt.ExecuteNonQuery();
        }

        public void updateChannelSymptomRelation(Channel channel, Symptom symptom, int importance, string comment)
        {
            updateChannelSymptomRelationSt.Parameters["@importance"].Value = importance;
            updateChannelSymptomRelationSt.Parameters["@comment"].Value = comment;
            updateChannelSymptomRelationSt.Parameters["@channelId"].Value = channel.Id;
            updateChannelSymptomRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            updateChannelSymptomRelationSt.ExecuteNonQuery();
        }

        public void updatePointSymptomRelation(Point point, Symptom symptom, int importance, string comment)
        {
            updatePointSymptomRelationSt.Parameters["@importance"].Value = importance;
            updatePointSymptomRelationSt.Parameters["@comment"].Value = comment;
            updatePointSymptomRelationSt.Parameters["@pointId"].Value = point.Id;
            updatePointSymptomRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            updatePointSymptomRelationSt.ExecuteNonQuery();
        }
        #endregion
        #region deletes
        public void deleteSymptom(Symptom symptom)
        {
            deleteSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            deleteSymptomSt.ExecuteNonQuery();
        }

        public void deleteMeeting(Meeting meeting)
        {
            deleteMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            deleteMeetingSt.ExecuteNonQuery();
        }

        public void deletePatient(Patient patient)
        {
            deletePatientSt.Parameters["@patientId"].Value = patient.Id;
            deletePatientSt.ExecuteNonQuery();
        }

        public void deletePoint(Point point)
        {
            deletePointSt.Parameters["@pointId"].Value = point.Id;
            deletePointSt.ExecuteNonQuery();
        }

        public void deleteChannel(Channel channel)
        {
            deleteChannelSt.Parameters["@channelId"].Value = channel.Id;
            deleteChannelSt.ExecuteNonQuery();
        }

        public void deleteSymptomPointRelation(Symptom symptom, Point point)
        {
            deleteSymptomPointRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            deleteSymptomPointRelationSt.Parameters["@pointId"].Value = point.Id;
            deleteSymptomPointRelationSt.ExecuteNonQuery();
        }

        public void deleteSymptomMeetingRelation(Symptom symptom, Meeting meeting)
        {
            deleteSymptomMeetingRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            deleteSymptomMeetingRelationSt.Parameters["@meetingId"].Value = meeting.Id;
            deleteSymptomMeetingRelationSt.ExecuteNonQuery();
        }

        public void deleteSymptomChannelRelation(Symptom symptom, Channel channel)
        {
            deleteSymptomChannelRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            deleteSymptomChannelRelationSt.Parameters["@channelId"].Value = channel.Id;
            deleteSymptomChannelRelationSt.ExecuteNonQuery();
        }

        public void deleteMeetingPoint(Meeting meeting, Point point)
        {
            deleteMeetingPointSt.Parameters["@meetingId"].Value = meeting.Id;
            deleteMeetingPointSt.Parameters["@pointId"].Value = point.Id;
            deleteMeetingPointSt.ExecuteNonQuery();
        }
        #endregion
        #region finds objects
        public Channel getChannel(int id)
        {
            getChannelByIdSt.Parameters["@id"].Value = id;
            using (SQLiteDataReader rs = getChannelByIdSt.ExecuteReader())
            {
                if (rs.Read())
                    return getChannel(rs);
                return null;
            }
        }

        public Point getPoint(int id)
        {
            getPointByIdSt.Parameters["@id"].Value = id;
            using (SQLiteDataReader rs = getPointByIdSt.ExecuteReader())
            {
                if (rs.Read())
                    return getPoint(rs);
                return null;
            }
        }

        public List<Point> getAllPoints()
        {
            using (SQLiteDataReader rs = getAllPointsSt.ExecuteReader())
            {
                return getPoints(rs);
            }
        }

        public Point getPoint(string name)
        {
            getPointByNameSt.Parameters["@name"].Value = name;
            using (SQLiteDataReader rs = getPointByNameSt.ExecuteReader())
            {
                if (rs.Read())
                    return getPoint(rs);
                return null;
            }
        }

        public Symptom getSymptom(string name)
        {
            getSymptomSt.Parameters["@name"].Value = name;
            using (SQLiteDataReader rs = getSymptomSt.ExecuteReader())
            {
                if (rs.Read())
                    return getSymptom(rs);
                return null;
            }
        }

        public List<Symptom> findSymptom(string name)
        {
            findSymptomSt.Parameters["@name"].Value = name;
            using (SQLiteDataReader rs = findSymptomSt.ExecuteReader())
            {
                return getSymptoms(rs);
            }
        }

        public List<Patient> findPatient(string name)
        {
            findPatientSt.Parameters["@name"].Value = name;
            using (SQLiteDataReader rs = findPatientSt.ExecuteReader())
            {
                return getPatients(rs);
            }
        }

        public List<ConnectionValue<Channel>> getAllChannelRelativeToSymptom(Symptom symptom)
        {
            getAllChannelRelativeToSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            using (SQLiteDataReader rs = getAllChannelRelativeToSymptomSt.ExecuteReader())
            {
                List<ConnectionValue<Channel>> o = new List<ConnectionValue<Channel>>();
                while (rs.Read())
                    o.Add(new ConnectionValue<Channel>(getChannel(rs), rs.GetIntL(ConnectionValue<Channel>.IMPORTENCE),
                            rs.GetStringL(ConnectionValue<Channel>.COMMENT)));
                return o;
            }
        }

        public List<ConnectionValue<Point>> getAllPointRelativeToSymptom(Symptom symptom)
        {
            getAllPointRelativeToSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            using (SQLiteDataReader rs = getAllPointRelativeToSymptomSt.ExecuteReader())
            {
                List<ConnectionValue<Point>> o = new List<ConnectionValue<Point>>();
                while (rs.Read())
                    o.Add(new ConnectionValue<Point>(getPoint(rs), rs.GetIntL(ConnectionValue<Point>.IMPORTENCE),
                            rs.GetStringL(ConnectionValue<Point>.COMMENT)));
                return o;
            }
        }

        public List<ConnectionValue<Symptom>> getAllSymptomRelativeToPoint(Point point)
        {
            getAllSymptomRelativeToPointSt.Parameters["@pointId"].Value = point.Id;
            using (SQLiteDataReader rs = getAllSymptomRelativeToPointSt.ExecuteReader())
            {
                return getSymptomsConnection(rs);
            }
        }

        public List<Meeting> getAllMeetingsRelativeToSymptoms(List<Symptom> symptoms)
        {
            string sql = "select MEETING.*,count(MEETING.ID) from MEETING INNER JOIN MEETING_SYMPTOM ON MEETING.ID = MEETING_SYMPTOM.MEETING_ID where ";
            for (int i = 0; i < symptoms.Count; i++)
            {
                sql += " MEETING_SYMPTOM.SYMPTOM_ID = " + symptoms[i].Id;
                if (i < symptoms.Count - 1)
                    sql += " or ";
            }
            sql += " having count(MEETING.ID) > 0 order by count(MEETING.ID) DESC;";
            getAllMeetingsRelativeToSymptomsSt.CommandText = sql;
            using (SQLiteDataReader rs = getAllMeetingsRelativeToSymptomsSt.ExecuteReader())
            {
                return getMeetings(rs);
            }
        }
        #endregion
        #region inserts
        public Channel insertChannel(Channel channel)
        {
            insertChannelSt.Parameters["@channelId"].Value = channel.Id;
            insertChannelSt.Parameters["@name"].Value = channel.Name;
            insertChannelSt.Parameters["@rt"].Value = channel.Rt;
            insertChannelSt.Parameters["@aminPoint"].Value = channel.MainPoint;
            insertChannelSt.Parameters["@evenPoint"].Value = channel.EvenPoint;
            insertChannelSt.Parameters["@path"].Value = channel.Path;
            insertChannelSt.Parameters["@role"].Value = channel.Role;
            insertChannelSt.Parameters["@comments"].Value = channel.Comments;
            insertChannelSt.ExecuteNonQuery();
            return channel;
        }

        public void insertSymptomChannelRelation(Symptom symptom, Channel channel, int importance, string comment)
        {
            insertSymptomChannelRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            insertSymptomChannelRelationSt.Parameters["@channelId"].Value = channel.Id;
            insertSymptomChannelRelationSt.Parameters["@importance"].Value = importance;
            insertSymptomChannelRelationSt.Parameters["@comment"].Value = comment;
            insertSymptomChannelRelationSt.ExecuteNonQuery();
        }

        public void insertMeetingPointRelation(Meeting meeting, Point point)
        {
            insertMeetingPointRelationSt.Parameters["@meetingId"].Value = meeting.Id;
            insertMeetingPointRelationSt.Parameters["@pointId"].Value = point.Id;
            insertMeetingPointRelationSt.ExecuteNonQuery();
        }

        public void insertSymptomPointRelation(Symptom symptom, Point point, int importance, string comment)
        {
            insertSymptomPointRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            insertSymptomPointRelationSt.Parameters["@pointId"].Value = point.Id;
            insertSymptomPointRelationSt.Parameters["@importance"].Value = importance;
            insertSymptomPointRelationSt.Parameters["@comment"].Value = comment;
            insertSymptomPointRelationSt.ExecuteNonQuery();
        }

        public void insertSymptomMeetingRelation(Symptom symptom, Meeting meeting)
        {
            insertSymptomMeetingRelationSt.Parameters["@meentigId"].Value = meeting.Id;
            insertSymptomMeetingRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            insertSymptomMeetingRelationSt.ExecuteNonQuery();
        }

        public Symptom insertSymptom(Symptom symptom)
        {
            insertSymptomSt.Parameters["@name"].Value = symptom.Name;
            insertSymptomSt.Parameters["@comment"].Value = symptom.Comment;
            insertSymptomSt.ExecuteNonQuery();

            long rowId = connection.LastInsertRowId;

            if (rowId != 0)
            {

                int id = (int)new SQLiteCommand("select SYMPTOM_ID from SYMPTOM where rowId = " + rowId, connection).ExecuteScalar();
                return new Symptom(id, symptom);
            }
            throw new Exception("ERORR:Insert didn't accure");
        }

        public Meeting insertMeeting(Meeting meeting)
        {
            insertMeetingSt.Parameters["@meetingId"].Value = meeting.PatientId;
            insertMeetingSt.Parameters["@purpose"].Value = meeting.Purpose;
            insertMeetingSt.Parameters["@date"].Value = meeting.Date;
            insertMeetingSt.Parameters["@description"].Value = meeting.Description;
            insertMeetingSt.Parameters["@summery"].Value = meeting.Summery;
            insertMeetingSt.Parameters["@resultDescription"].Value = meeting.ResultDescription;
            insertMeetingSt.Parameters["@resultValue"].Value = meeting.Result.Value;
            insertMeetingSt.ExecuteNonQuery();

            long rowId = connection.LastInsertRowId;

            if (rowId != 0)
            {
                int id = (int)new SQLiteCommand("select MEETING_ID from MEETING where rowId = " + rowId, connection).ExecuteScalar();
                return new Meeting(id, meeting);
            }
            throw new Exception("ERORR:insert meeting didn't accure");
        }

        public Patient insertPatient(Patient patient)
        {
            try
            {
                insertPatientSt.Parameters["@name"].Value = patient.Name;
                insertPatientSt.Parameters["@telephone"].Value = patient.Telephone;
                insertPatientSt.Parameters["@cellphone"].Value = patient.Cellphone;
                insertPatientSt.Parameters["@birthday"].Value = patient.Birthday;
                insertPatientSt.Parameters["@gender"].Value = patient.Gend.Value;
                insertPatientSt.Parameters["@address"].Value = patient.Address;
                insertPatientSt.Parameters["@email"].Value = patient.Email;
                insertPatientSt.Parameters["@medicalDescription"].Value = patient.MedicalDescription;
                insertPatientSt.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                if (e.ErrorCode == (int)SQLiteErrorCode.Constraint)
                    throw new Exceptions.UniqueNameException();
                else
                    throw e;
            }
            long rowId = connection.LastInsertRowId;
            if (rowId != 0)
            {
                int id = (int)new SQLiteCommand("select PATIENT_ID from PATIENT where rowId = " + rowId, connection).ExecuteScalar();
                return new Patient(id, patient);
            }
            throw new Exception("ERORR:insert patient didn't accure");
        }

        public Point insertPoint(Point point)
        {
            insertPointsSt.Parameters["@name"].Value = point.Name;
            insertPointsSt.Parameters["@minNeedleDepth"].Value = point.MinNeedleDepth;
            insertPointsSt.Parameters["@maxNeedleDepth"].Value = point.MaxNeedleDepth;
            insertPointsSt.Parameters["@position"].Value = point.Position;
            insertPointsSt.Parameters["@importance"].Value = point.Importance;
            insertPointsSt.Parameters["@comment1"].Value = point.Comment1;
            insertPointsSt.Parameters["@comment2"].Value = point.Comment2;
            insertPointsSt.Parameters["@note"].Value = point.Note;
            insertPointsSt.Parameters["@image"].Value = point.Image;
            insertPointsSt.ExecuteNonQuery();

            long rowId = connection.LastInsertRowId;
            if (rowId != 0)
            {
                int id = (int)new SQLiteCommand("select POINT_ID from POINTS where rowId = " + rowId, connection).ExecuteScalar();
                return new Point(id, point);
            }
            throw new Exception("ERORR:insert point did't accure");
        }
        #endregion
        #region get from SQLiteDataReader the objects
        private Channel getChannel(SQLiteDataReader rs)
        {
            return new Channel(rs.GetIntL(ID), rs.GetStringL(Channel.NAME), rs.GetStringL(Channel.RT),
                    rs.GetIntL(Channel.MAIN_POINT), rs.GetIntL(Channel.EVEN_POINT), rs.GetStringL(Channel.PATH),
                    rs.GetStringL(Channel.ROLE), rs.GetStringL(Channel.COMMENT));
        }

        private Meeting getMeeting(SQLiteDataReader rs)
        {
            return new Meeting(rs.GetIntL(ID), rs.GetIntL(Meeting.PATIENT_ID), rs.GetStringL(Meeting.PURPOSE),
                    rs.GetDateTimeL(Meeting.DATE), rs.GetStringL(Meeting.DESCRIPTION), rs.GetStringL(Meeting.SUMMERY),
                    rs.GetStringL(Meeting.RESULT_DESCRIPTION),
                    Meeting.ResultValue.FromValue(rs.GetIntL(Meeting.RESULT_VALUE)));
        }

        private List<Meeting> getMeetings(SQLiteDataReader rs)
        {
            List<Meeting> o = new List<Meeting>();
            while (rs.Read())
                o.Add(getMeeting(rs));
            return o;
        }

        private Patient getPatient(SQLiteDataReader rs)
        {
            return new Patient(rs.GetIntL(ID), rs.GetStringL(Patient.NAME), rs.GetStringL(Patient.TELEPHONE),
                    rs.GetStringL(Patient.CELLPHONE), rs.GetDateTimeL(Patient.BIRTHDAY),
                    Patient.Gender.FromValue(rs.GetIntL(Patient.GENDER)), rs.GetStringL(Patient.ADDRESS), rs.GetStringL(Patient.EMAIL),
                    rs.GetStringL(Patient.MEDICAL_DESCRIPTION));
        }

        private List<Patient> getPatients(SQLiteDataReader rs)
        {
            List<Patient> o = new List<Patient>();
            while (rs.Read())
                o.Add(getPatient(rs));
            return o;
        }

        private Point getPoint(SQLiteDataReader rs)
        {
            return new Point(rs.GetIntL(ID), rs.GetStringL(Point.NAME), rs.GetIntL(Point.MIN_NEEDLE_DEPTH),
                    rs.GetIntL(Point.MAX_NEEDLE_DEPTH), rs.GetStringL(Point.NEEDLE_DESCRIPTION), rs.GetStringL(Point.POSITION),
                    rs.GetIntL(Point.IMPORTENCE), rs.GetStringL(Point.COMMENT1), rs.GetStringL(Point.COMMENT2),
                    rs.GetStringL(Point.NOTE), rs.GetStringL(Point.IMAGE));
        }

        private List<Point> getPoints(SQLiteDataReader rs)
        {
            List<Point> o = new List<Point>();
            while (rs.Read())
                o.Add(getPoint(rs));
            return o;
        }

        private Symptom getSymptom(SQLiteDataReader rs)
        {
            return new Symptom(rs.GetIntL(ID), rs.GetStringL(Symptom.NAME), rs.GetStringL(Symptom.COMMENT));
        }

        private List<Symptom> getSymptoms(SQLiteDataReader rs)
        {
            List<Symptom> o = new List<Symptom>();
            while (rs.Read())
                o.Add(getSymptom(rs));
            return o;
        }

        private ConnectionValue<Symptom> getSymptomConnection(SQLiteDataReader rs)
        {
            return new ConnectionValue<Symptom>(getSymptom(rs), rs.GetIntL(ConnectionValue<Symptom>.IMPORTENCE), rs.GetStringL(ConnectionValue<Symptom>.COMMENT));
        }

        private List<ConnectionValue<Symptom>> getSymptomsConnection(SQLiteDataReader rs)
        {
            List<ConnectionValue<Symptom>> o = new List<ConnectionValue<Symptom>>();
            while (rs.Read())
                o.Add(getSymptomConnection(rs));
            return o;
        }
        #endregion
    }
}
