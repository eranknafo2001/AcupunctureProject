using MColor = System.Windows.Media.Color;
using System.Collections.Generic;
using SQLite.Net;
using Platform = SQLite.Net.Platform;
using System.Linq;
using SQLiteNetExtensions.Extensions;
using System.IO;
using System;
//using 

namespace AcupunctureProject.Database
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
			Database2.DatabaseConnection.GetAllChannels();
			var Folder = System.Reflection.Assembly.GetEntryAssembly().Location;
			Folder = Folder.Remove(Folder.LastIndexOf('\\') + 1);
			Folder += "database2.db";
			bool isDatabaseExists = File.Exists(Folder);
			Connection = new SQLiteConnection(new Platform.Win32.SQLitePlatformWin32(), Folder);
			if (!isDatabaseExists)
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
				Connection.CreateTable<Diagnostic>();
				Connection.CreateTable<Color>();
				InitColors();
				TransferData();
			}
			InsertedItemEvent += new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
			UpdatedItemEvent += new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
			DeletedItemEvent += new TableChangedEventHendler((t, i) => TableChangedEvent?.Invoke(t, i));
		}

		private static void TransferData()
		{
			new System.Threading.Thread(() => { System.Windows.Forms.MessageBox.Show("מתחיל לתקן את הבסיסי מידה (כדי שהתאריך יעבוד כמו שצריך)"); }).Start();
			var patients = Database2.DatabaseConnection.FindPatient("");
			var syms = Database2.DatabaseConnection.FindSymptom("");
			var d1syms = new List<Symptom>();
			var points = Database2.DatabaseConnection.GetAllPoints();
			var d1points = new List<Point>();
			var treatments = Database2.DatabaseConnection.GetAllTreatments();
			var channels = Database2.DatabaseConnection.GetAllChannels();
			var d1channels = new List<Channel>();
			Database2.DatabaseConnection.GetChildren(channels);
			Database2.DatabaseConnection.GetChildren(syms);
			Database2.DatabaseConnection.GetChildren(points);
			Database2.DatabaseConnection.GetChildren(treatments);
			Database2.DatabaseConnection.GetChildren(patients);

			foreach (var t in treatments)
			{
				var d1t = new Treatment()
				{
					Meetings = new List<Meeting>(),
					Name = t.Name,
					Path = t.Path
				};
				Insert(d1t);
			}

			foreach (var channel in channels)
			{
				var d1channel = new Channel()
				{
					Comments = channel.Comments,
					EvenPoint = channel.EvenPoint,
					Id = channel.Id,
					MainPoint = channel.MainPoint,
					Name = channel.Name,
					Path = channel.Path,
					Role = channel.Role,
					Rt = channel.Rt,
					Meetings = new List<Meeting>(),
					SymptomConnections = new List<ChannelSymptom>(),
					Symptoms = new List<Symptom>(),
				};
				d1channels.Add(d1channel);
				Insert(d1channel);
			}

			foreach (var point in points)
			{
				var d1point = new Point()
				{
					Comment1 = point.Comment1,
					Name = point.Name,
					Comment2 = point.Comment2,
					Image = point.Image,
					Importance = point.Importance,
					MaxNeedleDepth = point.MaxNeedleDepth,
					MinNeedleDepth = point.MinNeedleDepth,
					NeedleDescription = point.NeedleDescription,
					Note = point.Note,
					Position = point.Position,
					Symptoms = new List<Symptom>(),
					SymptomConnections = new List<SymptomPoint>(),
					Meetings = new List<Meeting>(),
				};
				d1points.Add(d1point);
				Insert(d1point);
			}

			foreach (var sym in syms)
			{
				var d1sym = new Symptom()
				{
					Comment = sym.Comment,
					Name = sym.Name,
					ChannelConnections = new List<ChannelSymptom>(),
					Channels = new List<Channel>(),
					Meetings = new List<Meeting>(),
					Points = new List<Point>(),
					PointsConnections = new List<SymptomPoint>(),
				};
				d1syms.Add(d1sym);
				Insert(d1sym);
				d1sym.ChannelConnections = new List<ChannelSymptom>();
				foreach (var con in sym.ChannelConnections)
				{
					Database2.DatabaseConnection.GetChildren(con);
					var d1con = new ChannelSymptom()
					{
						Channel = Connection.Get<Channel>(con.ChannelId),
						Symptom = d1sym,
						Comment = con.Comment,
						Importance = con.Imaportance,

					};
					InsertWithChildren(d1con);
				}
				d1sym.PointsConnections = new List<SymptomPoint>();
				foreach (var con in sym.PointsConnections)
				{
					Database2.DatabaseConnection.GetChildren(con);
					var d1con = new SymptomPoint()
					{
						Comment = con.Comment,
						Importance = con.Imaportance,
						Point = Connection.Find<Point>(s => s.Name == con.Point.Name),
						Symptom = d1sym,
					};
					InsertWithChildren(d1con);
				}
			}

			foreach (var patient in patients)
			{
				var d1patient = new Patient()
				{
					Address = patient.Address,
					Cellphone = patient.Cellphone,
					Email = patient.Email,
					Gend = (Gender)(int)patient.Gend,
					MedicalDescription = patient.MedicalDescription,
					Name = patient.Name,
					Telephone = patient.Telephone,
					Diagnostics = new List<Diagnostic>(),
					Meetings = new List<Meeting>(),
				};
				if (patient.Birthday == null)
					d1patient.Birthday = null;
				else
				{
					d1patient.Birthday = DateTime.FromFileTimeUtc(patient.Birthday.Value.ToFileTime());
				}
				Insert(d1patient);

				var meetings = patient.Meetings;
				Database2.DatabaseConnection.GetChildren(meetings);
				foreach (var meeting in meetings)
				{
					var d1meeting = new Meeting()
					{
						Description = meeting.Description,
						Date = DateTime.FromFileTimeUtc(meeting.Date.ToFileTime()),
						Patient = d1patient,
						Purpose = meeting.Purpose,
						Result = (ResultValue)(int)meeting.Result,
						ResultDescription = meeting.ResultDescription,
						Summery = meeting.Summery,
						Channels = new List<Channel>(),
						Points = new List<Point>(),
						Symptoms = new List<Symptom>(),
						Treatments = new List<Treatment>(),
					};
					Insert(d1meeting);
					d1meeting.Points = new List<Point>();
					foreach (var point in meeting.Points)
					{
						d1meeting.Points.Add(Connection.Find<Point>(s => s.Name == point.Name));
					}
					d1meeting.Symptoms = new List<Symptom>();
					foreach (var sym in meeting.Symptoms)
					{
						d1meeting.Symptoms.Add(Connection.Find<Symptom>(s => s.Name == sym.Name));
					}
					d1meeting.Treatments = new List<Treatment>();
					foreach (var t in meeting.Treatments)
					{
						d1meeting.Treatments.Add(Connection.Find<Treatment>(s => s.Name == t.Name));
					}
					UpdateWithChildren(d1meeting);
				}
			}
			new System.Threading.Thread(() => { System.Windows.Forms.MessageBox.Show("סיים לתקן את הבסיסי מידה (כדי שהתאריך יעבוד כמו שצריך)"); }).Start();
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
			Connection.InsertWithChildren(item);
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