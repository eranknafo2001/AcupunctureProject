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
	public delegate void TableChangedEventHendler(Type table, object item);

	public static class DatabaseConnection
	{
		public static event TableChangedEventHendler InsertedItemEvent
		{
			add => PDatabaseConnection.Instance.InsertedItemEvent += value;
			remove => PDatabaseConnection.Instance.InsertedItemEvent -= value;
		}
		public static event TableChangedEventHendler UpdatedItemEvent
		{
			add => PDatabaseConnection.Instance.UpdatedItemEvent += value;
			remove => PDatabaseConnection.Instance.UpdatedItemEvent -= value;
		}
		public static event TableChangedEventHendler DeletedItemEvent
		{
			add => PDatabaseConnection.Instance.DeletedItemEvent += value;
			remove => PDatabaseConnection.Instance.DeletedItemEvent -= value;
		}
		public static event TableChangedEventHendler TableChangedEvent
		{
			add => PDatabaseConnection.Instance.TableChangedEvent += value;
			remove => PDatabaseConnection.Instance.TableChangedEvent -= value;
		}
		public static T Update<T>(T item) => PDatabaseConnection.Instance.Update(item);
		public static MColor GetLevel(int level) => PDatabaseConnection.Instance.GetLevel(level);
		public static void SetLevel(int level, MColor color) => PDatabaseConnection.Instance.SetLevel(level, color);
		public static T UpdateWithChildren<T>(T item) => PDatabaseConnection.Instance.UpdateWithChildren(item);
		public static void Delete<T>(T item) => PDatabaseConnection.Instance.Delete(item);
		public static T GetChildren<T>(T item) => PDatabaseConnection.Instance.GetChildren(item);
		public static List<T> GetChildren<T>(List<T> items) => PDatabaseConnection.Instance.GetChildren(items);
		public static bool Exist<T>(T item) where T : class, ITable => PDatabaseConnection.Instance.Exist(item);
		public static T Set<T>(T item) where T : class, ITable => PDatabaseConnection.Instance.Set(item);
		public static T SetWithChildren<T>(T item) where T : class, ITable => PDatabaseConnection.Instance.SetWithChildren(item);
		public static T Insert<T>(T item) where T : ITable => PDatabaseConnection.Instance.Insert(item);
		public static T InsertWithChildren<T>(T item) where T : ITable => PDatabaseConnection.Instance.InsertWithChildren(item);
		public static List<Symptom> FindSymptom(string name) => PDatabaseConnection.Instance.FindSymptom(name);
		public static List<Patient> FindPatient(string name) => PDatabaseConnection.Instance.FindPatient(name);
		public static Meeting GetTheLastMeeting(Patient patient) => PDatabaseConnection.Instance.GetTheLastMeeting(patient);
		public static List<Treatment> GetAllTreatments() => PDatabaseConnection.Instance.GetAllTreatments();
		public static List<Point> GetAllPoints() => PDatabaseConnection.Instance.GetAllPoints();
	}

	public class PDatabaseConnection
	{
		private static PDatabaseConnection _Instance = null;
		public static PDatabaseConnection Instance
		{
			get
			{
				if (_Instance == null)
					_Instance = new PDatabaseConnection();
				return _Instance;
			}
		}
		private SQLiteConnection Connection;
		public readonly static int NUM_OF_PRIORITIES = 6;
		public event TableChangedEventHendler InsertedItemEvent;
		public event TableChangedEventHendler UpdatedItemEvent;
		public event TableChangedEventHendler DeletedItemEvent;
		public event TableChangedEventHendler TableChangedEvent;

		private PDatabaseConnection(string FileName = "database.db")
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
					Connection.CreateTable<Treatment>();
					Connection.CreateTable<Color>();
					InitColors();
				}
			}
			Connection.DropTable<Diagnostic>();
			Connection.CreateTable<Diagnostic>();
			InsertedItemEvent += new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
			UpdatedItemEvent += new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
			DeletedItemEvent += new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
		}

		~PDatabaseConnection()
		{
			InsertedItemEvent -= new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
			UpdatedItemEvent -= new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
			DeletedItemEvent -= new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
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
			DeletedItemEvent?.Invoke(typeof(T), null);
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
		public List<Point> GetAllPoints() => (from s in Connection.Table<Point>()
											  where true
											  select s).ToList();

		public List<Symptom> FindSymptom(string name) => (from symptom in Connection.Table<Symptom>()
														  where symptom.Name.ToLower().Contains(name.ToLower())
														  select symptom).ToList();

		public List<Patient> FindPatient(string name) => (from patient in Connection.Table<Patient>()
														  where patient.Name.ToLower().Contains(name.ToLower())
														  select patient).ToList();

		public Meeting GetTheLastMeeting(Patient patient) => (from meeting in Connection.Table<Meeting>()
															  where meeting.PatientId == patient.Id
															  orderby meeting.Id descending
															  select meeting).FirstOrDefault();

		public List<Treatment> GetAllTreatments() => (from t in Connection.Table<Treatment>()
													  where true
													  select t).ToList();
		#endregion
		#region inserts

		public bool Exist<T>(T item) where T : class, ITable => Connection.Find<T>(item.Id) != null;

		public T Set<T>(T item) where T : class, ITable
		{
			if (Exist(item))
			{
				Connection.Update(item);
				UpdatedItemEvent?.Invoke(typeof(T), item);
			}
			else
			{
				try
				{
					Connection.Insert(item);
				}
				catch (SQLite.Net.SQLiteException e)
				{
					throw e;
				}
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
			InsertedItemEvent?.Invoke(typeof(T), item);
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