using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.GUI.Exceptions
{
    class NotFullException
    {
        NotFullErorCode ErorCode;

        public NotFullException()
        {
            
        }
    }

    public enum NotFullErorCode
    {
        name=1,
        berthday=2,
        address=3,
        cellphone=4,
        telephone=5,
        email=6,
        gender=7,
        medicalHistory=8
    }
}
