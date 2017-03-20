using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions.TextBlob;
namespace AcupunctureProject.Database
{
    [Serializable]
    public class ValueInfo<T>
    {
        public T Value {get;set;}
        public string Info {get;set;}

        public ValueInfo(T value, string info)
        {
            Value = value;
            Info = info;
        }
    }

    public enum PreferColdOrHotType
    {
        HOT, COLD, NIETHER
    }

    public class Diagnostic : ITable
    {
        #region all the variables
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Profession { get; set; }
        public string MainComplaint { get; set; }
        public string SeconderyComlaint { get; set; }
        public string DrugsUsed { get; set; }
        public string TestsMade { get; set; }
        [TextBlob("Pain")]
        public ValueInfo<bool?> Pain { get; set; }
        [TextBlob("PainPreviousEvaluations")]
        public ValueInfo<bool?> PainPreviousEvaluations { get; set; }
        [TextBlob("Scans")]
        public ValueInfo<bool?> Scans { get; set; }
        [TextBlob("UnderStress")]
        public ValueInfo<bool?> UnderStress { get; set; }
        [TextBlob("TenseMuscles")]
        public ValueInfo<bool?> TenseMuscles { get; set; }
        [TextBlob("HighBloodPressureOrColesterol")]
        public ValueInfo<bool?> HighBloodPressureOrColesterol { get; set; }
        [TextBlob("GoodSleep")]
        public ValueInfo<bool?> GoodSleep { get; set; }
        [TextBlob("FallenToSleepProblem")]
        public ValueInfo<bool?> FallenToSleepProblem { get; set; }
        [TextBlob("Palpitations")]
        public ValueInfo<bool?> Palpitations { get; set; }
        public string DefeationRegularity { get; set; }
        [TextBlob("FatigueOrFeelsFulAfterEating")]
        public ValueInfo<bool?> FatigueOrFeelsFulAfterEating { get; set; }
        [TextBlob("DesireForSweetsAfterEating")]
        public ValueInfo<bool?> DesireForSweetsAfterEating { get; set; }
        [TextBlob("DifficultyConcentating")]
        public ValueInfo<bool?> DifficultyConcentating { get; set; }
        [TextBlob("OftenIll")]
        public ValueInfo<bool?> OftenIll { get; set; }
        [TextBlob("SufferingFromMucus")]
        public ValueInfo<bool?> SufferingFromMucus { get; set; }
        [TextBlob("CoughOrAllergySuffers")]
        public ValueInfo<bool?> CoughOrAllergySuffers { get; set; }
        [TextBlob("Smoking")]
        public ValueInfo<bool?> Smoking { get; set; }
        [TextBlob("FrequentOrUrgentUrination")]
        public ValueInfo<bool?> FrequentOrUrgentUrination { get; set; }
        [TextBlob("PreferColdOrHot")]
        public ValueInfo<PreferColdOrHotType> PreferColdOrHot { get; set; }
        [TextBlob("SuffersFromColdOrHot")]
        public ValueInfo<bool?> SuffersFromColdOrHot { get; set; }
        [TextBlob("SatisfiedDients")]
        public ValueInfo<bool?> SatisfiedDients { get; set; }
        [TextBlob("WantToLostWeight")]
        public ValueInfo<bool?> WantToLostWeight { get; set; }
        [TextBlob("UsingContraception")]
        public ValueInfo<bool?> UsingContraception { get; set; }
        [TextBlob("CycleRegular")]
        public ValueInfo<bool?> CycleRegular { get; set; }
        [TextBlob("SufferingFromCrampsOrNervousBeforeMenstruation")]
        public ValueInfo<bool?> SufferingFromCrampsOrNervousBeforeMenstruation { get; set; }
        [TextBlob("SufferingFromMenpause")]
        public ValueInfo<bool?> SufferingFromMenpause { get; set; }
        public string HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife { get; set; }
        public string WhatChangesDoYouExpectToSeeFromTreatment { get; set; }
        [ForeignKey(typeof(Patient))]
        public int PatientId { get; set; }
        [ManyToOne]
        public Patient Patient { get; set; }
        public DateTime CreationDate { get; set; }
        #endregion
    }
}