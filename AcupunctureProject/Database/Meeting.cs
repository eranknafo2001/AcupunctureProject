using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace AcupunctureProject.Database
{
    public class Meeting
    {
        public static readonly string PATIENT_ID = "PATIENT_ID";
        public static readonly string PURPOSE = "PURPOSE";
        public static readonly string DATE = "DATE";
        public static readonly string DESCRIPTION = "DESCRIPTION";
        public static readonly string SUMMERY = "SUMMERY";
        public static readonly string RESULT_DESCRIPTION = "RESULT_DESCRIPTION";
        public static readonly string RESULT_VALUE = "RESULT_VALUE";

        public class ResultValue
        {
            public static readonly ResultValue BETTER = new ResultValue(0), WORSE = new ResultValue(1), NO_CHANGE = new ResultValue(2);

            private int value;

            private ResultValue(int value)
            {
                this.value = value;
            }

            public int getValue()
            {
                return value;
            }

            public string tostring()
            {
                if (value == 0)
                {
                    return "יותר טוב";
                }
                else if (value == 1)
                {
                    return "החמיר";
                }
                else {
                    return "אין שינוי";
                }
            }
        };

        public int Id { get; private set; }
        public int PatiantId { get; set; }
        public string Purpose { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Summery { get; set; }
        public string ResultDescription { get; set; }
        public ResultValue Result { get; set; }

        public Meeting(int id, int patiantId, string purpose, DateTime date, string description, string summery,
                string resultDescription, ResultValue resultValue)
        {
            this.Id = id;
            this.PatiantId = patiantId;
            this.Purpose = purpose;
            this.Date = date;
            this.Description = description;
            this.Summery = summery;
            this.ResultDescription = resultDescription;
            this.Result = resultValue;
        }

        public Meeting(int id, Meeting other)
        {
            this.Id = id;
            this.PatiantId = other.PatiantId;
            this.Purpose = other.Purpose;
            this.Date = other.Date;
            this.Description = other.Description;
            this.Summery = other.Summery;
            this.ResultDescription = other.ResultDescription;
            this.Result = other.Result;
        }

        public Meeting(int patiantId, string purpose, DateTime date, string description, string summery, string resultDescription, ResultValue resultValue)
        {
            this.Id = -1;
            this.PatiantId = patiantId;
            this.Purpose = purpose;
            this.Date = date;
            this.Description = description;
            this.Summery = summery;
            this.ResultDescription = resultDescription;
            this.Result = resultValue;
        }
    }
}
