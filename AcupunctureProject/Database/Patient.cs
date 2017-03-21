using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AcupunctureProject.Database
{
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
        public string ToStringInSearch()
        {
            return Name + " - " + Birthday.ToShortDateString();
        }
    }
}
