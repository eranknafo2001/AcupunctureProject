using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.database
{
    public class ConnectionValue<V>
    {
        public V Value { get; private set; }
        public int Importance { get; private set; }
        public string Comment { get; private set; }

        public ConnectionValue(V value,int importance,string comment)
        {
            Value = value;
            Importance = importance;
            Comment = comment;
        }
    }
}
