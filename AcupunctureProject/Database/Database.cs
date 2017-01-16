using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace AcupunctureProject.Database
{
    public class Database
    {
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

        private SQLiteCommand getAllPointsSt;

        public Database()
        {
            string[] tempFolder = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');
            string folder = "";
            for (int i = 0; i < tempFolder.Length - 1; i++)
            {
                folder += tempFolder[i] + "\\";
            }
            connection = new SQLiteConnection("Data Source=" + folder + "database.db;Version=3;");
            connection.Open();

            getAllPointRelativeToSymptomSt = new SQLiteCommand("select POINTS.* , SYMPTOM_POINTS.IMPORTENCE , SYMPTOM_POINTS.COMMENT from POINTS INNER JOIN SYMPTOM_POINTS ON POINTS.ID = SYMPTOM_POINTS.POINT_ID and SYMPTOM_POINTS.SYMPTOM_ID = @symptomId order by SYMPTOM_POINTS.IMPORTENCE DESC;", connection);
            getAllPointRelativeToSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));
            getAllSymptomRelativeToPointSt = new SQLiteCommand("select SYMPTOM.* , SYMPTOM_POINTS.IMPORTENCE , SYMPTOM_POINTS.COMMENT from SYMPTOM INNER JOIN SYMPTOM_POINTS ON SYMPTOM.ID = SYMPTOM_POINTS.SYMPTOM_ID where SYMPTOM_POINTS.POINT_ID = @pointId order by SYMPTOM_POINTS.IMPORTENCE DESC;", connection);
            getAllSymptomRelativeToPointSt.Parameters.Add(new SQLiteParameter("@pointId"));
            getAllChannelRelativeToSymptomSt = new SQLiteCommand("SELECT CHANNEL.* , SYMPTOM_CHANNEL.IMPORTENCE , SYMPTOM_CHANNEL.COMMENT from CHANNEL INNER JOIN SYMPTOM_CHANNEL ON CHANNEL.ID = SYMPTOM_CHANNEL.CHANNEL_ID where SYMPTOM_CHANNEL.SYMPTOM_ID = @symptomId order by SYMPTOM_CHANNEL.IMPORTENCE DESC;", connection);
            getAllChannelRelativeToSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));

            insertSymptomSt = new SQLiteCommand("insert into SYMPTOM(NAME,COMMENT) values(@name,@comment);", connection);
            insertSymptomSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@comment") });
            insertMeetingSt = new SQLiteCommand("insert into MEETING(PATIENT_ID,PURPOSE,DATE,DESCRIPTION,SUMMERY,RESULT_DESCRIPTION,RESULT_VALUE) values(@patiantId,@purpose,@date,@description,@summery,@resultDescription,@resultValue);", connection);
            insertMeetingSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@patiantId"), new SQLiteParameter("@purpose"), new SQLiteParameter("@date"), new SQLiteParameter("@description"), new SQLiteParameter("@summery"), new SQLiteParameter("@resultDescription"), new SQLiteParameter("@resultValue") });
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
            updateMeetingSt = new SQLiteCommand("update MEETING set PATIENT_ID = @patiantId ,PURPOSE = @purpose ,DATE = @date ,DESCRIPTION = @description ,SUMMERY = @summery ,RESULT_DESCRIPTION = @resultDescription ,RESULT_VALUE = @resultValue where ID = @meetingId;", connection);
            updateMeetingSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@patiantId"), new SQLiteParameter("@purpose"), new SQLiteParameter("@date"), new SQLiteParameter("@description"), new SQLiteParameter("@summery"), new SQLiteParameter("@resultDescription"), new SQLiteParameter("@resultValue") });
            updatePatientSt = new SQLiteCommand("update PATIENT set NAME = @name ,TELEPHONE = @telephone ,CELLPHONE = @cellphone ,BIRTHDAY = @birthday ,GENDER = @gander ,ADDRESS = @address ,EMAIL = @email ,MEDICAL_DESCRIPTION = @medicalDescription where ID = @patiantId;", connection);
            updatePatientSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@telephone"), new SQLiteParameter("@cellphone"), new SQLiteParameter("@birthday"), new SQLiteParameter("@gender"), new SQLiteParameter("@address"), new SQLiteParameter("@email"), new SQLiteParameter("@medicalDescription") });
            updatePointSt = new SQLiteCommand("update POINTS set NAME = @name ,MIN_NEEDLE_DEPTH = @minNeeleDepth ,MAX_NEEDLE_DEPTH = @maxNeedleDepth ,POSITION = @position ,IMPORTENCE = @importance ,COMMENT1 = @comment1 ,COMMENT2 = @comment2,NOTE = @note,IMAGE = @image where ID = @pointId;", connection);
            updatePointSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@minNeedleDepth"), new SQLiteParameter("@maxNeedleDepth"), new SQLiteParameter("@position"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment1"), new SQLiteParameter("@comment2"), new SQLiteParameter("@note"), new SQLiteParameter("@image") });
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
            deletePatientSt = new SQLiteCommand("delete from PATIENT where ID = @patiantId;", connection);
            deletePatientSt.Parameters.Add(new SQLiteParameter("@patiantId"));
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
        }

        ~Database()
        {
            connection.Close();
        }

        public void updateSymptom(Symptom symptom)
        {
            updateSymptomSt.Parameters["@name"].Value = symptom.Name;
            updateSymptomSt.Parameters["@comment"].Value = symptom.Comment;
            updateSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            updateSymptomSt.ExecuteNonQuery();
        }

        //        public void updateMeeting( Meeting meeting) 
        //        {
        //            updateMeetingSt.setInt(1, meeting.getPatiantId());
        //            updateMeetingSt.setString(2, meeting.getPurpose());
        //            updateMeetingSt.setDate(3, meeting.getDate());
        //            updateMeetingSt.setString(4, meeting.getDescription());
        //            updateMeetingSt.setString(5, meeting.getSummery());
        //            updateMeetingSt.setString(6, meeting.getResultDescription());
        //            updateMeetingSt.setInt(7, meeting.getResultValue().getValue());
        //            updateMeetingSt.setInt(8, meeting.getId());
        //            updateMeetingSt.executeUpdate();
        //        }

        //        public void updatePatient( Patient patient) 
        //        {
        //            updatePatientSt.setString(1, patient.getName());
        //            updatePatientSt.setString(2, patient.getTelephone());
        //            updatePatientSt.setString(3, patient.getCellphone());
        //            updatePatientSt.setDate(4, patient.getBirthday());
        //            updatePatientSt.setInt(5, patient.getGender().getValue());
        //            updatePatientSt.setString(6, patient.getAddress());
        //            updatePatientSt.setString(7, patient.getEmail());
        //            updatePatientSt.setString(8, patient.getNedicalDescription());
        //            updatePatientSt.setInt(9, patient.getId());
        //            updatePatientSt.executeUpdate();
        //        }

        //        public void updatePoint( Point point) 
        //        {
        //            updatePointSt.setString(1, point.getName());
        //            updatePointSt.setInt(2, point.getMinNeedleDepth());
        //            updatePointSt.setInt(3, point.getMaxNeedleDepth());
        //            updatePointSt.setString(4, point.getPosition());
        //            updatePointSt.setInt(5, point.getImportance());
        //            updatePointSt.setString(6, point.getComment1());
        //            updatePointSt.setString(7, point.getComment2());
        //            updatePointSt.setString(8, point.getNote());
        //            updatePointSt.setString(9, point.getImage());
        //            updatePointSt.setInt(10, point.getId());
        //            updatePointSt.executeUpdate();
        //        }

        //        public void updateChannel( Channel channel) 
        //        {
        //            updateChannelSt.setString(1, channel.getName());
        //            updateChannelSt.setString(2, channel.getRt());
        //            updateChannelSt.setInt(3, channel.getMainPoint());
        //            updateChannelSt.setInt(4, channel.getEvenPoint());
        //            updateChannelSt.setString(5, channel.getPath());
        //            updateChannelSt.setString(6, channel.getRole());
        //            updateChannelSt.setString(7, channel.getComments());
        //            updateChannelSt.setInt(8, channel.getId());
        //            updateChannelSt.executeUpdate();
        //        }

        //        public void updateChannelSymptomRelation( Channel channel,  Symptom symptom, int importance,
        //                String comment) 
        //        {
        //            updateChannelSymptomRelationSt.setInt(1, importance);
        //            updateChannelSymptomRelationSt.setString(2, comment);
        //            updateChannelSymptomRelationSt.setInt(3, channel.getId());
        //            updateChannelSymptomRelationSt.setInt(4, symptom.getId());
        //            updateChannelSymptomRelationSt.executeUpdate();
        //        }

        //        public void updatePointSymptomRelation( Point point,  Symptom symptom, int importance, String comment)


        //        {
        //            updatePointSymptomRelationSt.setInt(1, importance);
        //            updatePointSymptomRelationSt.setString(2, comment);
        //            updatePointSymptomRelationSt.setInt(3, point.getId());
        //            updatePointSymptomRelationSt.setInt(4, symptom.getId());
        //            updatePointSymptomRelationSt.executeUpdate();
        //        }

        //        public void deleteSymptom( Symptom symptom) 
        //        {
        //            deleteSymptomSt.setInt(1, symptom.getId());
        //            deleteSymptomSt.executeUpdate();
        //        }

        //        public void deleteMeeting( Meeting meeting) 
        //        {
        //            deleteMeetingSt.setInt(1, meeting.getId());
        //            deleteMeetingSt.executeUpdate();
        //        }

        //        public void deletePatient( Patient patient) 
        //        {
        //            deletePatientSt.setInt(1, patient.getId());
        //            deletePatientSt.executeUpdate();
        //        }

        //        public void deletePoint( Point point) 
        //        {
        //            deletePointSt.setInt(1, point.getId());
        //            deletePointSt.executeUpdate();
        //        }

        //        public void deleteChannel( Channel channel) 
        //        {
        //            deleteChannelSt.setInt(1, channel.getId());
        //            deleteChannelSt.executeUpdate();
        //        }

        //        public void deleteSymptomPointRelation( Symptom symptom,  Point point) 
        //        {
        //            deleteSymptomPointRelationSt.setInt(1, symptom.getId());
        //            deleteSymptomPointRelationSt.setInt(2, point.getId());
        //            deleteSymptomPointRelationSt.executeUpdate();
        //        }

        //        public void deleteSymptomMeetingRelation( Symptom symptom,  Meeting meeting) 
        //        {
        //            deleteSymptomMeetingRelationSt.setInt(1, symptom.getId());
        //            deleteSymptomMeetingRelationSt.setInt(2, meeting.getId());
        //            deleteSymptomMeetingRelationSt.executeUpdate();
        //        }

        //        public void deleteSymptomChannelRelation( Symptom symptom,  Channel channel) 
        //        {
        //            deleteSymptomChannelRelationSt.setInt(1, symptom.getId());
        //            deleteSymptomChannelRelationSt.setInt(2, channel.getId());
        //            deleteSymptomChannelRelationSt.executeUpdate();
        //        }

        //        public void deleteMeetingPoint( Meeting meeting,  Point point) 
        //        {
        //            deleteMeetingPointSt.setInt(1, meeting.getId());
        //            deleteMeetingPointSt.setInt(2, point.getId());
        //            deleteMeetingPointSt.executeUpdate();
        //        }

        //        public Channel getChannel( int id) 
        //        {
        //            getChannelByIdSt.setInt(1, id);
        //            ResultSet rs = getChannelByIdSt.executeQuery();
        //		if (rs.next())
        //			return getChannel(rs);
        //		return null;
        //        }

        //        private Channel getChannel(ResultSet rs) 
        //        {
        //		return new Channel(rs.getInt(ID), rs.getString(Channel.NAME), rs.getString(Channel.RT),
        //				rs.getInt(Channel.MAIN_POINT), rs.getInt(Channel.EVEN_POINT), rs.getString(Channel.PATH),
        //				rs.getString(Channel.ROLE), rs.getString(Channel.COMMENT));
        //	}

        //    public Point getPoint( int id) 
        //    {
        //        getPointByIdSt.setInt(1, id);
        //        ResultSet rs = getPointByIdSt.executeQuery();
        //		if (rs.next())
        //			return getPoint(rs);
        //		return null;
        //    }

        //    public ArrayList<Point> getAllPoints() 
        //    {
        //        ResultSet rs = getAllPoints.executeQuery();
        //		return getPoints(rs);
        //    }

        //    public Point getPoint( String name) 
        //    {
        //        getPointByNameSt.setString(1, name);
        //        ResultSet rs = getPointByNameSt.executeQuery();
        //		if (rs.next())
        //			return getPoint(rs);
        //		return null;
        //    }

        //    public Symptom getSymptom( String name) 
        //    {
        //        getSymptomSt.setString(1, name);
        //        ResultSet rs = getSymptomSt.executeQuery();
        //		if (rs.next())
        //			return getSymptom(rs);
        //		return null;
        //    }

        //    public static Database getInstance() throws ClassNotFoundException, SQLException, FileNotFoundException {
        //		if (instance == null)
        //			instance = new Database();
        //		return instance;
        //	}

        //public ArrayList<Symptom> findSymptom( String name) 
        //{
        //    Statement s = connection.createStatement();
        //    ResultSet rs = s.executeQuery("SELECT * FROM SYMPTOM where NAME like '%" + name + "%';");
        //		return getSymptoms(rs);
        //}

        //public ArrayList<Patient> findPatient( String name) 
        //{
        //    Statement s = connection.createStatement();
        //    ResultSet rs = s.executeQuery("SELECT * FROM PATIENT where NAME like '%" + name + "%';");
        //		return getPatients(rs);
        //}

        //public ArrayList<ConnectionValue<Channel>> getAllChannelRelativeToSymptom( Symptom symptom)


        //{
        //    getAllChannelRelativeToSymptomSt.setInt(1, symptom.getId());
        //    ResultSet rs = getAllChannelRelativeToSymptomSt.executeQuery();
        //    ArrayList<ConnectionValue<Channel>> out = new ArrayList<>();
        //		while (rs.next())
        //			out.add(new ConnectionValue<Channel>(getChannel(rs), rs.getInt(ConnectionValue.IMPORTENCE),
        //					rs.getString(ConnectionValue.COMMENT)));
        //		return out;
        //	}

        //	public ArrayList<ConnectionValue<Point>> getAllPointRelativeToSymptom( Symptom symptom) 
        //{
        //    getAllPointRelativeToSymptomSt.setInt(1, symptom.getId());
        //    ResultSet rs = getAllPointRelativeToSymptomSt.executeQuery();
        //    ArrayList<ConnectionValue<Point>> out = new ArrayList<>();
        //		while (rs.next())
        //			out.add(new ConnectionValue<Point>(getPoint(rs), rs.getInt(ConnectionValue.IMPORTENCE),
        //					rs.getString(ConnectionValue.COMMENT)));
        //		return out;
        //	}

        //	public ArrayList<Symptom> getAllSymptomRelativeToPoint( Point point) 
        //{
        //    getAllSymptomRelativeToPointSt.setInt(1, point.getId());
        //    ResultSet rs = getAllSymptomRelativeToPointSt.executeQuery();
        //		return getSymptoms(rs);
        //}

        //public ArrayList<Meeting> getAllMeetingsRelativeToSymptoms( ArrayList<Symptom> symptoms) 
        //{
        //    String sql = "select MEETING.*,count(MEETING.ID) from MEETING INNER JOIN MEETING_SYMPTOM ON MEETING.ID = MEETING_SYMPTOM.MEETING_ID where ";
        //		for (int i = 0; i < symptoms.size(); i++) {
        //        sql += " MEETING_SYMPTOM.SYMPTOM_ID = " + symptoms.get(i).getId();
        //        if (i < symptoms.size() - 1)
        //            sql += " or ";
        //    }
        //    sql += " having count(MEETING.ID) > 0 order by count(MEETING.ID) DESC;";
        //    Statement s = connection.createStatement();
        //    ResultSet rs = s.executeQuery(sql);
        //		return getMeetings(rs);
        //}

        //public Channel insertChannel( Channel channel) 
        //{
        //    insertChannelSt.setInt(1, channel.getId());
        //    insertChannelSt.setString(2, channel.getName());
        //    insertChannelSt.setString(3, channel.getRt());
        //    insertChannelSt.setInt(4, channel.getMainPoint());
        //    insertChannelSt.setInt(5, channel.getEvenPoint());
        //    insertChannelSt.setString(6, channel.getPath());
        //    insertChannelSt.setString(7, channel.getRole());
        //    insertChannelSt.setString(8, channel.getComments());
        //    insertChannelSt.executeUpdate();
        //		return channel;
        //}

        //public void insertSymptomChannelRelation( Symptom symptom,  Channel channel,  int importance,
        //         String comment) 
        //{
        //    insertSymptomChannelRelationSt.setInt(1, symptom.getId());
        //    insertSymptomChannelRelationSt.setInt(2, channel.getId());
        //    insertSymptomChannelRelationSt.setInt(3, importance);
        //    insertSymptomChannelRelationSt.setString(4, comment);
        //    insertSymptomChannelRelationSt.executeUpdate();
        //}

        //public void insertMeetingPointRelation( Meeting meeting,  Point point) 
        //{
        //    insertMeetingPointRelationSt.setInt(1, meeting.getId());
        //    insertMeetingPointRelationSt.setInt(2, point.getId());
        //    insertMeetingPointRelationSt.executeUpdate();
        //}

        //public void insertSymptomPointRelation( Symptom symptom,  Point point, int importance, String comment)


        //{
        //    insertSymptomPointRelationSt.setInt(1, symptom.getId());
        //    insertSymptomPointRelationSt.setInt(2, point.getId());
        //    insertSymptomPointRelationSt.setInt(3, importance);
        //    insertSymptomPointRelationSt.setString(4, comment);
        //    insertSymptomPointRelationSt.executeUpdate();
        //}

        //public void insertSymptomMeetingRelation( Symptom symptom,  Meeting meeting) 
        //{
        //    insertSymptomMeetingRelationSt.setInt(1, meeting.getId());
        //    insertSymptomMeetingRelationSt.setInt(2, symptom.getId());
        //    insertSymptomMeetingRelationSt.executeUpdate();
        //}

        //public Symptom insertSymptom( Symptom symptom) throws Exception
        //{
        //    insertSymptomSt.setString(1, symptom.getName());
        //    insertSymptomSt.setString(2, symptom.getComment());
        //    insertSymptomSt.executeUpdate();
        //    ResultSet rs = insertSymptomSt.getGeneratedKeys();
        //		if (rs.next()) {
        //        int id = rs.getInt(1);
        //        return new Symptom(id, symptom);
        //    }
        //		throw new Exception("ERORR:Insert didn't accure");
        //	}

        //	public Meeting insertMeeting( Meeting meeting) throws Exception
        //{
        //    insertMeetingSt.setInt(1, meeting.getPatiantId());
        //    insertMeetingSt.setString(2, meeting.getPurpose());
        //    insertMeetingSt.setDate(3, meeting.getDate());
        //    insertMeetingSt.setString(4, meeting.getDescription());
        //    insertMeetingSt.setString(5, meeting.getSummery());
        //    insertMeetingSt.setString(6, meeting.getResultDescription());
        //    insertMeetingSt.setInt(7, meeting.getResultValue().getValue());
        //    insertMeetingSt.executeUpdate();
        //    ResultSet rs = insertMeetingSt.getGeneratedKeys();
        //		if (rs.next()) {
        //        int id = rs.getInt(1);
        //        return new Meeting(id, meeting);
        //    }
        //		throw new Exception("ERORR:insert meeting didn't accure");
        //	}

        //	public Patient insertPatient( Patient patient) throws Exception
        //{
        //    insertPatientSt.setString(1, patient.getName());
        //    insertPatientSt.setString(2, patient.getTelephone());
        //    insertPatientSt.setString(3, patient.getCellphone());
        //    insertPatientSt.setDate(4, patient.getBirthday());
        //    insertPatientSt.setInt(5, patient.getGender().getValue());
        //    insertPatientSt.setString(6, patient.getAddress());
        //    insertPatientSt.setString(7, patient.getEmail());
        //    insertPatientSt.setString(8, patient.getNedicalDescription());
        //    insertPatientSt.executeUpdate();
        //    ResultSet rs = insertPatientSt.getGeneratedKeys();
        //		if (rs.next()) {
        //        int id = rs.getInt(1);
        //        return new Patient(id, patient);
        //    }
        //		throw new Exception("ERORR:insert patient didn't accure");
        //	}

        //	public Point insertPoint( Point point) throws Exception
        //{
        //    insertPointsSt.setString(1, point.getName());
        //    insertPointsSt.setInt(2, point.getMinNeedleDepth());
        //    insertPointsSt.setInt(3, point.getMaxNeedleDepth());
        //    insertPointsSt.setString(4, point.getPosition());
        //    insertPointsSt.setInt(5, point.getImportance());
        //    insertPointsSt.setString(6, point.getComment1());
        //    insertPointsSt.setString(7, point.getComment2());
        //    insertPointsSt.setString(8, point.getNote());
        //    insertPointsSt.setString(9, point.getImage());
        //    insertPointsSt.executeUpdate();
        //    ResultSet rs = insertPointsSt.getGeneratedKeys();
        //		if (rs.next()) {
        //        int id = rs.getInt(1);
        //        return new Point(id, point);
        //    }
        //		throw new Exception("ERORR:insert point did't accure");
        //	}

        //	private Meeting getMeeting(ResultSet rs) 
        //{
        //		return new Meeting(rs.getInt(ID), rs.getInt(Meeting.PATIENT_ID), rs.getString(Meeting.PURPOSE),
        //				rs.getDate(Meeting.DATE), rs.getString(Meeting.DESCRIPTION), rs.getString(Meeting.SUMMERY),
        //				rs.getString(Meeting.RESULT_DESCRIPTION),
        //				Meeting.ResultValue.values()[rs.getInt(Meeting.RESULT_VALUE)]);
        //	}

        //	private ArrayList<Meeting> getMeetings(ResultSet rs) 
        //{
        //    ArrayList<Meeting> out = new ArrayList<>();
        //		while (rs.next())
        //			out.add(getMeeting(rs));
        //		return out;
        //	}

        //	private Patient getPatient(ResultSet rs) 
        //{
        //		return new Patient(rs.getInt(ID), rs.getString(Patient.NAME), rs.getString(Patient.TELEPHONE),
        //				rs.getString(Patient.CELLPHONE), rs.getDate(Patient.BIRTHDAY),
        //				Gender.values()[rs.getInt(Patient.GENDER)], rs.getString(Patient.ADDRESS), rs.getString(Patient.EMAIL),
        //				rs.getString(Patient.MEDICAL_DESCRIPTION));
        //	}

        //	private ArrayList<Patient> getPatients(ResultSet rs) 
        //{
        //    ArrayList<Patient> out = new ArrayList<>();
        //		while (rs.next())
        //			out.add(getPatient(rs));
        //		return out;
        //	}

        //	private Point getPoint(ResultSet rs) 
        //{
        //		return new Point(rs.getInt(ID), rs.getString(Point.NAME), rs.getInt(Point.MIN_NEEDLE_DEPTH),
        //				rs.getInt(Point.MAX_NEEDLE_DEPTH), rs.getString(Point.NEEDLE_DESCRIPTION), rs.getString(Point.POSITION),
        //				rs.getInt(Point.IMPORTENCE), rs.getString(Point.COMMENT1), rs.getString(Point.COMMENT2),
        //				rs.getString(Point.NOTE), rs.getString(Point.IMAGE));
        //	}

        //	private ArrayList<Point> getPoints(ResultSet rs) 
        //{
        //    ArrayList<Point> out = new ArrayList<>();
        //		while (rs.next())
        //			out.add(getPoint(rs));
        //		return out;
        //	}

        //	private Symptom getSymptom(ResultSet rs) 
        //{
        //		return new Symptom(rs.getInt(ID), rs.getString(Symptom.NAME), rs.getString(Symptom.COMMENT));
        //	}

        //	private ArrayList<Symptom> getSymptoms(ResultSet rs) 
        //{
        //    ArrayList<Symptom> out = new ArrayList<>();
        //		while (rs.next())
        //			out.add(getSymptom(rs));
        //		return out;
        //	}
    }
}
