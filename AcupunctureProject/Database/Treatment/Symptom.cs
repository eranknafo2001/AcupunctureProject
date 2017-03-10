using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.Database.Treatment
{
    public enum TableRelationType
    {
        TREATMENT, OPERATION, POINT,
    }
    public class Symptom
    {
        public int RelationId { get; private set; }
        public TableRelationType TableRelation { get; private set; }
        public string Note { get; set; }

        public Symptom(int relationId, TableRelationType tableRelation, string note)
        {
            RelationId = relationId;
            TableRelation = tableRelation;
            Note = note;
        }
    }
}
