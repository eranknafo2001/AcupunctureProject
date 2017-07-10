using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace AcupunctureProject.Database
{
	public class Treatment : ITable, INotifyPropertyChanged
	{
		[PrimaryKey, Unique, AutoIncrement]
		public int Id { get; set; }

		string _Name;
		[Unique, NotNull]
		public string Name
		{
			get => _Name;
			set
			{
				if (_Name != value)
				{
					_Name = value;
					PropertyChangedEvent();
				}
			}
		}
		string _Path;
		public string Path
		{
			get => _Path;
			set
			{
				if (_Path != value)
				{
					_Path = value;
					PropertyChangedEvent();
				}
			}
		}
		[ManyToMany(typeof(TreatmentsMeetings))]
		public List<Meeting> Meetings { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		void PropertyChangedEvent([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		public override string ToString() => Name;

	}
}
