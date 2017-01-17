using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.Database
{
    public class Point
    {
        public static readonly string MIN_NEEDLE_DEPTH = "MIN_NEEDLE_DEPTH";
        public static readonly string MAX_NEEDLE_DEPTH = "MAX_NEEDLE_DEPTH";
        public static readonly string NEEDLE_DESCRIPTION = "NEEDLE_DESCRIPTION";
        public static readonly string POSITION = "POSITION";
        public static readonly string IMPORTENCE = "IMPORTENCE";
        public static readonly string COMMENT1 = "COMMENT1";
        public static readonly string COMMENT2 = "COMMENT2";
        public static readonly string NAME = "NAME";
        public static readonly string NOTE = "NOTE";
        public static readonly string IMAGE = "IMAGE";

        public int Id { get; private set; }
        public string Name { get; set; }
        public int MinNeedleDepth { get; set; }
        public int MaxNeedleDepth { get; set; }
        public string NeedleDescription { get; set; }
        public string Position { get; set; }
        public int Importance { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Note { get; set; }
        public string Image { get; set; }

        public Point(int id, string name, int minNeedleDepth, int maxNeedleDepth, string needleDescription, string position,
                int importance, string comment1, string comment2, string note, string image)
        {
            this.Id = id;
            this.Name = name;
            this.MinNeedleDepth = minNeedleDepth;
            this.MaxNeedleDepth = maxNeedleDepth;
            this.NeedleDescription = needleDescription;
            this.Position = position;
            this.Importance = importance;
            this.Comment1 = comment1;
            this.Comment2 = comment2;
            this.Note = note;
            this.Image = image;
        }

        public Point(string name, int minNeedleDepth, int maxNeedleDepth, string needleDescription, string position, int importance, string comment1, string comment2, string note, string image) : this(-1, name, minNeedleDepth, maxNeedleDepth, needleDescription, position, importance, comment1, comment2, note, image)
        {
        }

        public Point(int id, Point other) : this(id, other.Name, other.MinNeedleDepth, other.MaxNeedleDepth, other.NeedleDescription, other.Position,
                    other.Importance, other.Comment1, other.Comment2, other.Note, other.Image)
        {

        }
    }
}
