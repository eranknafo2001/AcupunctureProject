using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.Database.Treatment
{
    public class Treatment
    {
        public int Id { get; private set; }
        public string Name { get; set; }

        public Treatment(int id,string name)
        {
            Id = id;
            Name = name;
        }
        public Treatment(string name) : this(-1, name) { }
    }
}
