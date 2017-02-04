using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject
{
    public static class Version
    {
        public static string Value { get; private set; }

        static Version()
        {
            Value = "beta-0.3.1";
        }
    }
}
