using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.Database
{
    public class Treatment
    {
        public readonly static string NAME = "NAME";
        public readonly static string PATH = "PATH";
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public Treatment(int id, string name, string path)
        {
            Id = id;
            Name = name;
            Path = path;
        }
        public Treatment(int id, Treatment other) : this(id, other.Name, other.Path) { }

        public Treatment(string name, string path) : this(-1, name, path) { }
    }
}
