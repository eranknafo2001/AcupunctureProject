﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AcupunctureProject.Database
{
	public static partial class Ex
	{
		public static string MyToString(this Gender gend)
		{
			switch (gend)
			{
				case Gender.MALE:
					return "זכר";
				case Gender.FEMALE:
					return "נקבה";
				case Gender.OTHER:
					return "אחר";
				default:
					return null;
			}
		}
	}
	public enum Gender
	{
		MALE, FEMALE, OTHER
	}
	public class Patient : ITable, INotifyPropertyChanged
	{
		private int _Id;
		[PrimaryKey, Unique, AutoIncrement]
		public int Id
		{
			get => _Id;
			set
			{
				if (value != _Id)
				{
					_Id = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _Name;
		public string Name
		{
			get => _Name;
			set
			{
				if (value != _Name)
				{
					_Name = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _Telephone;
		public string Telephone
		{
			get => _Telephone;
			set
			{
				if (value != _Telephone)
				{
					_Telephone = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _Cellphone;
		public string Cellphone
		{
			get => _Cellphone;
			set
			{
				if (value != _Cellphone)
				{
					_Cellphone = value;
					PropertyChangedEvent();
				}
			}
		}

		[Ignore]
		public DateTime? Birthday
		{
			get
			{
				if (BirthdayNum == null)
					return null;
				return DateTime.FromFileTime(BirthdayNum.Value);
			}
			set => BirthdayNum = value?.ToFileTime();
		}

		private long? _BirthdayNum;
		public long? BirthdayNum
		{
			get => _BirthdayNum;
			set
			{
				if (value != _BirthdayNum)
				{
					_BirthdayNum = value;
					PropertyChangedEvent();
					PropertyChangedEvent(nameof(Birthday));
					PropertyChangedEvent(nameof(BirthdaySort));
				}
			}
		}

		private Gender _Gend;
		public Gender Gend
		{
			get => _Gend;
			set
			{
				if (value != _Gend)
				{
					_Gend = value;
					PropertyChangedEvent();
					PropertyChangedEvent(nameof(GendString));
				}
			}
		}

		private string _Address;
		public string Address
		{
			get => _Address;
			set
			{
				if (value != _Address)
				{
					_Address = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _Email;
		public string Email
		{
			get => _Email;
			set
			{
				if (value != _Email)
				{
					_Email = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _MedicalDescription;
		public string MedicalDescription
		{
			get => _MedicalDescription;
			set
			{
				if (value != _MedicalDescription)
				{
					_MedicalDescription = value;
					PropertyChangedEvent();
				}
			}
		}

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Meeting> Meetings { get; set; }

		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Diagnostic> Diagnostics { get; set; }

		[Ignore]
		public string BirthdaySort => Birthday?.ToShortDateString();

		[Ignore]
		public string GendString => Gend.MyToString();

		public event PropertyChangedEventHandler PropertyChanged;
		private void PropertyChangedEvent([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
