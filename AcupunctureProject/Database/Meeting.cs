using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace AcupunctureProject.database
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
            public static readonly ResultValue NOT_SET = new ResultValue(0);
            public static readonly ResultValue BETTER = new ResultValue(1);
            public static readonly ResultValue WORSE = new ResultValue(2);
            public static readonly ResultValue NO_CHANGE = new ResultValue(3);

            public int Value { get; private set; }
            public static ResultValue FromValue(int value)
            {
                switch (value)
                {
                    case 0:
                        return NOT_SET;
                    case 1:
                        return BETTER;
                    case 2:
                        return WORSE;
                    case 3:
                        return NO_CHANGE;
                    default:
                        throw new Exception("ERROR::ResultValue is not exist");
                }
            }

            private ResultValue(int value)
            {
                this.Value = value;
            }

            public override string ToString()
            {
                switch (Value)
                {
                    case 0:
                        return "לא מוגדר";
                    case 1:
                        return "יותר טוב";
                    case 2:
                        return "החמיר";
                    case 3:
                        return "אין שינוי";
                    default:
                        return "Null";
                }
            }
        };

        public int Id { get; private set; }
        public int PatientId { get; set; }
        public string Purpose { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string SmallDescription
        {
            get
            {
                string[] s = Description.Split(' ');
                if (s.Length <= 20)
                    return Description;
                string o = "";
                for (int i = 0; i < s.Length && i < 20; i++)
                {
                    o += s[i] + " ";
                }
                return o + "...";
            }
        }
        public string DateString
        {
            get
            {
                return Date.ToShortDateString();
            }
        }
        public string Summery { get; set; }
        public string ResultDescription { get; set; }
        public ResultValue Result { get; set; }

        public Meeting(int id, int patiantId, string purpose, DateTime date, string description, string summery, string resultDescription, ResultValue resultValue)
        {
            Id = id;
            PatientId = patiantId;
            Purpose = purpose;
            Date = date;
            Description = description;
            Summery = summery;
            ResultDescription = resultDescription;
            Result = resultValue;
        }

        public Meeting(int id, Meeting other) : this(id, other.PatientId, other.Purpose, other.Date, other.Description, other.Summery, other.ResultDescription, other.Result) { }

        public Meeting(int patiantId, string purpose, DateTime date, string description, string summery, string resultDescription, ResultValue resultValue) : this(-1, patiantId, purpose, date, description, summery, resultDescription, resultValue) { }
    }
}
