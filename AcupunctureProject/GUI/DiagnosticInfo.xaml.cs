using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AcupunctureProject.Database;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AcupunctureProject.GUI
{

	/// <summary>
	/// Interaction logic for DiagnosticInfo.xaml
	/// </summary>
	public partial class DiagnosticInfo : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private void PropertyChangedEvent([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		private Diagnostic _Diagnostic;
		public Diagnostic Diagnostic
		{
			get => _Diagnostic;
			set
			{
				if (value != _Diagnostic)
				{
					_Diagnostic = value;
					PropertyChangedEvent();
				}
			}
		}

		private DiagnosticInfo()
		{
			DataContext = this;
			InitializeComponent();
		}
		public DiagnosticInfo(Patient patient) : this(new Diagnostic()
		{
			Patient = patient,
			CreationDate = DateTime.Now,
		}, false)
		{ }

		public DiagnosticInfo(Diagnostic diagnostic, bool exist = true) : this()
		{
			if (exist)
				DatabaseConnection.GetChildren(diagnostic);
			Diagnostic = diagnostic;
			var patient = diagnostic.Patient;
			Women.Visibility = patient?.Gend != Gender.MALE ? Visibility.Visible :
															  Visibility.Collapsed;
			//diagnostic.SetIfNotNull();
			SetYasNo(PainYas, PainNo, Diagnostic.PainValue);
			SetYasNo(PainPreviousEvaluationsYas, PainPreviousEvaluationsNo, Diagnostic.PainPreviousEvaluationsValue);
			SetYasNo(ScansYas, ScansNo, Diagnostic.ScansValue);
			SetYasNo(UnderStressYas, UnderStressNo, Diagnostic.UnderStressValue);
			SetYasNo(TenseMusclesYas, TenseMusclesNo, Diagnostic.TenseMusclesValue);
			SetYasNo(HighBloodPressureOrColesterolYas, HighBloodPressureOrColesterolNo, Diagnostic.HighBloodPressureOrColesterolValue);
			SetYasNo(GoodSleepYas, GoodSleepNo, Diagnostic.GoodSleepValue);
			SetYasNo(FallenToSleepProblemYas, FallenToSleepProblemNo, Diagnostic.FallenToSleepProblemValue);
			SetYasNo(PalpitationsYas, PalpitationsNo, Diagnostic.PalpitationsValue);
			SetYasNo(FatigueOrFeelsFulAfterEatingYas, FatigueOrFeelsFulAfterEatingNo, Diagnostic.FatigueOrFeelsFulAfterEatingValue);
			SetYasNo(DesireForSweetsAfterEatingYas, DesireForSweetsAfterEatingNo, Diagnostic.DesireForSweetsAfterEatingValue);
			SetYasNo(DifficultyConcentatingYas, DifficultyConcentatingNo, Diagnostic.DifficultyConcentatingValue);
			SetYasNo(OftenIllYas, OftenIllNo, Diagnostic.OftenIllValue);
			SetYasNo(SufferingFromMucusYas, SufferingFromMucusNo, Diagnostic.SufferingFromMucusValue);
			SetYasNo(CoughOrAllergySuffersYas, CoughOrAllergySuffersNo, Diagnostic.CoughOrAllergySuffersValue);
			SetYasNo(SmokingYas, SmokingNo, Diagnostic.SmokingValue);
			SetYasNo(FrequentOrUrgentUrinationYas, FrequentOrUrgentUrinationNo, Diagnostic.FrequentOrUrgentUrinationValue);
			switch (Diagnostic.PreferColdOrHotValue)
			{
				case PreferColdOrHotType.COLD:
					Cold.IsChecked = true;
					Hot.IsChecked = false;
					break;
				case PreferColdOrHotType.HOT:
					Hot.IsChecked = true;
					Cold.IsChecked = false;
					break;
				default:
					Cold.IsChecked = false;
					Hot.IsChecked = false;
					break;
			}
			SetYasNo(SuffersFromColdOrHotYas, SuffersFromColdOrHotNo, Diagnostic.SuffersFromColdOrHotValue);
			SetYasNo(SatisfiedDientsYas, SatisfiedDientsNo, Diagnostic.SatisfiedDientsValue);
			SetYasNo(WantToLostWeightYas, WantToLostWeightNo, Diagnostic.WantToLostWeightValue);
			SetYasNo(UsingContraceptionYas, UsingContraceptionNo, Diagnostic.UsingContraceptionValue);
			SetYasNo(CycleRegularYas, CycleRegularNo, Diagnostic.CycleRegularValue);
			SetYasNo(SufferingFromCrampsOrNervousBeforeMenstruationYas, SufferingFromCrampsOrNervousBeforeMenstruationNo, Diagnostic.SufferingFromCrampsOrNervousBeforeMenstruationValue);
			SetYasNo(SufferingFromMenpauseYas, SufferingFromMenpauseNo, Diagnostic.SufferingFromMenpauseValue);
		}

		private bool? GetYasNo(RadioButton yas, RadioButton no)
		{
			if (yas.IsChecked == true)
				return true;
			else if (no.IsChecked == true)
				return false;
			else
				return null;
		}

		private void SetYasNo(RadioButton yas, RadioButton no, bool? Value)
		{
			if (Value == null)
			{
				yas.IsChecked = false;
				no.IsChecked = false;
			}
			else
			{
				yas.IsChecked = Value.Value;
				no.IsChecked = !Value.Value;
			}
		}

		private void SaveData()
		{
			Diagnostic.PainValue = GetYasNo(PainYas, PainNo);
			Diagnostic.PainPreviousEvaluationsValue = GetYasNo(PainPreviousEvaluationsYas, PainPreviousEvaluationsNo);
			Diagnostic.ScansValue = GetYasNo(ScansYas, ScansNo);
			Diagnostic.UnderStressValue = GetYasNo(UnderStressYas, UnderStressNo);
			Diagnostic.TenseMusclesValue = GetYasNo(TenseMusclesYas, TenseMusclesNo);
			Diagnostic.HighBloodPressureOrColesterolValue = GetYasNo(HighBloodPressureOrColesterolYas, HighBloodPressureOrColesterolNo);
			Diagnostic.GoodSleepValue = GetYasNo(GoodSleepYas, GoodSleepNo);
			Diagnostic.FallenToSleepProblemValue = GetYasNo(FallenToSleepProblemYas, FallenToSleepProblemNo);
			Diagnostic.PalpitationsValue = GetYasNo(PalpitationsYas, PalpitationsNo);
			Diagnostic.FatigueOrFeelsFulAfterEatingValue = GetYasNo(FatigueOrFeelsFulAfterEatingYas, FatigueOrFeelsFulAfterEatingNo);
			Diagnostic.DesireForSweetsAfterEatingValue = GetYasNo(DesireForSweetsAfterEatingYas, DesireForSweetsAfterEatingNo);
			Diagnostic.DifficultyConcentatingValue = GetYasNo(DifficultyConcentatingYas, DifficultyConcentatingNo);
			Diagnostic.OftenIllValue = GetYasNo(OftenIllYas, OftenIllNo);
			Diagnostic.SufferingFromMucusValue = GetYasNo(SufferingFromMucusYas, SufferingFromMucusNo);
			Diagnostic.CoughOrAllergySuffersValue = GetYasNo(CoughOrAllergySuffersYas, CoughOrAllergySuffersNo);
			Diagnostic.SmokingValue = GetYasNo(SmokingYas, SmokingNo);
			Diagnostic.FrequentOrUrgentUrinationValue = GetYasNo(FrequentOrUrgentUrinationYas, FrequentOrUrgentUrinationNo);
			Diagnostic.SuffersFromColdOrHotValue = GetYasNo(SuffersFromColdOrHotYas, SuffersFromColdOrHotNo);
			Diagnostic.SatisfiedDientsValue = GetYasNo(SatisfiedDientsYas, SatisfiedDientsNo);
			Diagnostic.WantToLostWeightValue = GetYasNo(WantToLostWeightYas, WantToLostWeightNo);
			Diagnostic.UsingContraceptionValue = GetYasNo(UsingContraceptionYas, UsingContraceptionNo);
			Diagnostic.CycleRegularValue = GetYasNo(CycleRegularYas, CycleRegularNo);
			Diagnostic.SufferingFromCrampsOrNervousBeforeMenstruationValue = GetYasNo(SufferingFromCrampsOrNervousBeforeMenstruationYas, SufferingFromCrampsOrNervousBeforeMenstruationNo);
			Diagnostic.SufferingFromMenpauseValue = GetYasNo(SufferingFromMenpauseYas, SufferingFromMenpauseNo);
			bool? PreferColdOrHotBool = GetYasNo(Cold, Hot);
			Diagnostic.PreferColdOrHotValue = PreferColdOrHotBool == null ?
											   PreferColdOrHotType.NIETHER :
											   PreferColdOrHotBool == true ?
											   PreferColdOrHotType.COLD :
											   PreferColdOrHotType.HOT;
			Diagnostic = DatabaseConnection.SetWithChildren(Diagnostic);
		}

		private void Censel_Click(object sender, RoutedEventArgs e) =>
			Close();

		private void Save_Click(object sender, RoutedEventArgs e) =>
			SaveData();

		private void SaveClose_Click(object sender, RoutedEventArgs e)
		{
			SaveData();
			Close();
		}

	}
}
