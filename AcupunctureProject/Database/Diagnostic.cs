using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AcupunctureProject.Database
{
    public class ValueInfo<T> : INotifyPropertyChanged
    {
        private T _Value;
        public T Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                OnPropertyChanged("Value");
            }
        }
        private string _Info;
        public string Info
        {
            get
            {
                return _Info;
            }
            set
            {
                _Info = value;
                OnPropertyChanged("Value");
            }
        }

        public ValueInfo(T value, string info)
        {
            _Value = value;
            _Info = info;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public enum PreferColdOrHotType
    {
        HOT, COLD, NIETHER
    }

    public class Diagnostic : INotifyPropertyChanged
    {
        #region all the variables
        public readonly int Id;
        string _Profession;
        public string Profession
        {
            get
            {
                return _Profession;
            }
            set
            {
                _Profession = value;
                OnPropertyChanged("Profession");
            }
        }
        private string _MainComplaint;
        public string MainComplaint
        {
            get
            {
                return _MainComplaint;
            }
            set
            {
                _MainComplaint = value;
                OnPropertyChanged("MainComplaint");
            }
        }
        private string _SeconderyComlaint;
        public string SeconderyComlaint
        {
            get
            {
                return _SeconderyComlaint;
            }
            set
            {
                _SeconderyComlaint = value;
                OnPropertyChanged("SeconderyComlaint");
            }
        }
        private string _DrugsUsed;
        public string DrugsUsed
        {
            get
            {
                return _DrugsUsed;
            }
            set
            {
                _DrugsUsed = value;
                OnPropertyChanged("DrugsUsed");
            }
        }
        private string _TestsMade;
        public string TestsMade
        {
            get
            {
                return _TestsMade;
            }
            set
            {
                _TestsMade = value;
                OnPropertyChanged("TestsMade");
            }
        }
        private ValueInfo<bool?> _Pain;
        public ValueInfo<bool?> Pain
        {
            get
            {
                return _Pain;
            }
            set
            {
                _Pain = value;
                OnPropertyChanged("Pain");
            }
        }
        private ValueInfo<bool?> _PainPreviousEvaluations;
        public ValueInfo<bool?> PainPreviousEvaluations
        {
            get
            {
                return _PainPreviousEvaluations;
            }
            set
            {
                _PainPreviousEvaluations = value;
                OnPropertyChanged("PainPreviousEvaluations");
            }
        }
        private ValueInfo<bool?> _Scans;
        public ValueInfo<bool?> Scans
        {
            get
            {
                return _Scans;
            }
            set
            {
                _Scans = value;
                OnPropertyChanged("Scans");
            }
        }
        private ValueInfo<bool?> _UnderStress;
        public ValueInfo<bool?> UnderStress
        {
            get
            {
                return _UnderStress;
            }
            set
            {
                _UnderStress = value;
                OnPropertyChanged("UnderStress");
            }
        }
        private ValueInfo<bool?> _TenseMuscles;
        public ValueInfo<bool?> TenseMuscles
        {
            get
            {
                return _TenseMuscles;
            }
            set
            {
                _TenseMuscles = value;
                OnPropertyChanged("TenseMuscles");
            }
        }
        private ValueInfo<bool?> _HighBloodPressureOrColesterol;
        public ValueInfo<bool?> HighBloodPressureOrColesterol
        {
            get
            {
                return _HighBloodPressureOrColesterol;
            }
            set
            {
                _HighBloodPressureOrColesterol = value;
                OnPropertyChanged("HighBloodPressureOrColesterol");
            }
        }
        private ValueInfo<bool?> _GoodSleep;
        public ValueInfo<bool?> GoodSleep
        {
            get
            {
                return _GoodSleep;
            }
            set
            {
                _GoodSleep = value;
                OnPropertyChanged("GoodSleep");
            }
        }
        private ValueInfo<bool?> _FallenToSleepProblem;
        public ValueInfo<bool?> FallenToSleepProblem
        {
            get
            {
                return _FallenToSleepProblem;
            }
            set
            {
                _FallenToSleepProblem = value;
                OnPropertyChanged("FallenToSleepProblem");
            }
        }
        private ValueInfo<bool?> _Palpitations;
        public ValueInfo<bool?> Palpitations
        {
            get
            {
                return _Palpitations;
            }
            set
            {
                _Palpitations = value;
                OnPropertyChanged("Palpitations");
            }
        }
        private string _DefeationRegularity;
        public string DefeationRegularity
        {
            get
            {
                return _DefeationRegularity;
            }
            set
            {
                _DefeationRegularity = value;
                OnPropertyChanged("DefeationRegularity");
            }
        }
        private ValueInfo<bool?> _FatigueOrFeelsFulAfterEating;
        public ValueInfo<bool?> FatigueOrFeelsFulAfterEating
        {
            get
            {
                return _FatigueOrFeelsFulAfterEating;
            }
            set
            {
                _FatigueOrFeelsFulAfterEating = value;
                OnPropertyChanged("FatigueOrFeelsFulAfterEating");
            }
        }
        private ValueInfo<bool?> _DesireForSweetsAfterEating;
        public ValueInfo<bool?> DesireForSweetsAfterEating
        {
            get
            {
                return _DesireForSweetsAfterEating;
            }
            set
            {
                _DesireForSweetsAfterEating = value;
                OnPropertyChanged("DesireForSweetsAfterEating");
            }
        }
        private ValueInfo<bool?> _DifficultyConcentating;
        public ValueInfo<bool?> DifficultyConcentating
        {
            get
            {
                return _DifficultyConcentating;
            }
            set
            {
                _DifficultyConcentating = value;
                OnPropertyChanged("DifficultyConcentating");
            }
        }
        private ValueInfo<bool?> _OftenIll;
        public ValueInfo<bool?> OftenIll
        {
            get
            {
                return _OftenIll;
            }
            set
            {
                _OftenIll = value;
                OnPropertyChanged("OftenIll");
            }
        }
        private ValueInfo<bool?> _SufferingFromMucus;
        public ValueInfo<bool?> SufferingFromMucus
        {
            get
            {
                return _SufferingFromMucus;
            }
            set
            {
                _SufferingFromMucus = value;
                OnPropertyChanged("SufferingFromMucus");
            }
        }
        private ValueInfo<bool?> _CoughOrAllergySuffers;
        public ValueInfo<bool?> CoughOrAllergySuffers
        {
            get
            {
                return _CoughOrAllergySuffers;
            }
            set
            {
                _CoughOrAllergySuffers = value;
                OnPropertyChanged("CoughOrAllergySuffers");
            }
        }
        private ValueInfo<bool?> _Smoking;
        public ValueInfo<bool?> Smoking
        {
            get
            {
                return _Smoking;
            }
            set
            {
                _Smoking = value;
                OnPropertyChanged("Smoking");
            }
        }
        private ValueInfo<bool?> _FrequentOrUrgentUrination;
        public ValueInfo<bool?> FrequentOrUrgentUrination
        {
            get
            {
                return _FrequentOrUrgentUrination;
            }
            set
            {
                _FrequentOrUrgentUrination = value;
                OnPropertyChanged("FrequentOrUrgentUrination");
            }
        }
        private ValueInfo<PreferColdOrHotType> _PreferColdOrHot;
        public ValueInfo<PreferColdOrHotType> PreferColdOrHot
        {
            get
            {
                return _PreferColdOrHot;
            }
            set
            {
                _PreferColdOrHot = value;
                OnPropertyChanged("PreferColdOrHot");
            }
        }
        private ValueInfo<bool?> _SuffersFromColdOrHot;
        public ValueInfo<bool?> SuffersFromColdOrHot
        {
            get
            {
                return _SuffersFromColdOrHot;
            }
            set
            {
                _SuffersFromColdOrHot = value;
                OnPropertyChanged("SuffersFromColdOrHot");
            }
        }
        private ValueInfo<bool?> _SatisfiedDients;
        public ValueInfo<bool?> SatisfiedDients
        {
            get
            {
                return _SatisfiedDients;
            }
            set
            {
                _SatisfiedDients = value;
                OnPropertyChanged("SatisfiedDients");
            }
        }
        private ValueInfo<bool?> _WantToLostWeight;
        public ValueInfo<bool?> WantToLostWeight
        {
            get
            {
                return _WantToLostWeight;
            }
            set
            {
                _WantToLostWeight = value;
                OnPropertyChanged("WantToLostWeight");
            }
        }
        private ValueInfo<bool?> _UsingContraception;
        public ValueInfo<bool?> UsingContraception
        {
            get
            {
                return _UsingContraception;
            }
            set
            {
                _UsingContraception = value;
                OnPropertyChanged("UsingContraception");
            }
        }
        private ValueInfo<bool?> _CycleRegular;
        public ValueInfo<bool?> CycleRegular
        {
            get
            {
                return _CycleRegular;
            }
            set
            {
                _CycleRegular = value;
                OnPropertyChanged("CycleRegular");
            }
        }
        private ValueInfo<bool?> _SufferingFromCrampsOrNervousBeforeMenstruation;
        public ValueInfo<bool?> SufferingFromCrampsOrNervousBeforeMenstruation
        {
            get
            {
                return _SufferingFromCrampsOrNervousBeforeMenstruation;
            }
            set
            {
                _SufferingFromCrampsOrNervousBeforeMenstruation = value;
                OnPropertyChanged("SufferingFromCrampsOrNervousBeforeMenstruation");
            }
        }
        private ValueInfo<bool?> _SufferingFromMenpause;
        public ValueInfo<bool?> SufferingFromMenpause
        {
            get
            {
                return _SufferingFromMenpause;
            }
            set
            {
                _SufferingFromMenpause = value;
                OnPropertyChanged("SufferingFromMenpause");
            }
        }
        private string _HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
        public string HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife
        {
            get
            {
                return _HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
            }
            set
            {
                _HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife = value;
                OnPropertyChanged("HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife");
            }
        }
        private string _WhatChangesDoYouExpectToSeeFromTreatment;
        public string WhatChangesDoYouExpectToSeeFromTreatment
        {
            get
            {
                return _WhatChangesDoYouExpectToSeeFromTreatment;
            }
            set
            {
                _WhatChangesDoYouExpectToSeeFromTreatment = value;
                OnPropertyChanged("WhatChangesDoYouExpectToSeeFromTreatment");
            }
        }
        private int _PatientId;
        public int PatientId
        {
            get
            {
                return _PatientId;
            }
            set
            {
                _PatientId = value;
                OnPropertyChanged("PatientId");
            }
        }
        private DateTime _CreationDate;
        public DateTime CreationDate
        {
            get
            {
                return _CreationDate;
            }
            set
            {
                _CreationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }
        public string CreationDateString
        {
            get
            {
                return _CreationDate.ToShortDateString();
            }
        }
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

        public Diagnostic(int id = -1, Diagnostic diagnostic = null)
        {
            Id = id;
            if (diagnostic != null)
            {
                _Profession = diagnostic.Profession;
                _MainComplaint = diagnostic.MainComplaint;
                _SeconderyComlaint = diagnostic.SeconderyComlaint;
                _DrugsUsed = diagnostic.DrugsUsed;
                _TestsMade = diagnostic.TestsMade;
                _Pain = diagnostic.Pain;
                _PainPreviousEvaluations = diagnostic.PainPreviousEvaluations;
                _Scans = diagnostic.Scans;
                _UnderStress = diagnostic.UnderStress;
                _TenseMuscles = diagnostic.TenseMuscles;
                _HighBloodPressureOrColesterol = diagnostic.HighBloodPressureOrColesterol;
                _GoodSleep = diagnostic.GoodSleep;
                _FallenToSleepProblem = diagnostic.FallenToSleepProblem;
                _Palpitations = diagnostic.Palpitations;
                _DefeationRegularity = diagnostic.DefeationRegularity;
                _FatigueOrFeelsFulAfterEating = diagnostic.FatigueOrFeelsFulAfterEating;
                _DesireForSweetsAfterEating = diagnostic.DesireForSweetsAfterEating;
                _DifficultyConcentating = diagnostic.DifficultyConcentating;
                _OftenIll = diagnostic.OftenIll;
                _SufferingFromMucus = diagnostic.SufferingFromMucus;
                _CoughOrAllergySuffers = diagnostic.CoughOrAllergySuffers;
                _Smoking = diagnostic.Smoking;
                _FrequentOrUrgentUrination = diagnostic.FrequentOrUrgentUrination;
                _PreferColdOrHot = diagnostic.PreferColdOrHot;
                _SuffersFromColdOrHot = diagnostic.SuffersFromColdOrHot;
                _SatisfiedDients = diagnostic.SatisfiedDients;
                _WantToLostWeight = diagnostic.WantToLostWeight;
                _UsingContraception = diagnostic.UsingContraception;
                _CycleRegular = diagnostic.CycleRegular;
                _SufferingFromCrampsOrNervousBeforeMenstruation = diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation;
                _SufferingFromMenpause = diagnostic.SufferingFromMenpause;
                _HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife = diagnostic.HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
                _WhatChangesDoYouExpectToSeeFromTreatment = diagnostic.WhatChangesDoYouExpectToSeeFromTreatment;
                _CreationDate = diagnostic.CreationDate;
            }
            else
            {
                _Pain = new ValueInfo<bool?>(null, "");
                _PainPreviousEvaluations = new ValueInfo<bool?>(null, "");
                _Scans = new ValueInfo<bool?>(null, "");
                _UnderStress = new ValueInfo<bool?>(null, "");
                _TenseMuscles = new ValueInfo<bool?>(null, "");
                _HighBloodPressureOrColesterol = new ValueInfo<bool?>(null, "");
                _GoodSleep = new ValueInfo<bool?>(null, "");
                _FallenToSleepProblem = new ValueInfo<bool?>(null, "");
                _Palpitations = new ValueInfo<bool?>(null, "");
                _FatigueOrFeelsFulAfterEating = new ValueInfo<bool?>(null, "");
                _DesireForSweetsAfterEating = new ValueInfo<bool?>(null, "");
                _DifficultyConcentating = new ValueInfo<bool?>(null, "");
                _OftenIll = new ValueInfo<bool?>(null, "");
                _SufferingFromMucus = new ValueInfo<bool?>(null, "");
                _CoughOrAllergySuffers = new ValueInfo<bool?>(null, "");
                _Smoking = new ValueInfo<bool?>(null, "");
                _FrequentOrUrgentUrination = new ValueInfo<bool?>(null, "");
                _PreferColdOrHot = new ValueInfo<PreferColdOrHotType>(PreferColdOrHotType.NIETHER, "");
                _SuffersFromColdOrHot = new ValueInfo<bool?>(null, "");
                _SatisfiedDients = new ValueInfo<bool?>(null, "");
                _WantToLostWeight = new ValueInfo<bool?>(null, "");
                _UsingContraception = new ValueInfo<bool?>(null, "");
                _CycleRegular = new ValueInfo<bool?>(null, "");
                _SufferingFromCrampsOrNervousBeforeMenstruation = new ValueInfo<bool?>(null, "");
                _SufferingFromMenpause = new ValueInfo<bool?>(null, "");
                _Profession = "";
                _MainComplaint = "";
                _SeconderyComlaint = "";
                _DrugsUsed = "";
                _TestsMade = "";
                _DefeationRegularity = "";
                _HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife = "";
                _WhatChangesDoYouExpectToSeeFromTreatment = "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}