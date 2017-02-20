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
using AcupunctureProject.database;

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
            Diagnostic = new Diagnostic() { PatientId = patient.Id };
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

        }
        public DiagnosticInfo(Diagnostic diagnostic) : this()
        {
            Diagnostic = Diagnostic;
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
            if (Value.Value == null)
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
            Diagnostic.Pain.Value = GetYasNo(PainYas, PainNo);
            Diagnostic.PainPreviousEvaluations.Value = GetYasNo(PainPreviousEvaluationsYas, PainPreviousEvaluationsNo);
            Diagnostic.Scans.Value = GetYasNo(ScansYas, ScansNo);
            Diagnostic.UnderStress.Value = GetYasNo(UnderStressYas, UnderStressNo);
            Diagnostic.TenseMuscles.Value = GetYasNo(TenseMusclesYas, TenseMusclesNo);
            Diagnostic.HighBloodPressureOrColesterol.Value = GetYasNo(HighBloodPressureOrColesterolYas, HighBloodPressureOrColesterolNo);
            Diagnostic.GoodSleep.Value = GetYasNo(GoodSleepYas, GoodSleepNo);
            Diagnostic.FallenToSleepProblem.Value = GetYasNo(FallenToSleepProblemYas, FallenToSleepProblemNo);
            Diagnostic.Palpitations.Value = GetYasNo(PalpitationsYas, PalpitationsNo);
            Diagnostic.FatigueOrFeelsFulAfterEating.Value = GetYasNo(FatigueOrFeelsFulAfterEatingYas, FatigueOrFeelsFulAfterEatingNo);
            Diagnostic.DesireForSweetsAfterEating.Value = GetYasNo(DesireForSweetsAfterEatingYas, DesireForSweetsAfterEatingNo);
            Diagnostic.DifficultyConcentating.Value = GetYasNo(DifficultyConcentatingYas, DifficultyConcentatingNo);
            Diagnostic.OftenIll.Value = GetYasNo(OftenIllYas, OftenIllNo);
            Diagnostic.SufferingFromMucus.Value = GetYasNo(SufferingFromMucusYas, SufferingFromMucusNo);
            Diagnostic.CoughOrAllergySuffers.Value = GetYasNo(CoughOrAllergySuffersYas, CoughOrAllergySuffersNo);
            Diagnostic.Smoking.Value = GetYasNo(SmokingYas, SmokingNo);
            Diagnostic.FrequentOrUrgentUrination.Value = GetYasNo(FrequentOrUrgentUrinationYas, FrequentOrUrgentUrinationNo);
            Diagnostic.SuffersFromColdOrHot.Value = GetYasNo(SuffersFromColdOrHotYas, SuffersFromColdOrHotNo);
            Diagnostic.SatisfiedDients.Value = GetYasNo(SatisfiedDientsYas, SatisfiedDientsNo);
            Diagnostic.WantToLostWeight.Value = GetYasNo(WantToLostWeightYas, WantToLostWeightNo);
            Diagnostic.UsingContraception.Value = GetYasNo(UsingContraceptionYas, UsingContraceptionNo);
            Diagnostic.CycleRegular.Value = GetYasNo(CycleRegularYas, CycleRegularNo);
            Diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation.Value = GetYasNo(SufferingFromCrampsOrNervousBeforeMenstruationYas, SufferingFromCrampsOrNervousBeforeMenstruationNo);
            Diagnostic.SufferingFromMenpause.Value = GetYasNo(SufferingFromMenpauseYas, SufferingFromMenpauseNo);
            bool? PreferColdOrHotBool = GetYasNo(Cold, Hot);            Diagnostic.PreferColdOrHot.Value = PreferColdOrHotBool == null ? PreferColdOrHotType.NIETHER :
                                               PreferColdOrHotBool == true ? PreferColdOrHotType.COLD :
                                                                             PreferColdOrHotType.HOT;
            Diagnostic = Database.Instance.SetDiagnostic(Diagnostic);
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
