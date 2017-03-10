using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.Database
{
    public class Channel
    {
        public static readonly string NAME = "NAME";
        public static readonly string RT = "RT";
        public static readonly string MAIN_POINT = "MAIN_POINT";
        public static readonly string EVEN_POINT = "EVEN_POINT";
        public static readonly string PATH = "PATH";
        public static readonly string ROLE = "ROLE";
        public static readonly string COMMENT = "COMMENT";

        public int Id { get; private set; }
        public string Name { get; set; }
        public string Rt { get; set; }
        public int MainPoint { get; set; }
        public int EvenPoint { get; set; }
        public string Path { get; set; }
        public string Role { get; set; }
        public string Comments { get; set; }

        public Channel(int id, string name, string rt, int mainPoint, int evenPoint, string path, string role,
                string comments)
        {
            this.Id = id;
            this.Name = name;
            this.Rt = rt;
            this.MainPoint = mainPoint;
            this.EvenPoint = evenPoint;
            this.Path = path;
            this.Role = role;
            this.Comments = comments;
        }

        public override string ToString()
        {
            return Id + " - " + Rt + " (" + Name + ")";
        }
    }
}
