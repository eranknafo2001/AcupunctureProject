using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcupunctureProject.database
{
    public struct ValueInfo<T>
    {
        public T Value { get; set; }
        public string Info { get; set; }
        public ValueInfo(T value,string info)
        {
            Value = value;
            Info = info;
        }
    }

    public enum PreferColdOrHotType
    {
        HOT,COLD,NIETHER
    }

    public class Diagnostic
    {
        #region all the variables
        public readonly int Id;
        public string Profession { get; set; }
        public string MainComplaint { get; set; }
        public string SeconderyComlaint { get; set; }
        public string DrugsUsed { get; set; }
        public string TestsMade { get; set; }
        public ValueInfo<bool> Pain { get; set; }
        public ValueInfo<bool> PainPreviousEvaluations { get; set; }
        public ValueInfo<bool> Scans { get; set; }
        public ValueInfo<bool> UnderStress { get; set; }
        public ValueInfo<bool> TenseMuscles { get; set; }
        public ValueInfo<bool> HighBloodPressureOrColesterol { get; set; }
        public ValueInfo<bool> GoodSleep { get; set; }
        public ValueInfo<bool> FallenToSleepProblem { get; set; }
        public ValueInfo<bool> Palpitations { get; set; }
        public string DefeationRegularity { get; set; }
        public ValueInfo<bool> FatigueOrFeelsFulAfterEating { get; set; }
        public ValueInfo<bool> DesireForSweetsAfterEating { get; set; }
        public ValueInfo<bool> DifficultyConcentating { get; set; }
        public ValueInfo<bool> OftenIll { get; set; }
        public ValueInfo<bool> SufferingFromMucus { get; set; }
        public ValueInfo<bool> CoughOrAllergySuffers { get; set; }
        public ValueInfo<bool> Smoking { get; set; }
        public ValueInfo<bool> FrequentOrUrgentUrination { get; set; }
        public ValueInfo<PreferColdOrHotType> PreferColdOrHot { get; set; }
        public ValueInfo<bool> SuffersFromColdOrHot { get; set; }
        public ValueInfo<bool> SatisfiedDients { get; set; }
        public ValueInfo<bool> WantToLostWeight { get; set; }
        public ValueInfo<bool> UsingContraception { get; set; }
        public ValueInfo<bool> CycleRegular { get; set; }
        public ValueInfo<bool> SufferingFromCrampsOrNervousBeforeMenstruation { get; set; }
        public ValueInfo<bool> SufferingFromMenpause { get; set; }
        public string HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife { get; set; }
        public string WhatChangesDoYouExpectToSeeFromTreatment { get; set; }
        public int PatientId { get; set; }
        public DateTime CreationDate { get; set; }
        #endregion
        #region names of the variables
        public static readonly string PROFESSION = "PROFESSION";
        public static readonly string MAIN_COMPLAINT = "MAIN_COMPLAINT";
        public static readonly string SECONDERY_COMPLAINT = "SECONDERY_COMPLAINT";
        public static readonly string DRUGS_USED = "DRUGS_USED";
        public static readonly string TESTS_MADE = "TESTS_MADE";
        public static readonly string IN_PAIN = "IN_PAIN";
        public static readonly string PAIN_INFO = "PAIN_INFO";
        public static readonly string IS_PAIN_PREVIOUS_EVALUATIONS = "IS_PAIN_PREVIOUS_EVALUATIONS";
        public static readonly string PAIN_PREVIOUS_EVALUATION_INFO = "PAIN_PREVIOUS_EVALUATION_INFO";
        public static readonly string IS_THERE_ANYSORT_OF_SCANS = "IS_THERE_ANYSORT_OF_SCANS";
        public static readonly string THE_SCANS_INFO = "THE_SCANS_INFO";
        public static readonly string IS_UNDER_STRESS = "IS_UNDER_STRESS";
        public static readonly string STRESS_INFO = "STRESS_INFO";
        public static readonly string IS_TENSE_MUSCLES = "IS_TENSE_MUSCLES";
        public static readonly string TENSE_MUSCLES_INFO = "TENSE_MUSCLES_INFO";
        public static readonly string IS_HIGH_BLOOD_PRESSURE_OR_COLESTEROL = "IS_HIGH_BLOOD_PRESSURE_OR_COLESTEROL";
        public static readonly string HIGH_BLOOD_PRESSURE_OR_COLESTEROL_INFO = "HIGH_BLOOD_PRESSURE_OR_COLESTEROL_INFO";
        public static readonly string IS_GOOD_SLEEP = "IS_GOOD_SLEEP";
        public static readonly string GOOD_SLEEP_INFO = "GOOD_SLEEP_INFO";
        public static readonly string IS_FALLEN_TO_SLEEP_PROBLEM = "IS_FALLEN_TO_SLEEP_PROBLEM";
        public static readonly string FALLEN_TO_SLEEP_PROBLEM_INFO = "FALLEN_TO_SLEEP_PROBLEM_INFO";
        public static readonly string IS_PALPITATIONS = "IS_PALPITATIONS";
        public static readonly string PALPITATIONS_INFO = "PALPITATIONS_INFO";
        public static readonly string DEFECATION_REGULARITY = "DEFECATION_REGULARITY";
        public static readonly string IS_FATIGUE_OR_FEELS_FULL_AFTER_EATING = "IS_FATIGUE_OR_FEELS_FULL_AFTER_EATING";
        public static readonly string FATIGUE_OR_FEELS_FULL_AFTER_EATING_INFO = "FATIGUE_OR_FEELS_FULL_AFTER_EATING_INFO";
        public static readonly string IS_DESIRE_FOR_SWEETS_AFTER_EATING = "IS_DESIRE_FOR_SWEETS_AFTER_EATING";
        public static readonly string DESIRE_FOR_SWEETS_AFTER_EATING_INFO = "DESIRE_FOR_SWEETS_AFTER_EATING_INFO";
        public static readonly string IS_DIFFICULTY_CONCENTRATING = "IS_DIFFICULTY_CONCENTRATING";
        public static readonly string DIFFICULTY_CONCENTRATING_INFO = "DIFFICULTY_CONCENTRATING_INFO";
        public static readonly string IS_OFTEN_ILL = "IS_OFTEN_ILL";
        public static readonly string OFTEN_ILL_INFO = "OFTEN_ILL_INFO";
        public static readonly string IS_SUFFERING_FROM_MUCUS = "IS_SUFFERING_FROM_MUCUS";
        public static readonly string SUFFERING_FROM_MUCUS_INFO = "SUFFERING_FROM_MUCUS_INFO";
        public static readonly string IS_COUGH_OR_ALLERGY_SUFFERS = "IS_COUGH_OR_ALLERGY_SUFFERS";
        public static readonly string COUGH_OR_ALLERGY_SUFFERS_INFO = "COUGH_OR_ALLERGY_SUFFERS_INFO";
        public static readonly string IS_SMOKING = "IS_SMOKING";
        public static readonly string SMOKING_INFO = "SMOKING_INFO";
        public static readonly string IS_FREQUENT_OR_URGENT_URINATION = "IS_FREQUENT_OR_URGENT_URINATION";
        public static readonly string FREQUENT_OR_URGENT_URINATION_INFO = "FREQUENT_OR_URGENT_URINATION_INFO";
        public static readonly string PREFER_COLD_OR_HOT = "PREFER_COLD_OR_HOT";
        public static readonly string PREFER_COLD_OR_HOT_INFO = "PREFER_COLD_OR_HOT_INFO";
        public static readonly string IS_SUFFERS_FROM_COLD_OR_HOT = "IS_SUFFERS_FROM_COLD_OR_HOT";
        public static readonly string SUFFERS_FROM_COLD_OR_HOT_INFO = "SUFFERS_FROM_COLD_OR_HOT_INFO";
        public static readonly string IS_SATISFIED_DIETS = "IS_SATISFIED_DIETS";
        public static readonly string SATISFIED_DIETS_INFO = "SATISFIED_DIETS_INFO";
        public static readonly string IS_WANT_TO_LOST_WEIGHT = "IS_WANT_TO_LOST_WEIGHT";
        public static readonly string WANT_TO_LOST_WEIGHT_INFO = "WANT_TO_LOST_WEIGHT_INFO";
        public static readonly string IS_USING_CONTRACEPTION = "IS_USING_CONTRACEPTION";
        public static readonly string USING_CONTRACEPTION_INFO = "USING_CONTRACEPTION_INFO";
        public static readonly string IS_CYCLE_REGULAR = "IS_CYCLE_REGULAR";
        public static readonly string CYCLE_REGULAR_INFO = "CYCLE_REGULAR_INFO";
        public static readonly string IS_SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION = "IS_SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION";
        public static readonly string SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION_INFO = "SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION_INFO";
        public static readonly string IS_SUFFERING_FROM_MENOPAUSE = "IS_SUFFERING_FROM_MENOPAUSE";
        public static readonly string SUFFERING_FROM_MENOPAUSE_INFO = "SUFFERING_FROM_MENOPAUSE_INFO";
        public static readonly string HOW_MANY_HOURS_A_WEEK_ARE_YOU_WILLING_TO_INVEST_TO_IMPROVE_THE_QUALITY_OF_LIFE = "HOW_MANY_HOURS_A_WEEK_ARE_YOU_WILLING_TO_INVEST_TO_IMPROVE_THE_QUALITY_OF_LIFE";
        public static readonly string WHAT_CHANGES_DO_YOU_EXPECT_TO_SEE_FROM_TREATMENT = "WHAT_CHANGES_DO_YOU_EXPECT_TO_SEE_FROM_TREATMENT";
        public static readonly string PATIENT_ID = "PATIENT_ID";
        public static readonly string CREATION_DATE = "CREATION_DATE";
        #endregion

        public Diagnostic(int id = -1 , Diagnostic diagnostic = null)
        {
            Id = id;
            if(diagnostic != null)
            {
                Profession = diagnostic.Profession;
                MainComplaint = diagnostic.MainComplaint;
                SeconderyComlaint = diagnostic.SeconderyComlaint;
                DrugsUsed = diagnostic.DrugsUsed;
                TestsMade = diagnostic.TestsMade;
                Pain = diagnostic.Pain;
                PainPreviousEvaluations = diagnostic.PainPreviousEvaluations;
                Scans = diagnostic.Scans;
                UnderStress = diagnostic.UnderStress;
                TenseMuscles = diagnostic.TenseMuscles;
                HighBloodPressureOrColesterol = diagnostic.HighBloodPressureOrColesterol;
                GoodSleep = diagnostic.GoodSleep;
                FallenToSleepProblem = diagnostic.FallenToSleepProblem;
                Palpitations = diagnostic.Palpitations;
                DefeationRegularity = diagnostic.DefeationRegularity;
                FatigueOrFeelsFulAfterEating = diagnostic.FatigueOrFeelsFulAfterEating;
                DesireForSweetsAfterEating = diagnostic.DesireForSweetsAfterEating;
                DifficultyConcentating = diagnostic.DifficultyConcentating;
                OftenIll = diagnostic.OftenIll;
                SufferingFromMucus = diagnostic.SufferingFromMucus;
                CoughOrAllergySuffers = diagnostic.CoughOrAllergySuffers;
                Smoking = diagnostic.Smoking;
                FrequentOrUrgentUrination = diagnostic.FrequentOrUrgentUrination;
                PreferColdOrHot = diagnostic.PreferColdOrHot;
                SuffersFromColdOrHot = diagnostic.SuffersFromColdOrHot;
                SatisfiedDients = diagnostic.SatisfiedDients;
                WantToLostWeight = diagnostic.WantToLostWeight;
                UsingContraception = diagnostic.UsingContraception;
                CycleRegular = diagnostic.CycleRegular;
                SufferingFromCrampsOrNervousBeforeMenstruation = diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation;
                SufferingFromMenpause = diagnostic.SufferingFromMenpause;
                HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife = diagnostic.HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
                WhatChangesDoYouExpectToSeeFromTreatment = diagnostic.WhatChangesDoYouExpectToSeeFromTreatment;
                CreationDate = diagnostic.CreationDate;
            }
        }
    }
}