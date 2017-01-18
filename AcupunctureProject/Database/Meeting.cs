﻿using System;
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
            public static readonly ResultValue BETTER = new ResultValue(0), WORSE = new ResultValue(1), NO_CHANGE = new ResultValue(2);

            public int Value { get; private set; }
            public static ResultValue FromValue(int value)
            {
                if (value < 0 || value > 2)
                    throw new Exception("ERROR::ResultValue is not exist");
                return new ResultValue(value);
            }

            private ResultValue(int value)
            {
                this.Value = value;
            }

            public override string ToString()
            {
                if (Value == 0)
                    return "יותר טוב";
                else if (Value == 1)
                    return "החמיר";
                else
                    return "אין שינוי";
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
                return "test";
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