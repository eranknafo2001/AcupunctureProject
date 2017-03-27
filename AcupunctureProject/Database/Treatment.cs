using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AcupunctureProject.Database
{
    public class Treatment : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique, NotNull]
        public string Name { get; set; }
        public string Path { get; set; }
        [ManyToMany(typeof(TreatmentsMeetings))]
        public List<Meeting> Treatments { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
