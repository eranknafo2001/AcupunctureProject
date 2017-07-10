using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace AcupunctureProject.Database
{
	public static partial class Ex
	{
		public static string MyToString(this PreferColdOrHotType p)
		{
			return "";
		}
	}

	public enum PreferColdOrHotType
	{
		HOT, COLD, NIETHER
	}

	public class Diagnostic : ITable, INotifyPropertyChanged
	{
		#region all of the variables
		private int _Id;
		[PrimaryKey, Unique, AutoIncrement]
		public int Id
		{
			get => _Id;
			set
			{
				if (value != _Id)
				{
					_Id = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _Profession;
		public string Profession
		{
			get => _Profession;
			set
			{
				if (value != _Profession)
				{
					_Profession = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _MainComplaint;
		public string MainComplaint
		{
			get => _MainComplaint;
			set
			{
				if (value != _MainComplaint)
				{
					_MainComplaint = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _SeconderyComlaint;
		public string SeconderyComlaint
		{
			get => _SeconderyComlaint;
			set
			{
				if (value != _SeconderyComlaint)
				{
					_SeconderyComlaint = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _DrugsUsed;
		public string DrugsUsed
		{
			get => _DrugsUsed;
			set
			{
				if (value != _DrugsUsed)
				{
					_DrugsUsed = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _TestsMade;
		public string TestsMade
		{
			get => _TestsMade;
			set
			{
				if (value != _TestsMade)
				{
					_TestsMade = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _PainInfo;
		public string PainInfo
		{
			get => _PainInfo;
			set
			{
				if (value != _PainInfo)
				{
					_PainInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _PainValue;
		public bool? PainValue
		{
			get => _PainValue;
			set
			{
				if (value != _PainValue)
				{
					_PainValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _PainPreviousEvaluationsInfo;
		public string PainPreviousEvaluationsInfo
		{
			get => _PainPreviousEvaluationsInfo;
			set
			{
				if (value != _PainPreviousEvaluationsInfo)
				{
					_PainPreviousEvaluationsInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _PainPreviousEvaluationsValue;
		public bool? PainPreviousEvaluationsValue
		{
			get => _PainPreviousEvaluationsValue;
			set
			{
				if (value != _PainPreviousEvaluationsValue)
				{
					_PainPreviousEvaluationsValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _ScansInfo;
		public string ScansInfo
		{
			get => _ScansInfo;
			set
			{
				if (value != _ScansInfo)
				{
					_ScansInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _ScansValue;
		public bool? ScansValue
		{
			get => _ScansValue;
			set
			{
				if (value != _ScansValue)
				{
					_ScansValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _UnderStressInfo;
		public string UnderStressInfo
		{
			get => _UnderStressInfo;
			set
			{
				if (value != _UnderStressInfo)
				{
					_UnderStressInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _UnderStressValue;
		public bool? UnderStressValue
		{
			get => _UnderStressValue;
			set
			{
				if (value != _UnderStressValue)
				{
					_UnderStressValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _TenseMusclesInfo;
		public string TenseMusclesInfo
		{
			get => _TenseMusclesInfo;
			set
			{
				if (value != _TenseMusclesInfo)
				{
					_TenseMusclesInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _TenseMusclesValue;
		public bool? TenseMusclesValue
		{
			get => _TenseMusclesValue;
			set
			{
				if (value != _TenseMusclesValue)
				{
					_TenseMusclesValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _HighBloodPressureOrColesterolInfo;
		public string HighBloodPressureOrColesterolInfo
		{
			get => _HighBloodPressureOrColesterolInfo;
			set
			{
				if (value != _HighBloodPressureOrColesterolInfo)
				{
					_HighBloodPressureOrColesterolInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _HighBloodPressureOrColesterolValue;
		public bool? HighBloodPressureOrColesterolValue
		{
			get => _HighBloodPressureOrColesterolValue;
			set
			{
				if (value != _HighBloodPressureOrColesterolValue)
				{
					_HighBloodPressureOrColesterolValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _GoodSleepInfo;
		public string GoodSleepInfo
		{
			get => _GoodSleepInfo;
			set
			{
				if (value != _GoodSleepInfo)
				{
					_GoodSleepInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _GoodSleepValue;
		public bool? GoodSleepValue
		{
			get => _GoodSleepValue;
			set
			{
				if (value != _GoodSleepValue)
				{
					_GoodSleepValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _FallenToSleepProblemInfo;
		public string FallenToSleepProblemInfo
		{
			get => _FallenToSleepProblemInfo;
			set
			{
				if (value != _FallenToSleepProblemInfo)
				{
					_FallenToSleepProblemInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _FallenToSleepProblemValue;
		public bool? FallenToSleepProblemValue
		{
			get => _FallenToSleepProblemValue;
			set
			{
				if (value != _FallenToSleepProblemValue)
				{
					_FallenToSleepProblemValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _PalpitationsInfo;
		public string PalpitationsInfo
		{
			get => _PalpitationsInfo;
			set
			{
				if (value != _PalpitationsInfo)
				{
					_PalpitationsInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _PalpitationsValue;
		public bool? PalpitationsValue
		{
			get => _PalpitationsValue;
			set
			{
				if (value != _PalpitationsValue)
				{
					_PalpitationsValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _DefeationRegularity;
		public string DefeationRegularity
		{
			get => _DefeationRegularity;
			set
			{
				if (value != _DefeationRegularity)
				{
					_DefeationRegularity = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _FatigueOrFeelsFulAfterEatingInfo;
		public string FatigueOrFeelsFulAfterEatingInfo
		{
			get => _FatigueOrFeelsFulAfterEatingInfo;
			set
			{
				if (value != _FatigueOrFeelsFulAfterEatingInfo)
				{
					_FatigueOrFeelsFulAfterEatingInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _FatigueOrFeelsFulAfterEatingValue;
		public bool? FatigueOrFeelsFulAfterEatingValue
		{
			get => _FatigueOrFeelsFulAfterEatingValue;
			set
			{
				if (value != _FatigueOrFeelsFulAfterEatingValue)
				{
					_FatigueOrFeelsFulAfterEatingValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _DesireForSweetsAfterEatingInfo;
		public string DesireForSweetsAfterEatingInfo
		{
			get => _DesireForSweetsAfterEatingInfo;
			set
			{
				if (value != _DesireForSweetsAfterEatingInfo)
				{
					_DesireForSweetsAfterEatingInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _DesireForSweetsAfterEatingValue;
		public bool? DesireForSweetsAfterEatingValue
		{
			get => _DesireForSweetsAfterEatingValue;
			set
			{
				if (value != _DesireForSweetsAfterEatingValue)
				{
					_DesireForSweetsAfterEatingValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _DifficultyConcentatingInfo;
		public string DifficultyConcentatingInfo
		{
			get => _DifficultyConcentatingInfo;
			set
			{
				if (value != _DifficultyConcentatingInfo)
				{
					_DifficultyConcentatingInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _DifficultyConcentatingValue;
		public bool? DifficultyConcentatingValue
		{
			get => _DifficultyConcentatingValue;
			set
			{
				if (value != _DifficultyConcentatingValue)
				{
					_DifficultyConcentatingValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _OftenIllInfo;
		public string OftenIllInfo
		{
			get => _OftenIllInfo;
			set
			{
				if (value != _OftenIllInfo)
				{
					_OftenIllInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _OftenIllValue;
		public bool? OftenIllValue
		{
			get => _OftenIllValue;
			set
			{
				if (value != _OftenIllValue)
				{
					_OftenIllValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _SufferingFromMucusInfo;
		public string SufferingFromMucusInfo
		{
			get => _SufferingFromMucusInfo;
			set
			{
				if (value != _SufferingFromMucusInfo)
				{
					_SufferingFromMucusInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _SufferingFromMucusValue;
		public bool? SufferingFromMucusValue
		{
			get => _SufferingFromMucusValue;
			set
			{
				if (value != _SufferingFromMucusValue)
				{
					_SufferingFromMucusValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _CoughOrAllergySuffersInfo;
		public string CoughOrAllergySuffersInfo
		{
			get => _CoughOrAllergySuffersInfo;
			set
			{
				if (value != _CoughOrAllergySuffersInfo)
				{
					_CoughOrAllergySuffersInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _CoughOrAllergySuffersValue;
		public bool? CoughOrAllergySuffersValue
		{
			get => _CoughOrAllergySuffersValue;
			set
			{
				if (value != _CoughOrAllergySuffersValue)
				{
					_CoughOrAllergySuffersValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _SmokingInfo;
		public string SmokingInfo
		{
			get => _SmokingInfo;
			set
			{
				if (value != _SmokingInfo)
				{
					_SmokingInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _SmokingValue;
		public bool? SmokingValue
		{
			get => _SmokingValue;
			set
			{
				if (value != _SmokingValue)
				{
					_SmokingValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _FrequentOrUrgentUrinationInfo;
		public string FrequentOrUrgentUrinationInfo
		{
			get => _FrequentOrUrgentUrinationInfo;
			set
			{
				if (value != _FrequentOrUrgentUrinationInfo)
				{
					_FrequentOrUrgentUrinationInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _FrequentOrUrgentUrinationValue;
		public bool? FrequentOrUrgentUrinationValue
		{
			get => _FrequentOrUrgentUrinationValue;
			set
			{
				if (value != _FrequentOrUrgentUrinationValue)
				{
					_FrequentOrUrgentUrinationValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _PreferColdOrHotInfo;
		public string PreferColdOrHotInfo
		{
			get => _PreferColdOrHotInfo;
			set
			{
				if (value != _PreferColdOrHotInfo)
				{
					_PreferColdOrHotInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private PreferColdOrHotType? _PreferColdOrHotValue;
		public PreferColdOrHotType? PreferColdOrHotValue
		{
			get => _PreferColdOrHotValue;
			set
			{
				if (value != _PreferColdOrHotValue)
				{
					_PreferColdOrHotValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _SuffersFromColdOrHotInfo;
		public string SuffersFromColdOrHotInfo
		{
			get => _SuffersFromColdOrHotInfo;
			set
			{
				if (value != _SuffersFromColdOrHotInfo)
				{
					_SuffersFromColdOrHotInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _SuffersFromColdOrHotValue;
		public bool? SuffersFromColdOrHotValue
		{
			get => _SuffersFromColdOrHotValue;
			set
			{
				if (value != _SuffersFromColdOrHotValue)
				{
					_SuffersFromColdOrHotValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _SatisfiedDientsInfo;
		public string SatisfiedDientsInfo
		{
			get => _SatisfiedDientsInfo;
			set
			{
				if (value != _SatisfiedDientsInfo)
				{
					_SatisfiedDientsInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _SatisfiedDientsValue;
		public bool? SatisfiedDientsValue
		{
			get => _SatisfiedDientsValue;
			set
			{
				if (value != _SatisfiedDientsValue)
				{
					_SatisfiedDientsValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _WantToLostWeightInfo;
		public string WantToLostWeightInfo
		{
			get => _WantToLostWeightInfo;
			set
			{
				if (value != _WantToLostWeightInfo)
				{
					_WantToLostWeightInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _WantToLostWeightValue;
		public bool? WantToLostWeightValue
		{
			get => _WantToLostWeightValue;
			set
			{
				if (value != _WantToLostWeightValue)
				{
					_WantToLostWeightValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _UsingContraceptionInfo;
		public string UsingContraceptionInfo
		{
			get => _UsingContraceptionInfo;
			set
			{
				if (value != _UsingContraceptionInfo)
				{
					_UsingContraceptionInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _UsingContraceptionValue;
		public bool? UsingContraceptionValue
		{
			get => _UsingContraceptionValue;
			set
			{
				if (value != _UsingContraceptionValue)
				{
					_UsingContraceptionValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _CycleRegularInfo;
		public string CycleRegularInfo
		{
			get => _CycleRegularInfo;
			set
			{
				if (value != _CycleRegularInfo)
				{
					_CycleRegularInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _CycleRegularValue;
		public bool? CycleRegularValue
		{
			get => _CycleRegularValue;
			set
			{
				if (value != _CycleRegularValue)
				{
					_CycleRegularValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _SufferingFromCrampsOrNervousBeforeMenstruationInfo;
		public string SufferingFromCrampsOrNervousBeforeMenstruationInfo
		{
			get => _SufferingFromCrampsOrNervousBeforeMenstruationInfo;
			set
			{
				if (value != _SufferingFromCrampsOrNervousBeforeMenstruationInfo)
				{
					_SufferingFromCrampsOrNervousBeforeMenstruationInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _SufferingFromCrampsOrNervousBeforeMenstruationValue;
		public bool? SufferingFromCrampsOrNervousBeforeMenstruationValue
		{
			get => _SufferingFromCrampsOrNervousBeforeMenstruationValue;
			set
			{
				if (value != _SufferingFromCrampsOrNervousBeforeMenstruationValue)
				{
					_SufferingFromCrampsOrNervousBeforeMenstruationValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _SufferingFromMenpauseInfo;
		public string SufferingFromMenpauseInfo
		{
			get => _SufferingFromMenpauseInfo;
			set
			{
				if (value != _SufferingFromMenpauseInfo)
				{
					_SufferingFromMenpauseInfo = value;
					PropertyChangedEvent();
				}
			}
		}
		private bool? _SufferingFromMenpauseValue;
		public bool? SufferingFromMenpauseValue
		{
			get => _SufferingFromMenpauseValue;
			set
			{
				if (value != _SufferingFromMenpauseValue)
				{
					_SufferingFromMenpauseValue = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
		public string HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife
		{
			get => _HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
			set
			{
				if (value != _HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife)
				{
					_HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife = value;
					PropertyChangedEvent();
				}
			}
		}

		private string _WhatChangesDoYouExpectToSeeFromTreatment;
		public string WhatChangesDoYouExpectToSeeFromTreatment
		{
			get => _WhatChangesDoYouExpectToSeeFromTreatment;
			set
			{
				if (value != _WhatChangesDoYouExpectToSeeFromTreatment)
				{
					_WhatChangesDoYouExpectToSeeFromTreatment = value;
					PropertyChangedEvent();
				}
			}
		}

		[ForeignKey(typeof(Patient))]
		public int PatientId { get; set; }

		[ManyToOne]
		public Patient Patient { get; set; }

		[OneToMany]
		public List<Meeting> Meetings { get; set; }

		private DateTime? _CreationDate;
		public DateTime? CreationDate
		{
			get => _CreationDate;
			set
			{
				if (value != _CreationDate)
				{
					_CreationDate = value;
					PropertyChangedEvent();
				}
			}
		}

		[Ignore]
		public string CreationDateString
		{
			get => CreationDate?.ToShortDateString();
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		private void PropertyChangedEvent([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}