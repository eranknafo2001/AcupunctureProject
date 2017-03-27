using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("the movie length : ");
            int lengthInMin = int.Parse(Console.ReadLine());
            int min = lengthInMin % 60;
            int hour = lengthInMin / 60;
            if (hour != 0)
                Console.Write("{0} hour", hour);
            if (hour != 0 && min != 0)
                Console.Write(" and ");
            if (min != 0)
                Console.Write("{0} minutes", min);
            Console.Write("\n");
        }
    }
}
