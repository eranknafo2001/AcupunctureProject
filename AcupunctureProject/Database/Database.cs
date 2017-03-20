using MColor = System.Windows.Media.Color;
using System.Collections.Generic;
using SQLite.Net;
using Platform = SQLite.Net.Platform;
using System.Linq;
using SQLiteNetExtensions.Extensions;
using System.IO;

namespace AcupunctureProject.Database
{
    public interface ITable
    {
        int Id { get; set; }
    }
    public class DatabaseConnection
    {
        private static DatabaseConnection database = null;
        public static DatabaseConnection Instance
        {
            get
            {
                if (database == null)
                    database = new DatabaseConnection();
                return database;
            }
        }
        private SQLiteConnection Connection;
        public readonly static int NUM_OF_PRIORITIES = 6;
        private readonly string Folder;

        private DatabaseConnection()
        {
            string Folder = System.Reflection.Assembly.GetEntryAssembly().Location;
            Folder = Folder.Remove(Folder.LastIndexOf('\\') + 1);
            Folder += "database.db";
            Platform.Generic.SQLitePlatformGeneric p = new Platform.Generic.SQLitePlatformGeneric();
            bool isDatabaseExists = File.Exists(Folder);
            Connection = new SQLiteConnection(p, Folder);
            if (!isDatabaseExists)
            {
                using (Connection)
                {
                    Connection.CreateTable<SymptomPoint>();
                    Connection.CreateTable<ChannelSymptom>();
                    Connection.CreateTable<MeetingSymptom>();
                    Connection.CreateTable<MeetingPoint>();
                    Connection.CreateTable<ChannelMeeting>();
                    Connection.CreateTable<Point>();
                    Connection.CreateTable<Channel>();
                    Connection.CreateTable<Symptom>();
                    Connection.CreateTable<Patient>();
                    Connection.CreateTable<Meeting>();
                    Connection.CreateTable<Diagnostic>();
                    Connection.CreateTable<Treatment>();
                    Connection.CreateTable<Color>();
                    InitColors();
                }
            }

        }

        #region colors handler
        private void InitColors()
        {
            Connection.Insert(new Color() { Id = 0, R = 51, G = 0, B = 0 });
            Connection.Insert(new Color() { Id = 1, R = 102, G = 102, B = 0 });
            Connection.Insert(new Color() { Id = 2, R = 0, G = 102, B = 102 });
            Connection.Insert(new Color() { Id = 3, R = 51, G = 0, B = 102 });
            Connection.Insert(new Color() { Id = 4, R = 102, G = 0, B = 51 });
            Connection.Insert(new Color() { Id = 5, R = 0, G = 0, B = 102 });
        }
        public MColor GetLevel(int level) => Connection.Get<Color>(level).GetColor();

        public void SetLevel(int level, MColor color) => Connection.Update(new Color() { Id = level, R = color.R, G = color.G, B = color.B });
        #endregion
        public void Update<T>(T item) => Connection.UpdateWithChildren(item);
        public void Delete<T>(T item) => Connection.Delete(item);
        #region finds objects
        public Channel GetChannel(int id) => Connection.GetWithChildren<Channel>(id);

        public Point GetPoint(int id) => Connection.GetWithChildren<Point>(id);

        public List<Point> GetAllPoints() => Connection.GetAllWithChildren<Point>();

        public Point GetPoint(string name)
        {
            var list = Connection.GetAllWithChildren<Point>(point => point.Name == name);
            foreach (Point point in list)
                return point;
            return null;
        }

        public Symptom GetSymptom(string name)
        {
            var list = Connection.GetAllWithChildren<Symptom>(symptom => symptom.Name == name);
            foreach (Symptom symptom in list)
                return symptom;
            return null;
        }

        public List<Symptom> FindSymptom(string name)
        {
            return Connection.GetAllWithChildren<Symptom>(symptom => symptom.Name.Contains(name));
        }

        public List<Point> FindPoint(string name)
        {
            return Connection.GetAllWithChildren<Point>(point => point.Name.Contains(name));
        }


        public List<Patient> FindPatient(string name)
        {
            return Connection.GetAllWithChildren<Patient>(patient => patient.Name.Contains(name));
        }

        public List<Meeting> GetAllMeetingsRelativeToSymptoms(List<Symptom> symptoms)
        {
            HashSet<Meeting> list = new HashSet<Meeting>();
            foreach(var sym in symptoms)
            {
                var li = Connection.GetAllWithChildren<Meeting>(meeting => meeting.Symptoms.Contains(sym));
                foreach(var meeting in li)
                    list.Add(meeting);
            }
            return list.ToList();
        }

        public Meeting GetTheLastMeeting(Patient patient)
        {
            var list = Connection.GetAllWithChildren<Meeting>(meeting => meeting.PatientId == patient.Id).OrderBy(name => name.Date).Reverse();
            foreach (Meeting meeting in list)
            {
                return meeting;
            }
            return null;
        }

        public List<Treatment> GetAllTreatments() => Connection.GetAllWithChildren<Treatment>();

        public List<Treatment> GetTreatmentsByName(string name) => Connection.GetAllWithChildren<Treatment>(treatment => treatment.Name.Contains(name));
        #endregion
        #region inserts
        public Diagnostic SetDiagnostic(Diagnostic diagnostic)
        {
            if (Connection.Find<Diagnostic>(diagnostic.Id) != null)
            {
                Connection.Update(diagnostic);
            }
            else
            {
                diagnostic.Id = Connection.Insert(diagnostic);
                Update(diagnostic);
            }
            return diagnostic;
        }
        public T Insert<T>(T item) where T : ITable
        {
            Connection.Insert(item);
            Update(item);
            return item;
        }
        #endregion
    }
}