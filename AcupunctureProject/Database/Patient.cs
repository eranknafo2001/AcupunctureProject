using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

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
	public class Patient : ITable
	{
		[PrimaryKey,AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Telephone { get; set; }
		public string Cellphone { get; set; }
		public DateTime Birthday { get; set; }
		public Gender Gend { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string MedicalDescription { get; set; }
		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Meeting> Meetings { get; set; }
		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<Diagnostic> Diagnostics { get; set; }
		[Ignore]
		public string BirthdaySort {
			get
			{
				return Birthday.ToShortDateString();
			}
		}
		[Ignore]
		public string GendString {
			get
			{
				return Gend.MyToString();
			}
		}

		public string ToStringInSearch()
		{
			return Name + " - " + Birthday.ToShortDateString();
		}
	}
}
