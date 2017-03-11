using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.Database.Treatment
{
    public class Operation
    {
        public readonly static string TEXT = "TEXT";
        public int Id { get; private set; }
        public string Text { get; set; }

        public Operation(int id, string text)
        {
            Id = id;
            Text = text;
        }

        public Operation(int id,Operation other) : this(id, other.Text) { }

        public Operation(string text) : this(-1,  text) { }
    }
}
