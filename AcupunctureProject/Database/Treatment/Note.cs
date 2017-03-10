using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.Database.Treatment
{
    public class Note
    {
        public int RelationId { get; private set; }
        public TableRelationType TableRelation { get; private set; }
        public string Text { get; set; }

        public Note(int relationId, TableRelationType tableRelation, string text)
        {
            RelationId = relationId;
            TableRelation = tableRelation;
            Text = text;
        }
    }
}
