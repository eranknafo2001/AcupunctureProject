using MColor = System.Windows.Media.Color;
using System.Collections.Generic;
using SQLite.Net;
using Platform = SQLite.Net.Platform;
using System.Linq;
using SQLiteNetExtensions.Extensions;
using System.IO;
using System;

namespace AcupunctureProject.Database2
{
	public delegate void TableChangedEventHendler(Type table, object item);

	public static class DatabaseConnection
	{
		private static SQLiteConnection Connection;
		public readonly static int NUM_OF_PRIORITIES = 6;
		public static event TableChangedEventHendler InsertedItemEvent;
		public static event TableChangedEventHendler UpdatedItemEvent;
		public static event TableChangedEventHendler DeletedItemEvent;
		public static event TableChangedEventHendler TableChangedEvent;

		static DatabaseConnection()
		{
			var Folder = System.Reflection.Assembly.GetEntryAssembly().Location;
			Folder = Folder.Remove(Folder.LastIndexOf('\\') + 1);
			Folder += "database.db";
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
			InsertedItemEvent += new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
			UpdatedItemEvent += new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
			DeletedItemEvent += new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
		}

		private static readonly Destructor destructor = new Destructor();
		private class Destructor
		{
			~Destructor()
			{
				InsertedItemEvent -= new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
				UpdatedItemEvent -= new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
				DeletedItemEvent -= new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
			}
		}

		#region colors handler
		private static void InitColors()
		{
			Connection.Insert(new Color() { Id = 0, R = 51, G = 0, B = 0 });
			Connection.Insert(new Color() { Id = 1, R = 102, G = 102, B = 0 });
			Connection.Insert(new Color() { Id = 2, R = 0, G = 102, B = 102 });
			Connection.Insert(new Color() { Id = 3, R = 51, G = 0, B = 102 });
			Connection.Insert(new Color() { Id = 4, R = 102, G = 0, B = 51 });
			Connection.Insert(new Color() { Id = 5, R = 0, G = 0, B = 102 });
		}
		public static MColor GetLevel(int level) => Connection.Get<Color>(level).GetColor();

		public static void SetLevel(int level, MColor color) => Connection.Update(new Color() { Id = level, R = color.R, G = color.G, B = color.B });
		#endregion
		public static T Update<T>(T item)
		{
			Connection.Update(item);
			UpdatedItemEvent?.Invoke(typeof(T), item);
			return item;
		}
		public static T UpdateWithChildren<T>(T item)
		{
			Connection.UpdateWithChildren(item);
			UpdatedItemEvent?.Invoke(typeof(T), item);
			return item;
		}
		public static void Delete<T>(T item)
		{
			Connection.Delete(item);
			DeletedItemEvent?.Invoke(typeof(T), null);
		}
		public static T GetChildren<T>(T item)
		{
			Connection.GetChildren(item);
			return item;
		}
		public static List<T> GetChildren<T>(List<T> items)
		{
			foreach (var item in items)
				Connection.GetChildren(item);
			return items;
		}
		#region finds objects
		public static List<Point> GetAllPoints() => (from s in Connection.Table<Point>()
													 where true
													 select s).ToList();

		public static List<Symptom> FindSymptom(string name) => (from symptom in Connection.Table<Symptom>()
																 where symptom.Name.ToLower().Contains(name.ToLower())
																 select symptom).ToList();

		public static List<Channel> GetAllChannels() => (from s in Connection.Table<Channel>()
														 where true
														 select s).ToList();

		public static List<Patient> FindPatient(string name) => (from patient in Connection.Table<Patient>()
																 where patient.Name.ToLower().Contains(name.ToLower())
																 select patient).ToList();

		public static Meeting GetTheLastMeeting(Patient patient) => (from meeting in Connection.Table<Meeting>()
																	 where meeting.PatientId == patient.Id
																	 orderby meeting.Id descending
																	 select meeting).FirstOrDefault();

		public static List<Treatment> GetAllTreatments() => (from t in Connection.Table<Treatment>()
															 where true
															 select t).ToList();
		#endregion
		#region inserts

		public static bool Exist<T>(T item) where T : class, ITable => Connection.Find<T>(item.Id) != null;

		public static T Set<T>(T item) where T : class, ITable
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

		public static T SetWithChildren<T>(T item) where T : class, ITable
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

		public static T Insert<T>(T item) where T : ITable
		{
			Connection.Insert(item);
			InsertedItemEvent?.Invoke(typeof(T), item);
			return item;
		}

		public static T InsertWithChildren<T>(T item) where T : ITable
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
		[SQLite.Net.Attributes.PrimaryKey, SQLite.Net.Attributes.Unique]
		int Id { get; set; }
	}
}