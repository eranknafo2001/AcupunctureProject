using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.Database.Treatment
{
    public class Symptom
    {
        public int Id { get; private set; }
        public string Text { get; set; }

        public Symptom(int id, string text)
        {
            Id = id;
            Text = text;
        }
        public Symptom(int id,Symptom other) : this(id, other.Text) { }
    }
}
