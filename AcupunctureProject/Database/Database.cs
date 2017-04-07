using MColor = System.Windows.Media.Color;
using System.Collections.Generic;
using SQLite.Net;
using Platform = SQLite.Net.Platform;
using System.Linq;
using SQLiteNetExtensions.Extensions;
using System.IO;
using System;

namespace AcupunctureProject.Database
{
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
		public delegate void TableChanged(Type table, object item);
		public event TableChanged InsertedItemEvent;
		public event TableChanged UpdatedItemEvent;
		public event TableChanged DeletedItemEvent;
		public event TableChanged TableChangedEvent;

		private DatabaseConnection(string FileName = "database.db")
		{
			var Folder = System.Reflection.Assembly.GetEntryAssembly().Location;
			Folder = Folder.Remove(Folder.LastIndexOf('\\') + 1);
			Folder += FileName;
			var p = new Platform.Win32.SQLitePlatformWin32();
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
					Connection.CreateTable<TreatmentsMeetings>();
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
			//Connection.DropTable<MeetingSymptom>();
			//Connection.DropTable<MeetingPoint>();
			//Connection.DropTable<ChannelMeeting>();
			//Connection.CreateTable<MeetingSymptom>();
			//Connection.CreateTable<MeetingPoint>();
			//Connection.CreateTable<ChannelMeeting>();
			//Connection.CreateTable<TreatmentsMeetings>();
			InsertedItemEvent += new TableChanged((t, i) => TableChangedEvent?.Invoke(t, i));
			UpdatedItemEvent += new TableChanged((t, i) => TableChangedEvent?.Invoke(t, i));
			DeletedItemEvent += new TableChanged((t, i) => TableChangedEvent?.Invoke(t, i));
		}

		~DatabaseConnection()
		{
			InsertedItemEvent -= new TableChanged((t, i) => TableChangedEvent?.Invoke(t, i));
			UpdatedItemEvent -= new TableChanged((t, i) => TableChangedEvent?.Invoke(t, i));
			DeletedItemEvent -= new TableChanged((t, i) => TableChangedEvent?.Invoke(t, i));
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
		public T Update<T>(T item)
		{
			Connection.Update(item);
			UpdatedItemEvent?.Invoke(typeof(T), item);
			return item;
		}
		public T UpdateWithChildren<T>(T item)
		{
			Connection.UpdateWithChildren(item);
			UpdatedItemEvent?.Invoke(typeof(T), item);
			return item;
		}
		public void Delete<T>(T item)
		{
			Connection.Delete(item);
			DeletedItemEvent?.Invoke(typeof(T),null);
		}
		public T GetChildren<T>(T item)
		{
			Connection.GetChildren(item);
			return item;
		}
		public List<T> GetChildren<T>(List<T> items)
		{
			foreach (var item in items)
				Connection.GetChildren(item);
			return items;
		}
		#region finds objects
		public Channel GetChannel(int id) => Connection.Get<Channel>(id);

		public Point GetPoint(int id) => Connection.Get<Point>(id);

		public List<Point> GetAllPoints() =>  (from s in Connection.Table<Point>() where true select s).ToList();

		public Point GetPoint(string name) => Connection.Find<Point>(point => point.Name == name);

		public Symptom GetSymptom(string name) => Connection.Find<Symptom>(symptom => symptom.Name == name);

		public List<Symptom> FindSymptom(string name) => (from symptom in Connection.Table<Symptom>() where symptom.Name.ToLower().Contains(name.ToLower()) select symptom).ToList();

		public List<Patient> FindPatient(string name) => (from patient in Connection.Table<Patient>()
														  where patient.Name.ToLower().Contains(name.ToLower())
														  select patient).ToList();

		public Meeting GetTheLastMeeting(Patient patient) => (from meeting in Connection.Table<Meeting>()
															  where meeting.PatientId == patient.Id
															  orderby meeting.Date descending
															  select meeting).FirstOrDefault();

		public List<Treatment> GetAllTreatments() => (from t in Connection.Table<Treatment>() where true select t).ToList();
		#endregion
		#region inserts
		public T Set<T>(T item) where T : class, ITable
		{
			if (Connection.Find<T>(item.Id) != null)
			{
				Connection.Update(item);
				UpdatedItemEvent?.Invoke(typeof(T), item);
			}
			else
			{
				Connection.Insert(item);
				InsertedItemEvent?.Invoke(typeof(T), item);
			}
			return item;
		}

		public T SetWithChildren<T>(T item) where T : class, ITable
		{
			if (Connection.Find<T>(item.Id) != null)
			{
				Connection.UpdateWithChildren(item);
				UpdatedItemEvent?.Invoke(typeof(T), item);
			}
			else
			{
				Connection.InsertWithChildren(item);
				InsertedItemEvent?.Invoke(typeof(T), item);
			}
			return item;
		}

		public T Insert<T>(T item) where T : ITable
		{
			Connection.Insert(item);
			InsertedItemEvent?.Invoke(typeof(T), item);
			return item;
		}

		public T InsertWithChildren<T>(T item) where T : ITable
		{
			Connection.Insert(item);
			Connection.UpdateWithChildren(item);
			//InsertedItemEvent?.Invoke(typeof(T), item);
			return item;
		}
		#endregion
	}

	public interface ITable
	{
		[SQLite.Net.Attributes.PrimaryKey]
		int Id { get; set; }
	}
}