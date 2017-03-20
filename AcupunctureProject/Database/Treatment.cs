using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace AcupunctureProject.Database
{
    public class Treatment : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique,NotNull]
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
