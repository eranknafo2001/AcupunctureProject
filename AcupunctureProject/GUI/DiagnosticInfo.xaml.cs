using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AcupunctureProject.Database;

namespace AcupunctureProject.GUI
{
	/// <summary>
	/// Interaction logic for DiagnosticInfo.xaml
	/// </summary>
	public partial class DiagnosticInfo : Window
	{
		public Diagnostic Diagnostic { get; set; }
		private DiagnosticInfo()
		{
			InitializeComponent();
		}
		public DiagnosticInfo(Patient patient) : this()
		{
			Diagnostic = new Diagnostic() { Patient = patient };
			Women.Visibility = patient == null ? Visibility.Visible : patient.Gend == Gender.FEMALE ? Visibility.Visible : Visibility.Collapsed;
			CreationDate.SelectedDate = DateTime.Now;
			SetYasNo(PainYas, PainNo, Diagnostic.Pain);
			SetYasNo(PainPreviousEvaluationsYas, PainPreviousEvaluationsNo, Diagnostic.PainPreviousEvaluations);
			SetYasNo(ScansYas, ScansNo, Diagnostic.Scans);
			SetYasNo(UnderStressYas, UnderStressNo, Diagnostic.UnderStress);
			SetYasNo(TenseMusclesYas, TenseMusclesNo, Diagnostic.TenseMuscles);
			SetYasNo(HighBloodPressureOrColesterolYas, HighBloodPressureOrColesterolNo, Diagnostic.HighBloodPressureOrColesterol);
			SetYasNo(GoodSleepYas, GoodSleepNo, Diagnostic.GoodSleep);
			SetYasNo(FallenToSleepProblemYas, FallenToSleepProblemNo, Diagnostic.FallenToSleepProblem);
			SetYasNo(PalpitationsYas, PalpitationsNo, Diagnostic.Palpitations);
			SetYasNo(FatigueOrFeelsFulAfterEatingYas, FatigueOrFeelsFulAfterEatingNo, Diagnostic.FatigueOrFeelsFulAfterEating);
			SetYasNo(DesireForSweetsAfterEatingYas, DesireForSweetsAfterEatingNo, Diagnostic.DesireForSweetsAfterEating);
			SetYasNo(DifficultyConcentatingYas, DifficultyConcentatingNo, Diagnostic.DifficultyConcentating);
			SetYasNo(OftenIllYas, OftenIllNo, Diagnostic.OftenIll);
			SetYasNo(SufferingFromMucusYas, SufferingFromMucusNo, Diagnostic.SufferingFromMucus);
			SetYasNo(CoughOrAllergySuffersYas, CoughOrAllergySuffersNo, Diagnostic.CoughOrAllergySuffers);
			SetYasNo(SmokingYas, SmokingNo, Diagnostic.Smoking);
			SetYasNo(FrequentOrUrgentUrinationYas, FrequentOrUrgentUrinationNo, Diagnostic.FrequentOrUrgentUrination);
			if (Diagnostic.PreferColdOrHot != null)
			{
				switch (Diagnostic.PreferColdOrHot.Value)
				{
					case PreferColdOrHotType.COLD:
						Cold.IsChecked = true;
						Hot.IsChecked = false;
						break;
					case PreferColdOrHotType.HOT:
						Hot.IsChecked = true;
						Cold.IsChecked = false;
						break;
					case PreferColdOrHotType.NIETHER:
					default:
						Cold.IsChecked = false;
						Hot.IsChecked = false;
						break;
				}
			}
			SetYasNo(SuffersFromColdOrHotYas, SuffersFromColdOrHotNo, Diagnostic.SuffersFromColdOrHot);
			SetYasNo(SatisfiedDientsYas, SatisfiedDientsNo, Diagnostic.SatisfiedDients);
			SetYasNo(WantToLostWeightYas, WantToLostWeightNo, Diagnostic.WantToLostWeight);
			SetYasNo(UsingContraceptionYas, UsingContraceptionNo, Diagnostic.UsingContraception);
			SetYasNo(CycleRegularYas, CycleRegularNo, Diagnostic.CycleRegular);
			SetYasNo(SufferingFromCrampsOrNervousBeforeMenstruationYas, SufferingFromCrampsOrNervousBeforeMenstruationNo, Diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation);
			SetYasNo(SufferingFromMenpauseYas, SufferingFromMenpauseNo, Diagnostic.SufferingFromMenpause);

		}
		public DiagnosticInfo(Diagnostic diagnostic) : this()
		{
			DatabaseConnection.Instance.GetChildren(diagnostic);
			Diagnostic = diagnostic;
			Patient patient = diagnostic.Patient;
			Women.Visibility = patient == null || patient.Gend == Gender.FEMALE ? Visibility.Visible :
																				  Visibility.Collapsed;
			CreationDate.SelectedDate = diagnostic.CreationDate;
			SetYasNo(PainYas, PainNo, Diagnostic.Pain);
			SetYasNo(PainPreviousEvaluationsYas, PainPreviousEvaluationsNo, Diagnostic.PainPreviousEvaluations);
			SetYasNo(ScansYas, ScansNo, Diagnostic.Scans);
			SetYasNo(UnderStressYas, UnderStressNo, Diagnostic.UnderStress);
			SetYasNo(TenseMusclesYas, TenseMusclesNo, Diagnostic.TenseMuscles);
			SetYasNo(HighBloodPressureOrColesterolYas, HighBloodPressureOrColesterolNo, Diagnostic.HighBloodPressureOrColesterol);
			SetYasNo(GoodSleepYas, GoodSleepNo, Diagnostic.GoodSleep);
			SetYasNo(FallenToSleepProblemYas, FallenToSleepProblemNo, Diagnostic.FallenToSleepProblem);
			SetYasNo(PalpitationsYas, PalpitationsNo, Diagnostic.Palpitations);
			SetYasNo(FatigueOrFeelsFulAfterEatingYas, FatigueOrFeelsFulAfterEatingNo, Diagnostic.FatigueOrFeelsFulAfterEating);
			SetYasNo(DesireForSweetsAfterEatingYas, DesireForSweetsAfterEatingNo, Diagnostic.DesireForSweetsAfterEating);
			SetYasNo(DifficultyConcentatingYas, DifficultyConcentatingNo, Diagnostic.DifficultyConcentating);
			SetYasNo(OftenIllYas, OftenIllNo, Diagnostic.OftenIll);
			SetYasNo(SufferingFromMucusYas, SufferingFromMucusNo, Diagnostic.SufferingFromMucus);
			SetYasNo(CoughOrAllergySuffersYas, CoughOrAllergySuffersNo, Diagnostic.CoughOrAllergySuffers);
			SetYasNo(SmokingYas, SmokingNo, Diagnostic.Smoking);
			SetYasNo(FrequentOrUrgentUrinationYas, FrequentOrUrgentUrinationNo, Diagnostic.FrequentOrUrgentUrination);
			switch (Diagnostic.PreferColdOrHot.Value)
			{
				case PreferColdOrHotType.COLD:
					Cold.IsChecked = true;
					Hot.IsChecked = false;
					break;
				case PreferColdOrHotType.HOT:
					Hot.IsChecked = true;
					Cold.IsChecked = false;
					break;
				case PreferColdOrHotType.NIETHER:
				default:
					Cold.IsChecked = false;
					Hot.IsChecked = false;
					break;
			}
			SetYasNo(SuffersFromColdOrHotYas, SuffersFromColdOrHotNo, Diagnostic.SuffersFromColdOrHot);
			SetYasNo(SatisfiedDientsYas, SatisfiedDientsNo, Diagnostic.SatisfiedDients);
			SetYasNo(WantToLostWeightYas, WantToLostWeightNo, Diagnostic.WantToLostWeight);
			SetYasNo(UsingContraceptionYas, UsingContraceptionNo, Diagnostic.UsingContraception);
			SetYasNo(CycleRegularYas, CycleRegularNo, Diagnostic.CycleRegular);
			SetYasNo(SufferingFromCrampsOrNervousBeforeMenstruationYas, SufferingFromCrampsOrNervousBeforeMenstruationNo, Diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation);
			SetYasNo(SufferingFromMenpauseYas, SufferingFromMenpauseNo, Diagnostic.SufferingFromMenpause);
			Profession.Text = Diagnostic.Profession;
			MainComplaint.Text = Diagnostic.MainComplaint;
			SeconderyComlaint.Text = Diagnostic.SeconderyComlaint;
			DrugsUsed.Text = Diagnostic.DrugsUsed;
			TestsMade.Text = Diagnostic.TestsMade;
			Pain.Text = Diagnostic.Pain?.Info;
			PainPreviousEvaluations.Text = Diagnostic.PainPreviousEvaluations?.Info;
			Scans.Text = Diagnostic.Scans?.Info;
			UnderStress.Text = Diagnostic.UnderStress?.Info;
			TenseMuscles.Text = Diagnostic.TenseMuscles?.Info;
			HighBloodPressureOrColesterol.Text = Diagnostic.HighBloodPressureOrColesterol?.Info;
			GoodSleep.Text = Diagnostic.GoodSleep?.Info;
			FallenToSleepProblem.Text = Diagnostic.FallenToSleepProblem?.Info;
			Palpitations.Text = Diagnostic.Palpitations?.Info;
			DefeationRegularity.Text = Diagnostic.DefeationRegularity;
			FatigueOrFeelsFulAfterEating.Text = Diagnostic.FatigueOrFeelsFulAfterEating?.Info;
			DesireForSweetsAfterEating.Text = Diagnostic.DesireForSweetsAfterEating?.Info;
			DifficultyConcentating.Text = Diagnostic.DifficultyConcentating?.Info;
			OftenIll.Text = Diagnostic.OftenIll?.Info;
			SufferingFromMucus.Text = Diagnostic.SufferingFromMucus?.Info;
			CoughOrAllergySuffers.Text = Diagnostic.CoughOrAllergySuffers?.Info;
			Smoking.Text = Diagnostic.Smoking?.Info;
			FrequentOrUrgentUrination.Text = Diagnostic.FrequentOrUrgentUrination?.Info;
			PreferColdOrHot.Text = Diagnostic.PreferColdOrHot?.Info;
			SuffersFromColdOrHot.Text = Diagnostic.SuffersFromColdOrHot?.Info;
			SatisfiedDients.Text = Diagnostic.SatisfiedDients?.Info;
			WantToLostWeight.Text = Diagnostic.WantToLostWeight?.Info;
			UsingContraception.Text = Diagnostic.UsingContraception?.Info;
			CycleRegular.Text = Diagnostic.CycleRegular?.Info;
			SufferingFromCrampsOrNervousBeforeMenstruation.Text = Diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation?.Info;
			SufferingFromMenpause.Text = Diagnostic.SufferingFromMenpause?.Info;
			HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife.Text = Diagnostic.HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
			WhatChangesDoYouExpectToSeeFromTreatment.Text = Diagnostic.WhatChangesDoYouExpectToSeeFromTreatment;
		}

		private void Enter(object sender, KeyEventArgs e)
		{
			if (sender.GetType() != typeof(TextBox))
				return;
			if (e.Key == Key.Enter)
			{
				TextBox textBox = (TextBox)sender;
				int temp = textBox.SelectionStart;
				textBox.Text = textBox.Text.Remove(temp, textBox.SelectionLength);
				textBox.Text = textBox.Text.Insert(temp, "\n");
				textBox.SelectionLength = 0;
				textBox.SelectionStart = temp + 1;
			}
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

		private void SetYasNo(RadioButton yas, RadioButton no, ValueInfo<bool?> Value)
		{
			if (Value?.Value == null)
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
			Diagnostic.Pain = new ValueInfo<bool?>(GetYasNo(PainYas, PainNo), Pain.Text);
			Diagnostic.PainPreviousEvaluations = new ValueInfo<bool?>(GetYasNo(PainPreviousEvaluationsYas, PainPreviousEvaluationsNo), PainPreviousEvaluations.Text);
			Diagnostic.Scans = new ValueInfo<bool?>(GetYasNo(ScansYas, ScansNo), Scans.Text);
			Diagnostic.UnderStress = new ValueInfo<bool?>(GetYasNo(UnderStressYas, UnderStressNo), UnderStress.Text);
			Diagnostic.TenseMuscles = new ValueInfo<bool?>(GetYasNo(TenseMusclesYas, TenseMusclesNo), TenseMuscles.Text);
			Diagnostic.HighBloodPressureOrColesterol = new ValueInfo<bool?>(GetYasNo(HighBloodPressureOrColesterolYas, HighBloodPressureOrColesterolNo), HighBloodPressureOrColesterol.Text);
			Diagnostic.GoodSleep = new ValueInfo<bool?>(GetYasNo(GoodSleepYas, GoodSleepNo), GoodSleep.Text);
			Diagnostic.FallenToSleepProblem = new ValueInfo<bool?>(GetYasNo(FallenToSleepProblemYas, FallenToSleepProblemNo), FallenToSleepProblem.Text);
			Diagnostic.Palpitations = new ValueInfo<bool?>(GetYasNo(PalpitationsYas, PalpitationsNo), Palpitations.Text);
			Diagnostic.FatigueOrFeelsFulAfterEating = new ValueInfo<bool?>(GetYasNo(FatigueOrFeelsFulAfterEatingYas, FatigueOrFeelsFulAfterEatingNo), FatigueOrFeelsFulAfterEating.Text);
			Diagnostic.DesireForSweetsAfterEating = new ValueInfo<bool?>(GetYasNo(DesireForSweetsAfterEatingYas, DesireForSweetsAfterEatingNo), DesireForSweetsAfterEating.Text);
			Diagnostic.DifficultyConcentating = new ValueInfo<bool?>(GetYasNo(DifficultyConcentatingYas, DifficultyConcentatingNo), DifficultyConcentating.Text);
			Diagnostic.OftenIll = new ValueInfo<bool?>(GetYasNo(OftenIllYas, OftenIllNo), OftenIll.Text);
			Diagnostic.SufferingFromMucus = new ValueInfo<bool?>(GetYasNo(SufferingFromMucusYas, SufferingFromMucusNo), SufferingFromMucus.Text);
			Diagnostic.CoughOrAllergySuffers = new ValueInfo<bool?>(GetYasNo(CoughOrAllergySuffersYas, CoughOrAllergySuffersNo), CoughOrAllergySuffers.Text);
			Diagnostic.Smoking = new ValueInfo<bool?>(GetYasNo(SmokingYas, SmokingNo), Smoking.Text);
			Diagnostic.FrequentOrUrgentUrination = new ValueInfo<bool?>(GetYasNo(FrequentOrUrgentUrinationYas, FrequentOrUrgentUrinationNo), FrequentOrUrgentUrination.Text);
			Diagnostic.SuffersFromColdOrHot = new ValueInfo<bool?>(GetYasNo(SuffersFromColdOrHotYas, SuffersFromColdOrHotNo), SuffersFromColdOrHot.Text);
			Diagnostic.SatisfiedDients = new ValueInfo<bool?>(GetYasNo(SatisfiedDientsYas, SatisfiedDientsNo), SatisfiedDients.Text);
			Diagnostic.WantToLostWeight = new ValueInfo<bool?>(GetYasNo(WantToLostWeightYas, WantToLostWeightNo), WantToLostWeight.Text);
			Diagnostic.UsingContraception = new ValueInfo<bool?>(GetYasNo(UsingContraceptionYas, UsingContraceptionNo), UsingContraception.Text);
			Diagnostic.CycleRegular = new ValueInfo<bool?>(GetYasNo(CycleRegularYas, CycleRegularNo), CycleRegular.Text);
			Diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation = new ValueInfo<bool?>(GetYasNo(SufferingFromCrampsOrNervousBeforeMenstruationYas, SufferingFromCrampsOrNervousBeforeMenstruationNo), SufferingFromCrampsOrNervousBeforeMenstruation.Text);
			Diagnostic.SufferingFromMenpause = new ValueInfo<bool?>(GetYasNo(SufferingFromMenpauseYas, SufferingFromMenpauseNo), SufferingFromMenpause.Text);
			bool? PreferColdOrHotBool = GetYasNo(Cold, Hot);
			Diagnostic.PreferColdOrHot = new ValueInfo<PreferColdOrHotType>(PreferColdOrHotBool == null ? 
																			PreferColdOrHotType.NIETHER : 
																			PreferColdOrHotBool == true ? 
																			PreferColdOrHotType.COLD : 
																			PreferColdOrHotType.HOT, 
																			PreferColdOrHot.Text);
			Diagnostic.Profession = Profession.Text;
			Diagnostic.MainComplaint = MainComplaint.Text;
			Diagnostic.SeconderyComlaint = SeconderyComlaint.Text;
			Diagnostic.DrugsUsed = DrugsUsed.Text;
			Diagnostic.TestsMade = TestsMade.Text;
			Diagnostic.CreationDate = (DateTime)CreationDate.SelectedDate;
			Diagnostic = DatabaseConnection.Instance.Set(Diagnostic);
		}

		private void Censel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			SaveData();
		}

		private void SaveClose_Click(object sender, RoutedEventArgs e)
		{
			SaveData();
			Close();
		}
	}
}
