using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.Database.Treatment
{
    public class Operation
    {
        public int Id { get; private set; }
        public int TreatmentId { get; private set; }
        public string Text { get; set; }

        public Operation(int id, int treatmentId, string text)
        {
            Id = id;
            TreatmentId = treatmentId;
            Text = text;
        }

        public Operation(int treatmentId, string text) : this(-1, treatmentId, text) { }
    }
}
