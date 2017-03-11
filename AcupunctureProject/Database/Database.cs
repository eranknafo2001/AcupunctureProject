using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using AcupunctureProject.Database.Treatment;

namespace AcupunctureProject.Database
{
    public static class Extension
    {
        public static string GetStringL(this SQLiteDataReader reader, string str)
        {
            return reader[str].ToString();
        }

        public static DateTime GetDateTimeL(this SQLiteDataReader reader, string str)
        {
            string o = reader[str].ToString();
            return DateTime.Parse(o);
        }

        public static int GetIntL(this SQLiteDataReader reader, string str)
        {
            return (int)(Int64)reader[str];
        }

        public static bool? GetBoolL(this SQLiteDataReader reader, string str)
        {
            object o = reader[str];
            if (o == null || o.GetType() == typeof(DBNull))
                return null;
            return (Boolean)o;
        }

        public static Color ReadColor(this BinaryReader reader)
        {
            return new Color() { R = reader.ReadByte(), G = reader.ReadByte(), B = reader.ReadByte(), A = 255 };
        }

        public static void Write(this BinaryWriter writer, Color color)
        {
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
        }
    }

    public class DatabaseConnection
    {
        private static readonly string ID = "ID";

        private static DatabaseConnection database = null;
        public static DatabaseConnection Instance
        {
            get
            {
                if (database == null)
                    database = new DatabaseConnection();
                return database;
            }
        }

        private SQLiteConnection Connection;
        #region all SQLiteCommand declirations
        private SQLiteCommand GetAllChannelRelativeToSymptomSt;
        private SQLiteCommand GetAllPointRelativeToSymptomSt;
        private SQLiteCommand GetAllSymptomRelativeToPointSt;
        private SQLiteCommand GetAllMeetingsRelativeToPatientOrderByDateSt;
        private SQLiteCommand GetAllSymptomRelativeToMeetingSt;
        private SQLiteCommand GetAllPointRelativeToMeetingSt;

        private SQLiteCommand GetPatientRelativeToMeetingSt;
        private SQLiteCommand GetPatientRelativeToDiagnosticSt;

        private SQLiteCommand InsertSymptomSt;
        private SQLiteCommand InsertMeetingSt;
        private SQLiteCommand InsertPatientSt;
        private SQLiteCommand InsertPointsSt;
        private SQLiteCommand InsertChannelSt;
        private SQLiteCommand InsertDiagnosticSt;

        private SQLiteCommand UpdateSymptomSt;
        private SQLiteCommand UpdateMeetingSt;
        private SQLiteCommand UpdatePatientSt;
        private SQLiteCommand UpdatePointSt;
        private SQLiteCommand UpdateChannelSt;
        private SQLiteCommand UpdateChannelSymptomRelationSt;
        private SQLiteCommand UpdatePointSymptomRelationSt;
        private SQLiteCommand UpdateDiagnosticSt;

        private SQLiteCommand DeleteSymptomSt;
        private SQLiteCommand DeleteMeetingSt;
        private SQLiteCommand DeletePatientSt;
        private SQLiteCommand DeletePointSt;
        private SQLiteCommand DeleteChannelSt;
        private SQLiteCommand DeleteSymptomPointRelationSt;
        private SQLiteCommand DeleteSymptomMeetingRelationSt;
        private SQLiteCommand DeleteSymptomChannelRelationSt;
        private SQLiteCommand DeleteMeetingPointSt;

        private SQLiteCommand InsertSymptomPointRelationSt;
        private SQLiteCommand InsertSymptomMeetingRelationSt;
        private SQLiteCommand InsertMeetingPointRelationSt;
        private SQLiteCommand InsertSymptomChannelRelationSt;

        private SQLiteCommand GetSymptomSt;
        private SQLiteCommand GetPointByNameSt;
        private SQLiteCommand GetPointByIdSt;
        private SQLiteCommand GetAllDiagnosticByPatientSt;
        private SQLiteCommand GetDiagnosticByMeetingSt;

        private SQLiteCommand GetChannelByIdSt;
        private SQLiteCommand GetTheLastMeetingSt;

        private SQLiteCommand FindSymptomSt;

        private SQLiteCommand FindPatientSt;
        private SQLiteCommand FindPointSt;

        private SQLiteCommand GetAllMeetingsRelativeToSymptomsSt;

        private SQLiteCommand GetAllPointsSt;

        private SQLiteCommand InsertTreatment_TreatmentSt;
        private SQLiteCommand InsertTreatment_OperationSt;
        private SQLiteCommand InsertTreatment_SymptomSt;
        private SQLiteCommand InsertTreatment_NoteSt;

        private SQLiteCommand InsertTreatment_NotePointSt;
        private SQLiteCommand InsertTreatment_NoteOperationSt;
        private SQLiteCommand InsertTreatment_NoteTreatmentRelationSt;

        private SQLiteCommand InsertTreatment_SymptomOperationRelationSt;
        private SQLiteCommand InsertTreatment_SymptomPointRelationSt;

        private SQLiteCommand UpdateTreatment_TreatmentSt;
        private SQLiteCommand UpdateTreatment_OperationSt;
        private SQLiteCommand UpdateTreatment_SymptomSt;
        private SQLiteCommand UpdateTreatment_NoteSt;

        private SQLiteCommand DeleteTreatment_TreatmentSt;
        private SQLiteCommand DeleteTreatment_OperationSt;
        private SQLiteCommand DeleteTreatment_SymptomSt;
        private SQLiteCommand DeleteTreatment_NoteSt;

        private SQLiteCommand DeleteTreatment_NotePointSt;
        private SQLiteCommand DeleteTreatment_NoteOperationSt;
        private SQLiteCommand DeleteTreatment_NoteTreatmentRelationSt;

        private SQLiteCommand DeleteTreatment_SymptomOperationRelationSt;
        private SQLiteCommand DeleteTreatment_SymptomPointRelationSt;

        private SQLiteCommand GetAllTreatmentsSt;
        private SQLiteCommand GetTreatmentByNameSt;
        private SQLiteCommand GetAllOperationsByTreatmentSt;
        private SQLiteCommand GetAllPointByOperationSt;
        private SQLiteCommand GetNoteByPointSt;
        private SQLiteCommand GetNoteByTreatmentSt;
        private SQLiteCommand GetNoteByOperationSt;
        private SQLiteCommand GetPointsByNoteSt;
        private SQLiteCommand GetOperationsByNoteSt;
        private SQLiteCommand GetSymptomByPointSt;
        private SQLiteCommand GetSymptomByOperationSt;
        private SQLiteCommand GetPointsBySymptomSt;

        private Color[] PriorityColors;
        public readonly static int NUM_OF_PRIORITIES = 6;
        private readonly static string SETTING_COLOR_FILE = "setting_color.dat";
        private readonly string Folder;
        #endregion
        private DatabaseConnection()
        {
            string[] tempFolder = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');
            Folder = "";
            for (int i = 0; i < tempFolder.Length - 1; i++)
            {
                Folder += tempFolder[i] + "\\";
            }
            Connection = new SQLiteConnection("Data Source=" + Folder + "database.db;Version=3;");
            Connection.Open();
            #region inserts
            InsertSymptomSt = new SQLiteCommand("insert into SYMPTOM(NAME,COMMENT) values(@name,@comment);", Connection);
            InsertSymptomSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@comment") });

            InsertMeetingSt = new SQLiteCommand("insert into MEETING(PATIENT_ID,PURPOSE,DATE,DESCRIPTION,SUMMERY,RESULT_DESCRIPTION,RESULT_VALUE,DIAGNOSTIC_ID) values(@patientId,@purpose,@date,@description,@summery,@resultDescription,@resultValue,@diagnosticId);", Connection);
            InsertMeetingSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@patientId"), new SQLiteParameter("@purpose"), new SQLiteParameter("@date"), new SQLiteParameter("@description"), new SQLiteParameter("@summery"), new SQLiteParameter("@resultDescription"), new SQLiteParameter("@resultValue"), new SQLiteParameter("@diagnosticId") });

            InsertPatientSt = new SQLiteCommand("insert into PATIENT(NAME,TELEPHONE,CELLPHONE,BIRTHDAY,GENDER,ADDRESS,EMAIL,MEDICAL_DESCRIPTION) values(@name,@telephone,@cellphone,@birthday,@gender,@address,@email,@medicalDescription);", Connection);
            InsertPatientSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@telephone"), new SQLiteParameter("@cellphone"), new SQLiteParameter("@birthday"), new SQLiteParameter("@gender"), new SQLiteParameter("@address"), new SQLiteParameter("@email"), new SQLiteParameter("@medicalDescription") });

            InsertPointsSt = new SQLiteCommand("insert into POINTS(NAME,MIN_NEEDLE_DEPTH,MAX_NEEDLE_DEPTH,POSITION,IMPORTENCE,COMMENT1,COMMENT2,NOTE,IMAGE) values(@name,@minNeedleDepth,@maxNeedleDepth,@position,@importance,@comment1,@comment2,@note,@image);", Connection);
            InsertPointsSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@minNeedleDepth"), new SQLiteParameter("@maxNeedleDepth"), new SQLiteParameter("@position"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment1"), new SQLiteParameter("@comment2"), new SQLiteParameter("@note"), new SQLiteParameter("@image") });

            InsertSymptomPointRelationSt = new SQLiteCommand("insert into SYMPTOM_POINTS values(@symptomId,@pointId,@importance,@comment);", Connection);
            InsertSymptomPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });

            InsertSymptomMeetingRelationSt = new SQLiteCommand("insert into MEETING_SYMPTOM(MEETING_ID,SYMPTOM_ID) values(@meetingId,@symptomId);", Connection);
            InsertSymptomMeetingRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@meetingId"), new SQLiteParameter("@symptomId") });

            InsertMeetingPointRelationSt = new SQLiteCommand("insert into MEETING_POINTS(MEETING_ID,POINT_ID) values(@meetingId,@pointId);", Connection);
            InsertMeetingPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@meetingId"), new SQLiteParameter("@pointId") });

            InsertChannelSt = new SQLiteCommand("insert into CHANNEL values(@channelId,@name,@rt,@mainPoint,@evenPoint,@path,@role,@comments);", Connection);
            InsertChannelSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@channelId"), new SQLiteParameter("@name"), new SQLiteParameter("@rt"), new SQLiteParameter("@mainPoint"), new SQLiteParameter("@evenPoint"), new SQLiteParameter("@path"), new SQLiteParameter("@role"), new SQLiteParameter("@comments") });

            InsertSymptomChannelRelationSt = new SQLiteCommand("insert into SYMPTOM_CHANNEL values(@symptomId,@channelId,@importance,@comment);", Connection);
            InsertSymptomChannelRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@channelId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });

            InsertDiagnosticSt = new SQLiteCommand("insert into DIAGNOSTIC(PROFESSION,MAIN_COMPLAINT,SECONDERY_COMPLAINT,DRUGS_USED,TESTS_MADE,IN_PAIN,PAIN_INFO,IS_PAIN_PREVIOUS_EVALUATIONS,PAIN_PREVIOUS_EVALUATION_INFO,IS_THERE_ANYSORT_OF_SCANS,THE_SCANS_INFO,IS_UNDER_STRESS,STRESS_INFO,IS_TENSE_MUSCLES,TENSE_MUSCLES_INFO,IS_HIGH_BLOOD_PRESSURE_OR_COLESTEROL,HIGH_BLOOD_PRESSURE_OR_COLESTEROL_INFO,IS_GOOD_SLEEP,GOOD_SLEEP_INFO,IS_FALLEN_TO_SLEEP_PROBLEM,FALLEN_TO_SLEEP_PROBLEM_INFO,IS_PALPITATIONS,PALPITATIONS_INFO,DEFECATION_REGULARITY,IS_FATIGUE_OR_FEELS_FULL_AFTER_EATING,FATIGUE_OR_FEELS_FULL_AFTER_EATING_INFO,IS_DESIRE_FOR_SWEETS_AFTER_EATING,DESIRE_FOR_SWEETS_AFTER_EATING_INFO,IS_DIFFICULTY_CONCENTRATING,DIFFICULTY_CONCENTRATING_INFO,IS_OFTEN_ILL,OFTEN_ILL_INFO,IS_SUFFERING_FROM_MUCUS,SUFFERING_FROM_MUCUS_INFO,IS_COUGH_OR_ALLERGY_SUFFERS,COUGH_OR_ALLERGY_SUFFERS_INFO,IS_SMOKING,SMOKING_INFO,IS_FREQUENT_OR_URGENT_URINATION,FREQUENT_OR_URGENT_URINATION_INFO,PREFER_COLD_OR_HOT,PREFER_COLD_OR_HOT_INFO,IS_SUFFERS_FROM_COLD_OR_HOT,SUFFERS_FROM_COLD_OR_HOT_INFO,IS_SATISFIED_DIETS,SATISFIED_DIETS_INFO,IS_WANT_TO_LOST_WEIGHT,WANT_TO_LOST_WEIGHT_INFO,IS_USING_CONTRACEPTION,USING_CONTRACEPTION_INFO,IS_CYCLE_REGULAR,CYCLE_REGULAR_INFO,IS_SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION,SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION_INFO,IS_SUFFERING_FROM_MENOPAUSE,SUFFERING_FROM_MENOPAUSE_INFO,HOW_MANY_HOURS_A_WEEK_ARE_YOU_WILLING_TO_INVEST_TO_IMPROVE_THE_QUALITY_OF_LIFE,WHAT_CHANGES_DO_YOU_EXPECT_TO_SEE_FROM_TREATMENT,PATIENT_ID,CREATION_DATE) values(@Profession, @MainComplaint, @SeconderyComlaint, @DrugsUsed, @TestsMade, @IsPain, @PainInfo , @IsPainPreviousEvaluations, @PainPreviousEvaluationsInfo, @IsScans, @ScansInfo, @IsUnderStress, @UnderStressInfo, @IsTenseMuscles, @TenseMusclesInfo, @IsHighBloodPressureOrColesterol, @HighBloodPressureOrColesterolInfo, @IsGoodSleep, @GoodSleepInfo, @IsFallenToSleepProblem, @FallenToSleepProblemInfo, @IsPalpitations, @PalpitationsInfo, @DefeationRegularity, @IsFatigueOrFeelsFulAfterEating, @FatigueOrFeelsFulAfterEatingInfo, @IsDesireForSweetsAfterEating, @DesireForSweetsAfterEatingInfo, @IsDifficultyConcentating, @DifficultyConcentatingInfo, @IsOftenIll, @OftenIllInfo, @IsSufferingFromMucus, @SufferingFromMucusInfo, @IsCoughOrAllergySuffers, @CoughOrAllergySuffersInfo, @IsSmoking, @SmokingInfo, @IsFrequentOrUrgentUrination, @FrequentOrUrgentUrinationInfo, @PreferColdOrHot, @PreferColdOrHotInfo, @IsSuffersFromColdOrHot, @SuffersFromColdOrHotInfo, @IsSatisfiedDients, @SatisfiedDientsInfo, @IsWantToLostWeight, @WantToLostWeightInfo, @IsUsingContraception, @UsingContraceptionInfo, @IsCycleRegular, @CycleRegularInfo, @IsSufferingFromCrampsOrNervousBeforeMenstruation, @SufferingFromCrampsOrNervousBeforeMenstruationInfo, @IsSufferingFromMenpause, @SufferingFromMenpauseInfo, @HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife, @WhatChangesDoYouExpectToSeeFromTreatment,@patientId,@creationDate);", Connection);
            InsertDiagnosticSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@Profession"), new SQLiteParameter("@MainComplaint"), new SQLiteParameter("@SeconderyComlaint"), new SQLiteParameter("@DrugsUsed"), new SQLiteParameter("@TestsMade"), new SQLiteParameter("@IsPain"), new SQLiteParameter("@PainInfo"), new SQLiteParameter("@IsPainPreviousEvaluations"), new SQLiteParameter("@PainPreviousEvaluationsInfo"), new SQLiteParameter("@IsScans"), new SQLiteParameter("@ScansInfo"), new SQLiteParameter("@IsUnderStress"), new SQLiteParameter("@UnderStressInfo"), new SQLiteParameter("@IsTenseMuscles"), new SQLiteParameter("@TenseMusclesInfo"), new SQLiteParameter("@IsHighBloodPressureOrColesterol"), new SQLiteParameter("@HighBloodPressureOrColesterolInfo"), new SQLiteParameter("@IsGoodSleep"), new SQLiteParameter("@GoodSleepInfo"), new SQLiteParameter("@IsFallenToSleepProblem"), new SQLiteParameter("@FallenToSleepProblemInfo"), new SQLiteParameter("@IsPalpitations"), new SQLiteParameter("@PalpitationsInfo"), new SQLiteParameter("@DefeationRegularity"), new SQLiteParameter("@IsFatigueOrFeelsFulAfterEating"), new SQLiteParameter("@FatigueOrFeelsFulAfterEatingInfo"), new SQLiteParameter("@IsDesireForSweetsAfterEating"), new SQLiteParameter("@DesireForSweetsAfterEatingInfo"), new SQLiteParameter("@IsDifficultyConcentating"), new SQLiteParameter("@DifficultyConcentatingInfo"), new SQLiteParameter("@IsOftenIll"), new SQLiteParameter("@OftenIllInfo"), new SQLiteParameter("@IsSufferingFromMucus"), new SQLiteParameter("@SufferingFromMucusInfo"), new SQLiteParameter("@IsCoughOrAllergySuffers"), new SQLiteParameter("@CoughOrAllergySuffersInfo"), new SQLiteParameter("@IsSmoking"), new SQLiteParameter("@SmokingInfo"), new SQLiteParameter("@IsFrequentOrUrgentUrination"), new SQLiteParameter("@FrequentOrUrgentUrinationInfo"), new SQLiteParameter("@PreferColdOrHot"), new SQLiteParameter("@PreferColdOrHotInfo"), new SQLiteParameter("@IsSuffersFromColdOrHot"), new SQLiteParameter("@SuffersFromColdOrHotInfo"), new SQLiteParameter("@IsSatisfiedDients"), new SQLiteParameter("@SatisfiedDientsInfo"), new SQLiteParameter("@IsWantToLostWeight"), new SQLiteParameter("@WantToLostWeightInfo"), new SQLiteParameter("@IsUsingContraception"), new SQLiteParameter("@UsingContraceptionInfo"), new SQLiteParameter("@IsCycleRegular"), new SQLiteParameter("@CycleRegularInfo"), new SQLiteParameter("@IsSufferingFromCrampsOrNervousBeforeMenstruation"), new SQLiteParameter("@SufferingFromCrampsOrNervousBeforeMenstruationInfo"), new SQLiteParameter("@IsSufferingFromMenpause"), new SQLiteParameter("@SufferingFromMenpauseInfo"), new SQLiteParameter("@HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife"), new SQLiteParameter("@WhatChangesDoYouExpectToSeeFromTreatment"), new SQLiteParameter("@patientId"), new SQLiteParameter("@creationDate") });
            #region treatment
            InsertTreatment_TreatmentSt = new SQLiteCommand("insert into TREATMENT(NAME) values(@name);",Connection);
            InsertTreatment_TreatmentSt.Parameters.Add(new SQLiteParameter("@name"));

            InsertTreatment_OperationSt = new SQLiteCommand("insert into OPERATION_TREATMENT(TREATMENT_ID,TEXT) values(@treatmentId,@text);", Connection);
            InsertTreatment_OperationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@treatmentId"), new SQLiteParameter("@test") });

            InsertTreatment_SymptomSt = new SQLiteCommand("insert into SYMPTOM_TREATMENT(TEXT) values(@text);", Connection);
            InsertTreatment_SymptomSt.Parameters.Add(new SQLiteParameter("@text"));

            InsertTreatment_NoteSt = new SQLiteCommand("insert into NOTE_TREATMENT(TEXT) values(@text);", Connection);
            InsertTreatment_NoteSt.Parameters.Add(new SQLiteParameter("@text"));

            InsertTreatment_NotePointSt = new SQLiteCommand("insert into NOTE_TREATMENT_POINT values(@noteTreatmentId,@pointId);", Connection);
            InsertTreatment_NotePointSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@noteTreatmentId"), new SQLiteParameter("@pointId") });

            InsertTreatment_NoteOperationSt = new SQLiteCommand("insert into NOTE_TREATMENT_OPERATION_TREATMENT values(@noteTreatmentId,@operationTreatmentId);", Connection);
            InsertTreatment_NoteOperationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@noteTreatmentId"), new SQLiteParameter("@operationTreatmentId") });

            InsertTreatment_NoteTreatmentRelationSt = new SQLiteCommand("insert into NOTE_TREATMENT_TREATMENT values(@noteTreatmentId,@treatmentId);", Connection);
            InsertTreatment_NoteTreatmentRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@noteTreatmentId"), new SQLiteParameter("treatmentId") });

            InsertTreatment_SymptomOperationRelationSt = new SQLiteCommand("insert into SYMPTOM_TREATMENT_OPERATION_TREATMENT values(@symptomId,@operationId);", Connection);
            InsertTreatment_SymptomOperationRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@operationId") });

            InsertTreatment_SymptomPointRelationSt = new SQLiteCommand("insert into SYMPTOM_TREATMENT_POINT values(@symptomId,@pointId);", Connection);
            InsertTreatment_SymptomPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId") });
            #endregion
            #endregion
            #region updates
            UpdateSymptomSt = new SQLiteCommand("update SYMPTOM set NAME = @name ,COMMENT = @comment where ID = @symptomId;", Connection);
            UpdateSymptomSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@comment") });

            UpdateMeetingSt = new SQLiteCommand("update MEETING set PATIENT_ID = @patientId ,PURPOSE = @purpose ,DATE = @date ,DESCRIPTION = @description ,SUMMERY = @summery ,RESULT_DESCRIPTION = @resultDecription ,RESULT_VALUE = @resultValue, DIAGNOSTIC_ID = @diagnosticId where ID = @meetingId;", Connection);
            UpdateMeetingSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@patientId"), new SQLiteParameter("@purpose"), new SQLiteParameter("@date"), new SQLiteParameter("@description"), new SQLiteParameter("@summery"), new SQLiteParameter("@resultDecription"), new SQLiteParameter("@resultValue"), new SQLiteParameter("@meetingId"), new SQLiteParameter("@diagnosticId") });

            UpdatePatientSt = new SQLiteCommand("update PATIENT set NAME = @name ,TELEPHONE = @telephone ,CELLPHONE = @cellphone ,BIRTHDAY = @birthday ,GENDER = @gander ,ADDRESS = @address ,EMAIL = @email ,MEDICAL_DESCRIPTION = @medicalDescription where ID = @patientId;", Connection);
            UpdatePatientSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@telephone"), new SQLiteParameter("@cellphone"), new SQLiteParameter("@birthday"), new SQLiteParameter("@gander"), new SQLiteParameter("@address"), new SQLiteParameter("@email"), new SQLiteParameter("@medicalDescription"), new SQLiteParameter("@patientId") });

            UpdatePointSt = new SQLiteCommand("update POINTS set NAME = @name ,MIN_NEEDLE_DEPTH = @minNeedleDepth ,MAX_NEEDLE_DEPTH = @maxNeedleDepth ,POSITION = @position ,IMPORTENCE = @importance ,COMMENT1 = @comment1 ,COMMENT2 = @comment2,NOTE = @note,IMAGE = @image where ID = @pointId;", Connection);
            UpdatePointSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@minNeedleDepth"), new SQLiteParameter("@maxNeedleDepth"), new SQLiteParameter("@position"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment1"), new SQLiteParameter("@comment2"), new SQLiteParameter("@note"), new SQLiteParameter("@image"), new SQLiteParameter("@pointId") });

            UpdateChannelSt = new SQLiteCommand("update CHANNEL set NAME = @name, RT = @rt ,MAIN_POINT = @mainPoint ,EVEN_POINT = @evenPoint ,PATH = @path ,ROLE = @role ,COMMENT = @comment where ID = @channelId;", Connection);
            UpdateChannelSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@channelId"), new SQLiteParameter("@name"), new SQLiteParameter("@rt"), new SQLiteParameter("@mainPoint"), new SQLiteParameter("@evenPoint"), new SQLiteParameter("@path"), new SQLiteParameter("@role"), new SQLiteParameter("@comments") });

            UpdateChannelSymptomRelationSt = new SQLiteCommand("update SYMPTOM_CHANNEL set IMPORTENCE = @importance ,COMMENT = @comment where CHANNEL_ID = @channelId and SYMPTOM_ID = @symptomId;", Connection);
            UpdateChannelSymptomRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@channelId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });

            UpdatePointSymptomRelationSt = new SQLiteCommand("update SYMPTOM_POINTS set IMPORTENCE = @importance ,COMMENT = @comment where POINT_ID = @pointId and SYMPTOM_ID = @symptomId;", Connection);
            UpdatePointSymptomRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });

            UpdateDiagnosticSt = new SQLiteCommand("update DIAGNOSTIC set PROFESSION = @Profession, MAIN_COMPLAINT = @MainComplaint, SECONDERY_COMPLAINT = @SeconderyComlaint, DRUGS_USED = @DrugsUsed, TESTS_MADE = @TestsMade, IN_PAIN = @IsPain, PAIN_INFO = @PainInfo , IS_PAIN_PREVIOUS_EVALUATIONS = @IsPainPreviousEvaluations, PAIN_PREVIOUS_EVALUATION_INFO = @PainPreviousEvaluationsInfo, IS_THERE_ANYSORT_OF_SCANS = @IsScans, THE_SCANS_INFO = @ScansInfo, IS_UNDER_STRESS = @IsUnderStress, STRESS_INFO = @UnderStressInfo, IS_TENSE_MUSCLES = @IsTenseMuscles, TENSE_MUSCLES_INFO = @TenseMusclesInfo, IS_HIGH_BLOOD_PRESSURE_OR_COLESTEROL = @IsHighBloodPressureOrColesterol, HIGH_BLOOD_PRESSURE_OR_COLESTEROL_INFO = @HighBloodPressureOrColesterolInfo, IS_GOOD_SLEEP = @IsGoodSleep, GOOD_SLEEP_INFO = @GoodSleepInfo, IS_FALLEN_TO_SLEEP_PROBLEM = @IsFallenToSleepProblem, FALLEN_TO_SLEEP_PROBLEM_INFO = @FallenToSleepProblemInfo, IS_PALPITATIONS = @IsPalpitations, PALPITATIONS_INFO = @PalpitationsInfo, DEFECATION_REGULARITY = @DefeationRegularity, IS_FATIGUE_OR_FEELS_FULL_AFTER_EATING = @IsFatigueOrFeelsFulAfterEating, FATIGUE_OR_FEELS_FULL_AFTER_EATING_INFO = @FatigueOrFeelsFulAfterEatingInfo, IS_DESIRE_FOR_SWEETS_AFTER_EATING = @IsDesireForSweetsAfterEating, DESIRE_FOR_SWEETS_AFTER_EATING_INFO = @DesireForSweetsAfterEatingInfo, IS_DIFFICULTY_CONCENTRATING = @IsDifficultyConcentating, DIFFICULTY_CONCENTRATING_INFO = @DifficultyConcentatingInfo, IS_OFTEN_ILL = @IsOftenIll, OFTEN_ILL_INFO = @OftenIllInfo, IS_SUFFERING_FROM_MUCUS = @IsSufferingFromMucus, SUFFERING_FROM_MUCUS_INFO = @SufferingFromMucusInfo, IS_COUGH_OR_ALLERGY_SUFFERS = @IsCoughOrAllergySuffers, COUGH_OR_ALLERGY_SUFFERS_INFO = @CoughOrAllergySuffersInfo, IS_SMOKING = @IsSmoking, SMOKING_INFO = @SmokingInfo, IS_FREQUENT_OR_URGENT_URINATION = @IsFrequentOrUrgentUrination, FREQUENT_OR_URGENT_URINATION_INFO = @FrequentOrUrgentUrinationInfo, PREFER_COLD_OR_HOT = @PreferColdOrHot, PREFER_COLD_OR_HOT_INFO = @PreferColdOrHotInfo, IS_SUFFERS_FROM_COLD_OR_HOT = @IsSuffersFromColdOrHot, SUFFERS_FROM_COLD_OR_HOT_INFO = @SuffersFromColdOrHotInfo, IS_SATISFIED_DIETS = @IsSatisfiedDients, SATISFIED_DIETS_INFO = @SatisfiedDientsInfo, IS_WANT_TO_LOST_WEIGHT = @IsWantToLostWeight, WANT_TO_LOST_WEIGHT_INFO = @WantToLostWeightInfo, IS_USING_CONTRACEPTION = @IsUsingContraception, USING_CONTRACEPTION_INFO = @UsingContraceptionInfo, IS_CYCLE_REGULAR = @IsCycleRegular, CYCLE_REGULAR_INFO = @CycleRegularInfo, IS_SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION = @IsSufferingFromCrampsOrNervousBeforeMenstruation, SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION_INFO = @SufferingFromCrampsOrNervousBeforeMenstruationInfo, IS_SUFFERING_FROM_MENOPAUSE = @IsSufferingFromMenpause, SUFFERING_FROM_MENOPAUSE_INFO = @SufferingFromMenpauseInfo, HOW_MANY_HOURS_A_WEEK_ARE_YOU_WILLING_TO_INVEST_TO_IMPROVE_THE_QUALITY_OF_LIFE = @HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife, WHAT_CHANGES_DO_YOU_EXPECT_TO_SEE_FROM_TREATMENT = @WhatChangesDoYouExpectToSeeFromTreatment , PATIENT_ID = @patientId , CREATION_DATE = @creationDate where ID = @Id;", Connection);
            UpdateDiagnosticSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@Profession"), new SQLiteParameter("@MainComplaint"), new SQLiteParameter("@SeconderyComlaint"), new SQLiteParameter("@DrugsUsed"), new SQLiteParameter("@TestsMade"), new SQLiteParameter("@IsPain"), new SQLiteParameter("@PainInfo"), new SQLiteParameter("@IsPainPreviousEvaluations"), new SQLiteParameter("@PainPreviousEvaluationsInfo"), new SQLiteParameter("@IsScans"), new SQLiteParameter("@ScansInfo"), new SQLiteParameter("@IsUnderStress"), new SQLiteParameter("@UnderStressInfo"), new SQLiteParameter("@IsTenseMuscles"), new SQLiteParameter("@TenseMusclesInfo"), new SQLiteParameter("@IsHighBloodPressureOrColesterol"), new SQLiteParameter("@HighBloodPressureOrColesterolInfo"), new SQLiteParameter("@IsGoodSleep"), new SQLiteParameter("@GoodSleepInfo"), new SQLiteParameter("@IsFallenToSleepProblem"), new SQLiteParameter("@FallenToSleepProblemInfo"), new SQLiteParameter("@IsPalpitations"), new SQLiteParameter("@PalpitationsInfo"), new SQLiteParameter("@DefeationRegularity"), new SQLiteParameter("@IsFatigueOrFeelsFulAfterEating"), new SQLiteParameter("@FatigueOrFeelsFulAfterEatingInfo"), new SQLiteParameter("@IsDesireForSweetsAfterEating"), new SQLiteParameter("@DesireForSweetsAfterEatingInfo"), new SQLiteParameter("@IsDifficultyConcentating"), new SQLiteParameter("@DifficultyConcentatingInfo"), new SQLiteParameter("@IsOftenIll"), new SQLiteParameter("@OftenIllInfo"), new SQLiteParameter("@IsSufferingFromMucus"), new SQLiteParameter("@SufferingFromMucusInfo"), new SQLiteParameter("@IsCoughOrAllergySuffers"), new SQLiteParameter("@CoughOrAllergySuffersInfo"), new SQLiteParameter("@IsSmoking"), new SQLiteParameter("@SmokingInfo"), new SQLiteParameter("@IsFrequentOrUrgentUrination"), new SQLiteParameter("@FrequentOrUrgentUrinationInfo"), new SQLiteParameter("@PreferColdOrHot"), new SQLiteParameter("@PreferColdOrHotInfo"), new SQLiteParameter("@IsSuffersFromColdOrHot"), new SQLiteParameter("@SuffersFromColdOrHotInfo"), new SQLiteParameter("@IsSatisfiedDients"), new SQLiteParameter("@SatisfiedDientsInfo"), new SQLiteParameter("@IsWantToLostWeight"), new SQLiteParameter("@WantToLostWeightInfo"), new SQLiteParameter("@IsUsingContraception"), new SQLiteParameter("@UsingContraceptionInfo"), new SQLiteParameter("@IsCycleRegular"), new SQLiteParameter("@CycleRegularInfo"), new SQLiteParameter("@IsSufferingFromCrampsOrNervousBeforeMenstruation"), new SQLiteParameter("@SufferingFromCrampsOrNervousBeforeMenstruationInfo"), new SQLiteParameter("@IsSufferingFromMenpause"), new SQLiteParameter("@SufferingFromMenpauseInfo"), new SQLiteParameter("@HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife"), new SQLiteParameter("@WhatChangesDoYouExpectToSeeFromTreatment"), new SQLiteParameter("@Id"), new SQLiteParameter("@patientId"), new SQLiteParameter("@creationDate") });

            #region treatments
            UpdateTreatment_TreatmentSt = new SQLiteCommand("update TREATMENT set NAME = @name where ID = @id;", Connection);
            UpdateTreatment_TreatmentSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@id") });

            UpdateTreatment_OperationSt = new SQLiteCommand("update OPERATION_TREATMENT set TREATMENT_ID = @treatmentId , TEXT = @text where ID = @id;", Connection);
            UpdateTreatment_OperationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@treatmentId"), new SQLiteParameter("@test"),new SQLiteParameter("@id") });

            UpdateTreatment_SymptomSt = new SQLiteCommand("update SYMPTOM_TREATMENT set TEXT = @text where ID = @id;", Connection);
            UpdateTreatment_SymptomSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@text"),new SQLiteParameter("@id") } );

            UpdateTreatment_NoteSt = new SQLiteCommand("update NOTE_TREATMENT set TEXT = @text where ID = @id;", Connection);
            UpdateTreatment_NoteSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@text") ,new SQLiteParameter("@id")});
            #endregion
            #endregion
            #region deletes
            DeleteSymptomSt = new SQLiteCommand("delete from SYMPTOM where ID = @symptomId;", Connection);
            DeleteSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));

            DeleteMeetingSt = new SQLiteCommand("delete from MEETING where ID = @meetingId;", Connection);
            DeleteMeetingSt.Parameters.Add(new SQLiteParameter("@meetingId"));

            DeletePatientSt = new SQLiteCommand("delete from PATIENT where ID = @patientId;", Connection);
            DeletePatientSt.Parameters.Add(new SQLiteParameter("@patientId"));

            DeletePointSt = new SQLiteCommand("delete from POINTS where ID = @pointId;", Connection);
            DeletePointSt.Parameters.Add(new SQLiteParameter("@pointId"));

            DeleteChannelSt = new SQLiteCommand("delete from CHANNEL where ID = @channelId;", Connection);
            DeleteChannelSt.Parameters.Add(new SQLiteParameter("@channelId"));

            DeleteSymptomPointRelationSt = new SQLiteCommand("delete from SYMPTOM_POINTS where SYMPTOM_ID = @symptomId and POINT_ID = @pointId;", Connection);
            DeleteSymptomPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId") });

            DeleteSymptomMeetingRelationSt = new SQLiteCommand("delete from MEETING_SYMPTOM where SYMPTOM_ID = @symptomId and MEETING_ID = @meetingId;", Connection);
            DeleteSymptomMeetingRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@meetingId") });

            DeleteSymptomChannelRelationSt = new SQLiteCommand("delete from SYMPTOM_CHANNEL where SYMPTOM_ID = @symptomId and CHANNEL_ID = @channelId;", Connection);
            DeleteSymptomChannelRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@channelId") });

            DeleteMeetingPointSt = new SQLiteCommand("delete from MEETING_POINTS where MEETING_ID = @meetingId and POINT_ID = @pointId;", Connection);
            DeleteMeetingPointSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@meetingId"), new SQLiteParameter("@pointId") });
            #region treatments
            DeleteTreatment_TreatmentSt = new SQLiteCommand("delete from TREATMENT where ID = @id;", Connection);
            DeleteTreatment_TreatmentSt.Parameters.Add(new SQLiteParameter("id"));

            DeleteTreatment_OperationSt = new SQLiteCommand("delete from OPERATION_TREATMENT where ID = @id;", Connection);
            DeleteTreatment_OperationSt.Parameters.Add(new SQLiteParameter("@id"));

            DeleteTreatment_SymptomSt = new SQLiteCommand("delete from SYMPTOM_TREATMENT where ID = @id;", Connection);
            DeleteTreatment_SymptomSt.Parameters.Add(new SQLiteParameter("@id"));

            DeleteTreatment_NoteSt = new SQLiteCommand("delete from NOTE_TREATMENT where ID = @id;", Connection);
            DeleteTreatment_NoteSt.Parameters.Add(new SQLiteParameter("@id"));

            DeleteTreatment_NotePointSt = new SQLiteCommand("delete from NOTE_TREATMENT_POINT where NOTE_TREATMENT_ID = @noteTreatmentId and POINT_ID = @pointId;", Connection);
            DeleteTreatment_NotePointSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@noteTreatmentId"), new SQLiteParameter("@pointId") });

            DeleteTreatment_NoteOperationSt = new SQLiteCommand("delete from NOTE_TREATMENT_OPERATION_TREATMENT where NOTE_TREATMENT_ID = @noteTreatmentId and OPERATION_TREATMENT_ID = @operationTreatmentId;", Connection);
            DeleteTreatment_NoteOperationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@noteTreatmentId"), new SQLiteParameter("@operationTreatmentId") });

            DeleteTreatment_NoteTreatmentRelationSt = new SQLiteCommand("delete from NOTE_TREATMENT_TREATMENT NOTE_TREATMENT_ID = @noteTreatmentId and TREATMENT_ID = @treatmentId;", Connection);
            DeleteTreatment_NoteTreatmentRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@noteTreatmentId"), new SQLiteParameter("treatmentId") });

            DeleteTreatment_SymptomOperationRelationSt = new SQLiteCommand("delete from SYMPTOM_TREATMENT_OPERATION_TREATMENT where SYMPTOM_ID = @symptomId and OPERATION_ID = @operationId;", Connection);
            DeleteTreatment_SymptomOperationRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@operationId") });

            DeleteTreatment_SymptomPointRelationSt = new SQLiteCommand("delete from SYMPTOM_TREATMENT_POINT where SYMPTOM_ID = @symptomId and POINT_ID = @pointId;", Connection);
            DeleteTreatment_SymptomPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId") });
            #endregion
            #endregion
            #region finds
            GetSymptomSt = new SQLiteCommand("select * from SYMPTOM where NAME = @name;", Connection);
            GetSymptomSt.Parameters.Add(new SQLiteParameter("@name"));

            GetPointByNameSt = new SQLiteCommand("select * from POINTS where NAME = @name;", Connection);
            GetPointByNameSt.Parameters.Add(new SQLiteParameter("@name"));

            GetPointByIdSt = new SQLiteCommand("select * from POINTS where ID = @id;", Connection);
            GetPointByIdSt.Parameters.Add(new SQLiteParameter("@id"));

            GetChannelByIdSt = new SQLiteCommand("select * from CHANNEL where ID = @id;", Connection);
            GetChannelByIdSt.Parameters.Add(new SQLiteParameter("@id"));

            GetAllMeetingsRelativeToPatientOrderByDateSt = new SQLiteCommand("SELECT * FROM MEETING WHERE PATIENT_ID=@patientId ORDER BY DATE DESC;", Connection);
            GetAllMeetingsRelativeToPatientOrderByDateSt.Parameters.Add(new SQLiteParameter("@patientId"));

            GetAllDiagnosticByPatientSt = new SQLiteCommand("select * from DIAGNOSTIC where PATIENT_ID = @patientId ORDER BY CREATION_DATE DESC;", Connection);
            GetAllDiagnosticByPatientSt.Parameters.Add(new SQLiteParameter("@patientId"));

            GetDiagnosticByMeetingSt = new SQLiteCommand("select * from DIAGNOSTIC where ID = @Id;", Connection);
            GetDiagnosticByMeetingSt.Parameters.Add(new SQLiteParameter("@Id"));

            GetAllPointsSt = new SQLiteCommand("select * from POINTS;", Connection);

            FindSymptomSt = new SQLiteCommand(Connection);

            FindPatientSt = new SQLiteCommand(Connection);

            FindPointSt = new SQLiteCommand(Connection);

            GetTheLastMeetingSt = new SQLiteCommand("select * from MEETING where MEETING.PATIENT_ID = @patientId order by date desc limit 1;", Connection);
            GetTheLastMeetingSt.Parameters.Add(new SQLiteParameter("@patientId"));

            GetAllMeetingsRelativeToSymptomsSt = new SQLiteCommand(Connection);

            GetAllPointRelativeToSymptomSt = new SQLiteCommand("select POINTS.* , SYMPTOM_POINTS.IMPORTENCE , SYMPTOM_POINTS.COMMENT from POINTS INNER JOIN SYMPTOM_POINTS ON POINTS.ID = SYMPTOM_POINTS.POINT_ID and SYMPTOM_POINTS.SYMPTOM_ID = @symptomId order by SYMPTOM_POINTS.IMPORTENCE DESC;", Connection);
            GetAllPointRelativeToSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));

            GetAllSymptomRelativeToPointSt = new SQLiteCommand("select SYMPTOM.* , SYMPTOM_POINTS.IMPORTENCE , SYMPTOM_POINTS.COMMENT from SYMPTOM INNER JOIN SYMPTOM_POINTS ON SYMPTOM.ID = SYMPTOM_POINTS.SYMPTOM_ID where SYMPTOM_POINTS.POINT_ID = @pointId order by SYMPTOM_POINTS.IMPORTENCE DESC;", Connection);
            GetAllSymptomRelativeToPointSt.Parameters.Add(new SQLiteParameter("@pointId"));

            GetAllChannelRelativeToSymptomSt = new SQLiteCommand("SELECT CHANNEL.* , SYMPTOM_CHANNEL.IMPORTENCE , SYMPTOM_CHANNEL.COMMENT from CHANNEL INNER JOIN SYMPTOM_CHANNEL ON CHANNEL.ID = SYMPTOM_CHANNEL.CHANNEL_ID where SYMPTOM_CHANNEL.SYMPTOM_ID = @symptomId order by SYMPTOM_CHANNEL.IMPORTENCE DESC;", Connection);
            GetAllChannelRelativeToSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));

            GetAllSymptomRelativeToMeetingSt = new SQLiteCommand("SELECT SYMPTOM.* FROM MEETING_SYMPTOM INNER JOIN MEETING ON MEETING_SYMPTOM.MEETING_ID = MEETING.ID INNER JOIN SYMPTOM ON SYMPTOM.ID = MEETING_SYMPTOM.SYMPTOM_ID WHERE MEETING.ID = @meetingId;", Connection);
            GetAllSymptomRelativeToMeetingSt.Parameters.Add(new SQLiteParameter("@meetingId"));

            GetAllPointRelativeToMeetingSt = new SQLiteCommand("SELECT POINTS.* FROM MEETING_POINTS INNER JOIN MEETING ON MEETING_POINTS.MEETING_ID = MEETING.ID INNER JOIN POINTS ON MEETING_POINTS.POINT_ID = POINTS.ID WHERE MEETING.ID = @meetingId", Connection);
            GetAllPointRelativeToMeetingSt.Parameters.Add(new SQLiteParameter("@meetingId"));

            GetPatientRelativeToMeetingSt = new SQLiteCommand("SELECT * FROM PATIENT WHERE ID = @patientId", Connection);
            GetPatientRelativeToMeetingSt.Parameters.Add(new SQLiteParameter("@meetingId"));

            GetPatientRelativeToDiagnosticSt = new SQLiteCommand("SELECT * FROM PATIENT WHERE ID=@patientId;", Connection);
            GetPatientRelativeToDiagnosticSt.Parameters.Add(new SQLiteParameter("@patientId"));
            #region treatments
            GetAllTreatmentsSt = new SQLiteCommand("select * from TREATMENT;",Connection);

            GetTreatmentByNameSt = new SQLiteCommand("select * from TREATMENT where NAME like '@name';",Connection);
            GetTreatmentByNameSt.Parameters.Add(new SQLiteParameter("@name"));

            GetAllOperationsByTreatmentSt = new SQLiteCommand("select * OPERATION_TREATMENT where TREATMENT_ID = @treatmentId;", Connection);
            GetAllOperationsByTreatmentSt.Parameters.Add(new SQLiteParameter("@treatmentId"));

            GetAllPointByOperationSt = new SQLiteCommand("select POINTS.* from OPERATION_TREATMENT_POINT INNER JOIN POINTS ON POINT_ID = ID where OPERATION_ID = @operationId;", Connection);
            GetAllPointByOperationSt.Parameters.Add(new SQLiteParameter("@operationId"));

            GetNoteByPointSt = new SQLiteCommand("select NOTE_TREATMENT.* from NOTE_TREATMENT INNER JOIN NOTE_TREATMENT_POINT ON NOTE_TREATMENT_ID = ID where POINT_ID = @pointId;", Connection);
            GetNoteByPointSt.Parameters.Add(new SQLiteParameter("@pointId"));

            GetNoteByTreatmentSt = new SQLiteCommand("select NOTE_TREATMENT.* from NOTE_TREATMENT INNER JOIN NOTE_TREATMENT_TREATMENT ON NOTE_TREATMENT_ID = ID where TREATMENT_ID = @treatmentId;", Connection);
            GetNoteByTreatmentSt.Parameters.Add(new SQLiteParameter("@treatmentId"));

            GetNoteByOperationSt = new SQLiteCommand("select NOTE_TREATMENT.* from NOTE_TREATMENT INNER JOIN OPERATION_TREATMENT ON NOTE_TREATMENT_ID = ID where OPEREATION_ID = @operationId;", Connection);
            GetNoteByOperationSt.Parameters.Add(new SQLiteParameter("@operationId"));

            GetPointsByNoteSt = new SQLiteCommand("select POINTS.* from POINTS INNER JOIN NOTE_TREATMENT_POINT ON ID = POINT_ID where NOTE_TREATMENT_ID = @noteTreatmentId;", Connection);
            GetPointsByNoteSt.Parameters.Add(new SQLiteParameter("@noteTreatmentId"));

            GetOperationsByNoteSt = new SQLiteCommand("select OPERATION_TREATMENT.* from OPERATION_TREATMENT INNER JOIN NOTE_TREATMENT_OPERATION_TREATMENT ON ID = OPERATION_ID where NOTE_TREATMENT_ID = @noteTreatmentId;", Connection);
            GetOperationsByNoteSt.Parameters.Add(new SQLiteParameter("@noteTreatmentId"));

            GetSymptomByPointSt = new SQLiteCommand("select SYMPTOM_TREATMENT.* from SYMPTOM_TREATMENT INNER JOIN SYMPTOM_TREATMENT_POINT ON ID = SYMPTOM_ID where POINT_ID = @pointId;", Connection);
            GetSymptomByPointSt.Parameters.Add(new SQLiteParameter("@pointId"));

            GetSymptomByOperationSt = new SQLiteCommand("select SYMPTOM_TREATMENT.* from SYMPTOM_TREATMENT INNER JOIN SYMPTOM_TREATMENT_OPERATION_TREATMENT ON ID = SYMPTON_ID where OPERATION_ID = @operationId;", Connection);
            GetSymptomByOperationSt.Parameters.Add(new SQLiteParameter("@operationId"));

            GetPointsBySymptomSt = new SQLiteCommand("select POINTS.* from POINTS INNER JOIN SYMPTOM_TREATMENT_POINT ID = POINT_ID where SYMPTOM_ID = @symptomId;", Connection);
            GetPointsBySymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));
            #endregion
            #endregion
            InitColors();
        }

        #region colors handler
        private void InitColors()
        {
            PriorityColors = new Color[NUM_OF_PRIORITIES];
            try
            {
                using (BinaryReader reader = new BinaryReader(new FileStream(Folder + SETTING_COLOR_FILE, FileMode.Open)))
                {
                    for (int i = 0; i < NUM_OF_PRIORITIES; i++)
                        PriorityColors[i] = reader.ReadColor();
                }
            }
            catch (FileNotFoundException)
            {
                PriorityColors[0] = new Color() { A = 255, R = 51, G = 0, B = 0 };
                PriorityColors[1] = new Color() { A = 255, R = 102, G = 102, B = 0 };
                PriorityColors[2] = new Color() { A = 255, R = 0, G = 102, B = 102 };
                PriorityColors[3] = new Color() { A = 255, R = 51, G = 0, B = 102 };
                PriorityColors[4] = new Color() { A = 255, R = 102, G = 0, B = 51 };
                PriorityColors[5] = new Color() { A = 255, R = 0, G = 0, B = 102 };
                using (BinaryWriter writer = new BinaryWriter(new FileStream(Folder + SETTING_COLOR_FILE, FileMode.Create)))
                {
                    for (int i = 0; i < NUM_OF_PRIORITIES; i++)
                    {
                        writer.Write(PriorityColors[i]);
                    }
                }
            }
        }
        public Color GetLevel(int level)
        {
            return PriorityColors[level];
        }

        public void SetLevel(int level, Color color)
        {
            PriorityColors[level] = color;
            using (BinaryWriter writer = new BinaryWriter(new FileStream(Folder + SETTING_COLOR_FILE, FileMode.OpenOrCreate)))
            {
                for (int i = 0; i < NUM_OF_PRIORITIES; i++)
                {
                    writer.Write(PriorityColors[i]);
                }
            }
        }
        #endregion
        #region updates
        public void UpdateSymptom(Symptom symptom)
        {
            UpdateSymptomSt.Parameters["@name"].Value = symptom.Name;
            UpdateSymptomSt.Parameters["@comment"].Value = symptom.Comment;
            UpdateSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            UpdateSymptomSt.ExecuteNonQuery();
        }

        public void UpdateMeeting(Meeting meeting)
        {
            UpdateMeetingSt.Parameters["@patientId"].Value = meeting.PatientId;
            UpdateMeetingSt.Parameters["@purpose"].Value = meeting.Purpose;
            UpdateMeetingSt.Parameters["@date"].Value = meeting.Date;
            UpdateMeetingSt.Parameters["@description"].Value = meeting.Description;
            UpdateMeetingSt.Parameters["@summery"].Value = meeting.Summery;
            UpdateMeetingSt.Parameters["@resultDecription"].Value = meeting.ResultDescription;
            UpdateMeetingSt.Parameters["@resultValue"].Value = meeting.Result.Value;
            UpdateMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            UpdateMeetingSt.Parameters["@diagnosticId"].Value = meeting.DiagnosticId;
            UpdateMeetingSt.ExecuteNonQuery();
        }

        public void UpdatePatient(Patient patient)
        {
            UpdatePatientSt.Parameters["@name"].Value = patient.Name;
            UpdatePatientSt.Parameters["@telephone"].Value = patient.Telephone;
            UpdatePatientSt.Parameters["@cellphone"].Value = patient.Cellphone;
            UpdatePatientSt.Parameters["@birthday"].Value = patient.Birthday;
            UpdatePatientSt.Parameters["@gander"].Value = patient.Gend.Value;
            UpdatePatientSt.Parameters["@address"].Value = patient.Address;
            UpdatePatientSt.Parameters["@email"].Value = patient.Email;
            UpdatePatientSt.Parameters["@medicalDescription"].Value = patient.MedicalDescription;
            UpdatePatientSt.Parameters["@patientId"].Value = patient.Id;
            UpdatePatientSt.ExecuteNonQuery();
        }

        public void UpdatePoint(Point point)
        {
            UpdatePointSt.Parameters["@name"].Value = point.Name;
            UpdatePointSt.Parameters["@minNeedleDepth"].Value = point.MinNeedleDepth;
            UpdatePointSt.Parameters["@maxNeedleDepth"].Value = point.MaxNeedleDepth;
            UpdatePointSt.Parameters["@position"].Value = point.Position;
            UpdatePointSt.Parameters["@importance"].Value = point.Importance;
            UpdatePointSt.Parameters["@comment1"].Value = point.Comment1;
            UpdatePointSt.Parameters["@comment2"].Value = point.Comment2;
            UpdatePointSt.Parameters["@note"].Value = point.Note;
            UpdatePointSt.Parameters["@image"].Value = point.Image;
            UpdatePointSt.Parameters["@pointId"].Value = point.Id;
            UpdatePointSt.ExecuteNonQuery();
        }

        public void UpdateChannel(Channel channel)
        {
            UpdateChannelSt.Parameters["@name"].Value = channel.Name;
            UpdateChannelSt.Parameters["@rt"].Value = channel.Rt;
            UpdateChannelSt.Parameters["@mainPoint"].Value = channel.MainPoint;
            UpdateChannelSt.Parameters["@evenPoint"].Value = channel.EvenPoint;
            UpdateChannelSt.Parameters["@path"].Value = channel.Path;
            UpdateChannelSt.Parameters["@role"].Value = channel.Role;
            UpdateChannelSt.Parameters["@comments"].Value = channel.Comments;
            UpdateChannelSt.Parameters["@channelId"].Value = channel.Id;
            UpdateChannelSt.ExecuteNonQuery();
        }

        public void UpdateChannelSymptomRelation(Channel channel, Symptom symptom, int importance, string comment)
        {
            UpdateChannelSymptomRelationSt.Parameters["@importance"].Value = importance;
            UpdateChannelSymptomRelationSt.Parameters["@comment"].Value = comment;
            UpdateChannelSymptomRelationSt.Parameters["@channelId"].Value = channel.Id;
            UpdateChannelSymptomRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            UpdateChannelSymptomRelationSt.ExecuteNonQuery();
        }

        public void UpdatePointSymptomRelation(Point point, Symptom symptom, int importance, string comment)
        {
            UpdatePointSymptomRelationSt.Parameters["@importance"].Value = importance;
            UpdatePointSymptomRelationSt.Parameters["@comment"].Value = comment;
            UpdatePointSymptomRelationSt.Parameters["@pointId"].Value = point.Id;
            UpdatePointSymptomRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            UpdatePointSymptomRelationSt.ExecuteNonQuery();
        }

        private void UpdateDiagnostic(Diagnostic diagnostic)
        {
            UpdateDiagnosticSt.Parameters["@Id"].Value = diagnostic.Id;
            UpdateDiagnosticSt.Parameters["@Profession"].Value = diagnostic.Profession;
            UpdateDiagnosticSt.Parameters["@MainComplaint"].Value = diagnostic.MainComplaint;
            UpdateDiagnosticSt.Parameters["@SeconderyComlaint"].Value = diagnostic.SeconderyComlaint;
            UpdateDiagnosticSt.Parameters["@DrugsUsed"].Value = diagnostic.DrugsUsed;
            UpdateDiagnosticSt.Parameters["@TestsMade"].Value = diagnostic.TestsMade;
            UpdateDiagnosticSt.Parameters["@IsPain"].Value = diagnostic.Pain.Value;
            UpdateDiagnosticSt.Parameters["@PainInfo"].Value = diagnostic.Pain.Info;
            UpdateDiagnosticSt.Parameters["@IsPainPreviousEvaluations"].Value = diagnostic.PainPreviousEvaluations.Value;
            UpdateDiagnosticSt.Parameters["@PainPreviousEvaluationsInfo"].Value = diagnostic.PainPreviousEvaluations.Info;
            UpdateDiagnosticSt.Parameters["@IsScans"].Value = diagnostic.Scans.Value;
            UpdateDiagnosticSt.Parameters["@ScansInfo"].Value = diagnostic.Scans.Info;
            UpdateDiagnosticSt.Parameters["@IsUnderStress"].Value = diagnostic.UnderStress.Value;
            UpdateDiagnosticSt.Parameters["@UnderStressInfo"].Value = diagnostic.UnderStress.Info;
            UpdateDiagnosticSt.Parameters["@IsTenseMuscles"].Value = diagnostic.TenseMuscles.Value;
            UpdateDiagnosticSt.Parameters["@TenseMusclesInfo"].Value = diagnostic.TenseMuscles.Info;
            UpdateDiagnosticSt.Parameters["@IsHighBloodPressureOrColesterol"].Value = diagnostic.HighBloodPressureOrColesterol.Value;
            UpdateDiagnosticSt.Parameters["@HighBloodPressureOrColesterolInfo"].Value = diagnostic.HighBloodPressureOrColesterol.Info;
            UpdateDiagnosticSt.Parameters["@IsGoodSleep"].Value = diagnostic.GoodSleep.Value;
            UpdateDiagnosticSt.Parameters["@GoodSleepInfo"].Value = diagnostic.GoodSleep.Info;
            UpdateDiagnosticSt.Parameters["@IsFallenToSleepProblem"].Value = diagnostic.FallenToSleepProblem.Value;
            UpdateDiagnosticSt.Parameters["@FallenToSleepProblemInfo"].Value = diagnostic.FallenToSleepProblem.Info;
            UpdateDiagnosticSt.Parameters["@IsPalpitations"].Value = diagnostic.Palpitations.Value;
            UpdateDiagnosticSt.Parameters["@PalpitationsInfo"].Value = diagnostic.Palpitations.Info;
            UpdateDiagnosticSt.Parameters["@DefeationRegularity"].Value = diagnostic.DefeationRegularity;
            UpdateDiagnosticSt.Parameters["@IsFatigueOrFeelsFulAfterEating"].Value = diagnostic.FatigueOrFeelsFulAfterEating.Value;
            UpdateDiagnosticSt.Parameters["@FatigueOrFeelsFulAfterEatingInfo"].Value = diagnostic.FatigueOrFeelsFulAfterEating.Info;
            UpdateDiagnosticSt.Parameters["@IsDesireForSweetsAfterEating"].Value = diagnostic.DesireForSweetsAfterEating.Value;
            UpdateDiagnosticSt.Parameters["@DesireForSweetsAfterEatingInfo"].Value = diagnostic.DesireForSweetsAfterEating.Info;
            UpdateDiagnosticSt.Parameters["@IsDifficultyConcentating"].Value = diagnostic.DifficultyConcentating.Value;
            UpdateDiagnosticSt.Parameters["@DifficultyConcentatingInfo"].Value = diagnostic.DifficultyConcentating.Info;
            UpdateDiagnosticSt.Parameters["@IsOftenIll"].Value = diagnostic.OftenIll.Value;
            UpdateDiagnosticSt.Parameters["@OftenIllInfo"].Value = diagnostic.OftenIll.Info;
            UpdateDiagnosticSt.Parameters["@IsSufferingFromMucus"].Value = diagnostic.SufferingFromMucus.Value;
            UpdateDiagnosticSt.Parameters["@SufferingFromMucusInfo"].Value = diagnostic.SufferingFromMucus.Info;
            UpdateDiagnosticSt.Parameters["@IsCoughOrAllergySuffers"].Value = diagnostic.CoughOrAllergySuffers.Value;
            UpdateDiagnosticSt.Parameters["@CoughOrAllergySuffersInfo"].Value = diagnostic.CoughOrAllergySuffers.Info;
            UpdateDiagnosticSt.Parameters["@IsSmoking"].Value = diagnostic.Smoking.Value;
            UpdateDiagnosticSt.Parameters["@SmokingInfo"].Value = diagnostic.Smoking.Info;
            UpdateDiagnosticSt.Parameters["@IsFrequentOrUrgentUrination"].Value = diagnostic.FrequentOrUrgentUrination.Value;
            UpdateDiagnosticSt.Parameters["@FrequentOrUrgentUrinationInfo"].Value = diagnostic.FrequentOrUrgentUrination.Info;
            UpdateDiagnosticSt.Parameters["@PreferColdOrHot"].Value = diagnostic.PreferColdOrHot;
            UpdateDiagnosticSt.Parameters["@PreferColdOrHotInfo"].Value = diagnostic.PreferColdOrHot.Info;
            UpdateDiagnosticSt.Parameters["@IsSuffersFromColdOrHot"].Value = diagnostic.SuffersFromColdOrHot.Value;
            UpdateDiagnosticSt.Parameters["@SuffersFromColdOrHotInfo"].Value = diagnostic.SuffersFromColdOrHot.Info;
            UpdateDiagnosticSt.Parameters["@IsSatisfiedDients"].Value = diagnostic.SatisfiedDients.Value;
            UpdateDiagnosticSt.Parameters["@SatisfiedDientsInfo"].Value = diagnostic.SatisfiedDients.Info;
            UpdateDiagnosticSt.Parameters["@IsWantToLostWeight"].Value = diagnostic.WantToLostWeight.Value;
            UpdateDiagnosticSt.Parameters["@WantToLostWeightInfo"].Value = diagnostic.WantToLostWeight.Info;
            UpdateDiagnosticSt.Parameters["@IsUsingContraception"].Value = diagnostic.UsingContraception.Value;
            UpdateDiagnosticSt.Parameters["@UsingContraceptionInfo"].Value = diagnostic.UsingContraception.Info;
            UpdateDiagnosticSt.Parameters["@IsCycleRegular"].Value = diagnostic.CycleRegular.Value;
            UpdateDiagnosticSt.Parameters["@CycleRegularInfo"].Value = diagnostic.CycleRegular.Info;
            UpdateDiagnosticSt.Parameters["@IsSufferingFromCrampsOrNervousBeforeMenstruation"].Value = diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation.Value;
            UpdateDiagnosticSt.Parameters["@SufferingFromCrampsOrNervousBeforeMenstruationInfo"].Value = diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation.Info;
            UpdateDiagnosticSt.Parameters["@IsSufferingFromMenpause"].Value = diagnostic.SufferingFromMenpause.Value;
            UpdateDiagnosticSt.Parameters["@SufferingFromMenpauseInfo"].Value = diagnostic.SufferingFromMenpause.Info;
            UpdateDiagnosticSt.Parameters["@HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife"].Value = diagnostic.HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
            UpdateDiagnosticSt.Parameters["@WhatChangesDoYouExpectToSeeFromTreatment"].Value = diagnostic.WhatChangesDoYouExpectToSeeFromTreatment;
            UpdateDiagnosticSt.Parameters["@patientId"].Value = diagnostic.PatientId;
            UpdateDiagnosticSt.Parameters["@creationDate"].Value = diagnostic.CreationDate;
            UpdateDiagnosticSt.ExecuteNonQuery();
        }
        public void UpdateTreatment(Treatment.Treatment treatment)
        {
            UpdateTreatment_TreatmentSt.Parameters["@id"].Value = treatment.Id;
            UpdateTreatment_TreatmentSt.Parameters["@name"].Value = treatment.Name;
            UpdateTreatment_TreatmentSt.ExecuteNonQuery();
        }
        public void UpdateOperation(Treatment.Operation operation)
        {
            UpdateTreatment_OperationSt.Parameters["@id"].Value = operation.Id;
            UpdateTreatment_OperationSt.Parameters["@text"].Value = operation.Text;
            UpdateTreatment_OperationSt.ExecuteNonQuery();
        }
        public void UpdateSymptom(Treatment.Symptom symptom)
        {
            UpdateTreatment_SymptomSt.Parameters["@id"].Value = symptom.Id;
            UpdateTreatment_SymptomSt.Parameters["@text"].Value = symptom.Text;
            UpdateTreatment_SymptomSt.ExecuteNonQuery();
        }
        public void UpdateNote(Treatment.Note note)
        {
            UpdateTreatment_NoteSt.Parameters["@id"].Value = note.Id;
            UpdateTreatment_NoteSt.Parameters["@text"].Value = note.Text;
            UpdateTreatment_NoteSt.ExecuteNonQuery();
        }
        #endregion
        #region deletes
        public void DeleteSymptom(Symptom symptom)
        {
            DeleteSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            DeleteSymptomSt.ExecuteNonQuery();
        }

        public void DeleteMeeting(Meeting meeting)
        {
            DeleteMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            DeleteMeetingSt.ExecuteNonQuery();
        }

        public void DeletePatient(Patient patient)
        {
            DeletePatientSt.Parameters["@patientId"].Value = patient.Id;
            DeletePatientSt.ExecuteNonQuery();
        }

        public void DeletePoint(Point point)
        {
            DeletePointSt.Parameters["@pointId"].Value = point.Id;
            DeletePointSt.ExecuteNonQuery();
        }

        public void DeleteChannel(Channel channel)
        {
            DeleteChannelSt.Parameters["@channelId"].Value = channel.Id;
            DeleteChannelSt.ExecuteNonQuery();
        }

        public void DeleteSymptomPointRelation(Symptom symptom, Point point)
        {
            DeleteSymptomPointRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            DeleteSymptomPointRelationSt.Parameters["@pointId"].Value = point.Id;
            DeleteSymptomPointRelationSt.ExecuteNonQuery();
        }

        public void DeleteSymptomMeetingRelation(Symptom symptom, Meeting meeting)
        {
            DeleteSymptomMeetingRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            DeleteSymptomMeetingRelationSt.Parameters["@meetingId"].Value = meeting.Id;
            DeleteSymptomMeetingRelationSt.ExecuteNonQuery();
        }

        public void DeleteSymptomChannelRelation(Symptom symptom, Channel channel)
        {
            DeleteSymptomChannelRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            DeleteSymptomChannelRelationSt.Parameters["@channelId"].Value = channel.Id;
            DeleteSymptomChannelRelationSt.ExecuteNonQuery();
        }

        public void DeleteMeetingPoint(Meeting meeting, Point point)
        {
            DeleteMeetingPointSt.Parameters["@meetingId"].Value = meeting.Id;
            DeleteMeetingPointSt.Parameters["@pointId"].Value = point.Id;
            DeleteMeetingPointSt.ExecuteNonQuery();
        }
        #region treatments
        public void DeleteTreatment(Treatment.Treatment treatment)
        {
            DeleteTreatment_TreatmentSt.Parameters["@id"].Value = treatment.Id;
            DeleteTreatment_TreatmentSt.ExecuteNonQuery();
        }
        public void DeleteOperation(Treatment.Operation operation)
        {
            DeleteTreatment_OperationSt.Parameters["@id"].Value = operation.Id;
            DeleteTreatment_OperationSt.ExecuteNonQuery();
        }
        public void DeleteSymptom(Treatment.Symptom symptom)
        {
            DeleteTreatment_SymptomSt.Parameters["@id"].Value = symptom.Id;
            DeleteTreatment_SymptomSt.ExecuteNonQuery();
        }
        public void DeleteNote(Treatment.Note note)
        {
            DeleteTreatment_NoteSt.Parameters["@id"].Value = note.Id;
            DeleteTreatment_NoteSt.ExecuteNonQuery();
        }
        public void DeleteNoteTreatmentRelation(Treatment.Note note,Treatment.Treatment treatment)
        {
            DeleteTreatment_NoteTreatmentRelationSt.Parameters["@noteTreatmentId"].Value = note.Id;
            DeleteTreatment_NoteTreatmentRelationSt.Parameters["@treatmentId"].Value = treatment.Id;
            DeleteTreatment_NoteTreatmentRelationSt.ExecuteNonQuery();
        }
        public void DeleteNotePointRelation(Treatment.Note note , Point point)
        {
            DeleteTreatment_NotePointSt.Parameters["@noteTreatmentId"].Value = note.Id;
            DeleteTreatment_NotePointSt.Parameters["@pointId"].Value = point.Id;
            DeleteTreatment_NotePointSt.ExecuteNonQuery();
        }
        public void DeleteNoteOperationRelation(Treatment.Note note,Treatment.Operation operation)
        {
            DeleteTreatment_NoteOperationSt.Parameters["@noteTreatmentId"].Value = note.Id;
            DeleteTreatment_NoteOperationSt.Parameters["@operationId"].Value = operation.Id;
            DeleteTreatment_NoteOperationSt.ExecuteNonQuery();
        }
        public void DeleteSymptomPointRelation(Treatment.Symptom symptom,Point point)
        {
            DeleteTreatment_SymptomPointRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            DeleteTreatment_SymptomPointRelationSt.Parameters["@pointId"].Value = point.Id;
            DeleteTreatment_SymptomPointRelationSt.ExecuteNonQuery();
        }
        public void DelteSymptomOperationRelation(Treatment.Symptom symptom ,Treatment.Operation operation)
        {
            DeleteTreatment_SymptomOperationRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            DeleteTreatment_SymptomOperationRelationSt.Parameters["@operationId"].Value = operation.Id;
            DeleteTreatment_SymptomOperationRelationSt.ExecuteNonQuery();
        }
        #endregion
        #endregion
        #region finds objects
        public List<Diagnostic> GetAllDiagnosticByPatient(Patient patient)
        {
            GetAllDiagnosticByPatientSt.Parameters["@patientId"].Value = patient.Id;
            List<Diagnostic> o = new List<Diagnostic>();
            using (SQLiteDataReader rs = GetAllDiagnosticByPatientSt.ExecuteReader())
            {
                while (rs.Read())
                {
                    o.Add(GetDiagnostic(rs));
                }
            }
            return o;
        }
        public Diagnostic GetDiagnosticByMeeting(Meeting meeting)
        {
            GetDiagnosticByMeetingSt.Parameters["@diagnosticId"].Value = meeting.DiagnosticId;
            using (SQLiteDataReader rs = GetDiagnosticByMeetingSt.ExecuteReader())
            {
                if (rs.Read())
                {
                    return GetDiagnostic(rs);
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Meeting> GetAllMeetingsRelativeToPatientOrderByDate(Patient patient)
        {
            GetAllMeetingsRelativeToPatientOrderByDateSt.Parameters["@patientId"].Value = patient.Id;
            using (SQLiteDataReader rs = GetAllMeetingsRelativeToPatientOrderByDateSt.ExecuteReader())
            {
                return GetMeetings(rs);
            }
        }
        public Channel GetChannel(int id)
        {
            GetChannelByIdSt.Parameters["@id"].Value = id;
            using (SQLiteDataReader rs = GetChannelByIdSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetChannel(rs);
                return null;
            }
        }

        public Point GetPoint(int id)
        {
            GetPointByIdSt.Parameters["@id"].Value = id;
            using (SQLiteDataReader rs = GetPointByIdSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetPoint(rs);
                return null;
            }
        }

        public List<Point> GetAllPoints()
        {
            using (SQLiteDataReader rs = GetAllPointsSt.ExecuteReader())
            {
                return GetPoints(rs);
            }
        }

        public Point GetPoint(string name)
        {
            GetPointByNameSt.Parameters["@name"].Value = name;
            using (SQLiteDataReader rs = GetPointByNameSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetPoint(rs);
                return null;
            }
        }

        public Symptom GetSymptom(string name)
        {
            GetSymptomSt.Parameters["@name"].Value = name;
            using (SQLiteDataReader rs = GetSymptomSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetSymptom(rs);
                return null;
            }
        }

        public List<Symptom> FindSymptom(string name)
        {
            FindSymptomSt.CommandText = "SELECT * FROM SYMPTOM where NAME like '%" + name + "%';";
            using (SQLiteDataReader rs = FindSymptomSt.ExecuteReader())
            {
                return GetSymptoms(rs);
            }
        }

        public List<Point> FindPoint(string name)
        {
            FindPointSt.CommandText = "SELECT * FROM POINTS where NAME like '%" + name + "%';";
            using (SQLiteDataReader rs = FindPointSt.ExecuteReader())
            {
                return GetPoints(rs);
            }
        }


        public List<Patient> FindPatient(string name)
        {
            FindPatientSt.CommandText = "SELECT * FROM PATIENT where NAME like '%" + name + "%';";
            using (SQLiteDataReader rs = FindPatientSt.ExecuteReader())
            {
                return GetPatients(rs);
            }
        }

        public List<ConnectionValue<Channel>> GetAllChannelRelativeToSymptom(Symptom symptom)
        {
            GetAllChannelRelativeToSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            using (SQLiteDataReader rs = GetAllChannelRelativeToSymptomSt.ExecuteReader())
            {
                List<ConnectionValue<Channel>> o = new List<ConnectionValue<Channel>>();
                while (rs.Read())
                    o.Add(new ConnectionValue<Channel>(GetChannel(rs), rs.GetIntL(ConnectionValue<Channel>.IMPORTENCE),
                            rs.GetStringL(ConnectionValue<Channel>.COMMENT)));
                return o;
            }
        }

        public List<ConnectionValue<Point>> GetAllPointRelativeToSymptom(Symptom symptom)
        {
            GetAllPointRelativeToSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            using (SQLiteDataReader rs = GetAllPointRelativeToSymptomSt.ExecuteReader())
            {
                List<ConnectionValue<Point>> o = new List<ConnectionValue<Point>>();
                while (rs.Read())
                    o.Add(new ConnectionValue<Point>(GetPoint(rs), rs.GetIntL(ConnectionValue<Point>.IMPORTENCE),
                            rs.GetStringL(ConnectionValue<Point>.COMMENT)));
                return o;
            }
        }

        public List<ConnectionValue<Symptom>> GetAllSymptomRelativeToPoint(Point point)
        {
            GetAllSymptomRelativeToPointSt.Parameters["@pointId"].Value = point.Id;
            using (SQLiteDataReader rs = GetAllSymptomRelativeToPointSt.ExecuteReader())
            {
                return GetSymptomsConnection(rs);
            }
        }

        public List<Meeting> GetAllMeetingsRelativeToSymptoms(List<Symptom> symptoms)
        {
            string sql = "select MEETING.*,count(MEETING.ID) from MEETING INNER JOIN MEETING_SYMPTOM ON MEETING.ID = MEETING_SYMPTOM.MEETING_ID where ";
            for (int i = 0; i < symptoms.Count; i++)
            {
                sql += " MEETING_SYMPTOM.SYMPTOM_ID = " + symptoms[i].Id;
                if (i < symptoms.Count - 1)
                    sql += " or ";
            }
            sql += " having count(MEETING.ID) > 0 order by count(MEETING.ID) DESC;";
            GetAllMeetingsRelativeToSymptomsSt.CommandText = sql;
            using (SQLiteDataReader rs = GetAllMeetingsRelativeToSymptomsSt.ExecuteReader())
            {
                return GetMeetings(rs);
            }
        }
        public List<Symptom> GetAllSymptomRelativeToMeeting(Meeting meeting)
        {
            GetAllSymptomRelativeToMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            using (SQLiteDataReader rs = GetAllSymptomRelativeToMeetingSt.ExecuteReader())
                return GetSymptoms(rs);
        }

        public Patient GetPatientRelativeToMeeting(Meeting meeting)
        {
            GetPatientRelativeToMeetingSt.Parameters["@patientId"].Value = meeting.PatientId;
            using (SQLiteDataReader rs = GetPatientRelativeToMeetingSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetPatient(rs);
                else
                    return null;
            }
        }

        public List<Point> GetAllPointsRelativeToMeeting(Meeting meeting)
        {
            GetAllPointRelativeToMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            using (SQLiteDataReader rs = GetAllPointRelativeToMeetingSt.ExecuteReader())
                return GetPoints(rs);
        }

        public Meeting GetTheLastMeeting(Patient patient)
        {
            GetTheLastMeetingSt.Parameters["@patientId"].Value = patient.Id;
            using (SQLiteDataReader rs = GetTheLastMeetingSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetMeeting(rs);
                else
                    return null;
                
            }
        }
        public List<Treatment.Treatment> GetAllTreatments()
        {
            using (SQLiteDataReader rs = GetAllTreatmentsSt.ExecuteReader())
            {
                return GetTreatments(rs);
            }        }

        private List<Treatment.Treatment> GetTreatments(SQLiteDataReader rs)
        {
            List<Treatment.Treatment> o = new List<Treatment.Treatment>();
            while(rs.Read())
            {
                o.Add(GetTreatment(rs));
            }
            return o;
        }

        private Treatment.Treatment GetTreatment(SQLiteDataReader rs)
        {
            return new Treatment.Treatment(rs.GetIntL(ID), rs.GetStringL(Treatment.Treatment.NAME));
        }

        public List<Treatment.Treatment> GetTreatmentsByName(string name)
        {
            GetTreatmentByNameSt.Parameters["@name"].Value = name;            using (SQLiteDataReader rs = GetTreatmentByNameSt.ExecuteReader())
            {
                return GetTreatments(rs);
            }
        }

        public List<Treatment.Operation> GetAllOperationsByTreatment(Treatment.Treatment treatment)
        {
            GetAllOperationsByTreatmentSt.Parameters["@treatmentId"].Value = treatment.Id;            using (SQLiteDataReader rs = GetAllOperationsByTreatmentSt.ExecuteReader())
            {
                return GetOperations(rs);
            }
        }

        private List<Operation> GetOperations(SQLiteDataReader rs)
        {
            List<Treatment.Operation> o = new List<Operation>();
            while(rs.Read())
            {
                o.Add(GetOperation(rs));
            }
            return o;
        }

        private Operation GetOperation(SQLiteDataReader rs)
        {
            return new Operation(rs.GetIntL(ID), rs.GetStringL(Operation.TEXT));
        }

        public Point GetAllPointByOperation(Treatment.Operation operation)
        {
            GetAllPointByOperationSt.Parameters["@operationId"].Value = operation.Id;            using (SQLiteDataReader rs = GetAllPointByOperationSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetPoint(rs);
                else
                    return null;
            }
        }

        public Note GetNoteByPoint(Point point)
        {
            GetNoteByPointSt.Parameters["@pointId"].Value = point.Id;            using (SQLiteDataReader rs = GetNoteByPointSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetNote(rs);
                else
                    return null;
            }
        }

        private List<Note> GetNotes(SQLiteDataReader rs)
        {
            List<Note> o = new List<Note>();
            while(rs.Read())
            {
                o.Add(GetNote(rs));
            }
            return o;
        }

        private Note GetNote(SQLiteDataReader rs)
        {
            return new Note(rs.GetIntL(ID), rs.GetStringL(Note.TEXT));
        }

        public Note GetNoteByTreatment(Treatment.Treatment treatment)
        {
            GetNoteByTreatmentSt.Parameters["@treatmentId"].Value = treatment.Id;            using (SQLiteDataReader rs = GetNoteByTreatmentSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetNote(rs);
                else
                    return null;
            }
        }

        public Note GetNoteByOperation(Treatment.Operation operation)
        {
            GetNoteByOperationSt.Parameters["@operationId"].Value = operation.Id;            using (SQLiteDataReader rs = GetNoteByOperationSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetNote(rs);
                else
                    return null;
            }
        }
        public List<Point> GetPointsByNote(Treatment.Note note)
        {
            GetPointsByNoteSt.Parameters["@noteTreatmentId"].Value = note.Id;            using (SQLiteDataReader rs = GetPointsByNoteSt.ExecuteReader())
            {
                return GetPoints(rs);
            }
        }
        public List<Operation> GetOperationsByNote(Treatment.Note note)
        {
            GetOperationsByNoteSt.Parameters["@noteTreatmentId"].Value = note.Id;            using (SQLiteDataReader rs = GetOperationsByNoteSt.ExecuteReader())
            {
                return GetOperations(rs);
            }
        }
        public Treatment.Symptom GetSymptomByPoint(Point point)
        {
            GetSymptomByPointSt.Parameters["@pointId"].Value = point.Id;            using (SQLiteDataReader rs = GetSymptomByPointSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetSymptomTreatment(rs);
                else
                    return null;
            }
        }

        private List<Treatment.Symptom> GetSymptomsTreatment(SQLiteDataReader rs)
        {
            List<Treatment.Symptom> o = new List<Treatment.Symptom>();
            while(rs.Read())
            {
                o.Add(GetSymptomTreatment(rs));
            }
            return o;
        }

        private Treatment.Symptom GetSymptomTreatment(SQLiteDataReader rs)
        {
            return new Treatment.Symptom(rs.GetIntL(ID), rs.GetStringL(Treatment.Symptom.TEXT));
        }

        public Treatment.Symptom GetSymptomByOperation(Treatment.Operation operation)
        {
            GetSymptomByOperationSt.Parameters["@operationId"].Value = operation.Id;            using (SQLiteDataReader rs = GetSymptomByOperationSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetSymptomTreatment(rs);
                else
                    return null;
            }
        }

        public List<Point> GetPointsBySymptom(Treatment.Symptom symptom)
        {
            GetPointsBySymptomSt.Parameters["@symptomId"].Value = symptom.Id;            using (SQLiteDataReader rs = GetPointsBySymptomSt.ExecuteReader())
            {
                return GetPoints(rs);
            }
        }
        #endregion
            #region inserts
        public Diagnostic SetDiagnostic(Diagnostic diagnostic)
        {
            if (diagnostic.Id == -1)
            {
                return InsertDiagnostic(diagnostic);
            }
                UpdateDiagnostic(diagnostic);
            return diagnostic;
        }
        public Channel InsertChannel(Channel channel)
        {
            InsertChannelSt.Parameters["@channelId"].Value = channel.Id;
            InsertChannelSt.Parameters["@name"].Value = channel.Name;
            InsertChannelSt.Parameters["@rt"].Value = channel.Rt;
            InsertChannelSt.Parameters["@aminPoint"].Value = channel.MainPoint;
            InsertChannelSt.Parameters["@evenPoint"].Value = channel.EvenPoint;
            InsertChannelSt.Parameters["@path"].Value = channel.Path;
            InsertChannelSt.Parameters["@role"].Value = channel.Role;
            InsertChannelSt.Parameters["@comments"].Value = channel.Comments;
            InsertChannelSt.ExecuteNonQuery();
            return channel;
        }

        private Diagnostic InsertDiagnostic(Diagnostic diagnostic)
        {
            InsertDiagnosticSt.Parameters["@Profession"].Value = diagnostic.Profession;
            InsertDiagnosticSt.Parameters["@MainComplaint"].Value = diagnostic.MainComplaint;
            InsertDiagnosticSt.Parameters["@SeconderyComlaint"].Value = diagnostic.SeconderyComlaint;
            InsertDiagnosticSt.Parameters["@DrugsUsed"].Value = diagnostic.DrugsUsed;
            InsertDiagnosticSt.Parameters["@TestsMade"].Value = diagnostic.TestsMade;
            InsertDiagnosticSt.Parameters["@IsPain"].Value = diagnostic.Pain.Value;
            InsertDiagnosticSt.Parameters["@PainInfo"].Value = diagnostic.Pain.Info;
            InsertDiagnosticSt.Parameters["@IsPainPreviousEvaluations"].Value = diagnostic.PainPreviousEvaluations.Value;
            InsertDiagnosticSt.Parameters["@PainPreviousEvaluationsInfo"].Value = diagnostic.PainPreviousEvaluations.Info;
            InsertDiagnosticSt.Parameters["@IsScans"].Value = diagnostic.Scans.Value;
            InsertDiagnosticSt.Parameters["@ScansInfo"].Value = diagnostic.Scans.Info;
            InsertDiagnosticSt.Parameters["@IsUnderStress"].Value = diagnostic.UnderStress.Value;
            InsertDiagnosticSt.Parameters["@UnderStressInfo"].Value = diagnostic.UnderStress.Info;
            InsertDiagnosticSt.Parameters["@IsTenseMuscles"].Value = diagnostic.TenseMuscles.Value;
            InsertDiagnosticSt.Parameters["@TenseMusclesInfo"].Value = diagnostic.TenseMuscles.Info;
            InsertDiagnosticSt.Parameters["@IsHighBloodPressureOrColesterol"].Value = diagnostic.HighBloodPressureOrColesterol.Value;
            InsertDiagnosticSt.Parameters["@HighBloodPressureOrColesterolInfo"].Value = diagnostic.HighBloodPressureOrColesterol.Info;
            InsertDiagnosticSt.Parameters["@IsGoodSleep"].Value = diagnostic.GoodSleep.Value;
            InsertDiagnosticSt.Parameters["@GoodSleepInfo"].Value = diagnostic.GoodSleep.Info;
            InsertDiagnosticSt.Parameters["@IsFallenToSleepProblem"].Value = diagnostic.FallenToSleepProblem.Value;
            InsertDiagnosticSt.Parameters["@FallenToSleepProblemInfo"].Value = diagnostic.FallenToSleepProblem.Info;
            InsertDiagnosticSt.Parameters["@IsPalpitations"].Value = diagnostic.Palpitations.Value;
            InsertDiagnosticSt.Parameters["@PalpitationsInfo"].Value = diagnostic.Palpitations.Info;
            InsertDiagnosticSt.Parameters["@DefeationRegularity"].Value = diagnostic.DefeationRegularity;
            InsertDiagnosticSt.Parameters["@IsFatigueOrFeelsFulAfterEating"].Value = diagnostic.FatigueOrFeelsFulAfterEating.Value;
            InsertDiagnosticSt.Parameters["@FatigueOrFeelsFulAfterEatingInfo"].Value = diagnostic.FatigueOrFeelsFulAfterEating.Info;
            InsertDiagnosticSt.Parameters["@IsDesireForSweetsAfterEating"].Value = diagnostic.DesireForSweetsAfterEating.Value;
            InsertDiagnosticSt.Parameters["@DesireForSweetsAfterEatingInfo"].Value = diagnostic.DesireForSweetsAfterEating.Info;
            InsertDiagnosticSt.Parameters["@IsDifficultyConcentating"].Value = diagnostic.DifficultyConcentating.Value;
            InsertDiagnosticSt.Parameters["@DifficultyConcentatingInfo"].Value = diagnostic.DifficultyConcentating.Info;
            InsertDiagnosticSt.Parameters["@IsOftenIll"].Value = diagnostic.OftenIll.Value;
            InsertDiagnosticSt.Parameters["@OftenIllInfo"].Value = diagnostic.OftenIll.Info;
            InsertDiagnosticSt.Parameters["@IsSufferingFromMucus"].Value = diagnostic.SufferingFromMucus.Value;
            InsertDiagnosticSt.Parameters["@SufferingFromMucusInfo"].Value = diagnostic.SufferingFromMucus.Info;
            InsertDiagnosticSt.Parameters["@IsCoughOrAllergySuffers"].Value = diagnostic.CoughOrAllergySuffers.Value;
            InsertDiagnosticSt.Parameters["@CoughOrAllergySuffersInfo"].Value = diagnostic.CoughOrAllergySuffers.Info;
            InsertDiagnosticSt.Parameters["@IsSmoking"].Value = diagnostic.Smoking.Value;
            InsertDiagnosticSt.Parameters["@SmokingInfo"].Value = diagnostic.Smoking.Info;
            InsertDiagnosticSt.Parameters["@IsFrequentOrUrgentUrination"].Value = diagnostic.FrequentOrUrgentUrination.Value;
            InsertDiagnosticSt.Parameters["@FrequentOrUrgentUrinationInfo"].Value = diagnostic.FrequentOrUrgentUrination.Info;
            InsertDiagnosticSt.Parameters["@PreferColdOrHot"].Value = diagnostic.PreferColdOrHot;
            InsertDiagnosticSt.Parameters["@PreferColdOrHotInfo"].Value = diagnostic.PreferColdOrHot.Info;
            InsertDiagnosticSt.Parameters["@IsSuffersFromColdOrHot"].Value = diagnostic.SuffersFromColdOrHot.Value;
            InsertDiagnosticSt.Parameters["@SuffersFromColdOrHotInfo"].Value = diagnostic.SuffersFromColdOrHot.Info;
            InsertDiagnosticSt.Parameters["@IsSatisfiedDients"].Value = diagnostic.SatisfiedDients.Value;
            InsertDiagnosticSt.Parameters["@SatisfiedDientsInfo"].Value = diagnostic.SatisfiedDients.Info;
            InsertDiagnosticSt.Parameters["@IsWantToLostWeight"].Value = diagnostic.WantToLostWeight.Value;
            InsertDiagnosticSt.Parameters["@WantToLostWeightInfo"].Value = diagnostic.WantToLostWeight.Info;
            InsertDiagnosticSt.Parameters["@IsUsingContraception"].Value = diagnostic.UsingContraception.Value;
            InsertDiagnosticSt.Parameters["@UsingContraceptionInfo"].Value = diagnostic.UsingContraception.Info;
            InsertDiagnosticSt.Parameters["@IsCycleRegular"].Value = diagnostic.CycleRegular.Value;
            InsertDiagnosticSt.Parameters["@CycleRegularInfo"].Value = diagnostic.CycleRegular.Info;
            InsertDiagnosticSt.Parameters["@IsSufferingFromCrampsOrNervousBeforeMenstruation"].Value = diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation.Value;
            InsertDiagnosticSt.Parameters["@SufferingFromCrampsOrNervousBeforeMenstruationInfo"].Value = diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation.Info;
            InsertDiagnosticSt.Parameters["@IsSufferingFromMenpause"].Value = diagnostic.SufferingFromMenpause.Value;
            InsertDiagnosticSt.Parameters["@SufferingFromMenpauseInfo"].Value = diagnostic.SufferingFromMenpause.Info;
            InsertDiagnosticSt.Parameters["@HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife"].Value = diagnostic.HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
            InsertDiagnosticSt.Parameters["@WhatChangesDoYouExpectToSeeFromTreatment"].Value = diagnostic.WhatChangesDoYouExpectToSeeFromTreatment;
            InsertDiagnosticSt.Parameters["@patientId"].Value = diagnostic.PatientId;
            InsertDiagnosticSt.Parameters["@creationDate"].Value = diagnostic.CreationDate;
            InsertDiagnosticSt.ExecuteNonQuery();

            long rowId = Connection.LastInsertRowId;

            if (rowId != 0)
            {
                int id = (int)(Int64)new SQLiteCommand("select ID from DIAGNOSTIC where rowId = " + rowId, Connection).ExecuteScalar();
                return new Diagnostic(id, diagnostic);
            }
            throw new Exception("ERORR:Insert didn't accure");
        }

        public void InsertSymptomChannelRelation(Symptom symptom, Channel channel, int importance, string comment)
        {
            InsertSymptomChannelRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            InsertSymptomChannelRelationSt.Parameters["@channelId"].Value = channel.Id;
            InsertSymptomChannelRelationSt.Parameters["@importance"].Value = importance;
            InsertSymptomChannelRelationSt.Parameters["@comment"].Value = comment;
            InsertSymptomChannelRelationSt.ExecuteNonQuery();
        }

        public void InsertMeetingPointRelation(Meeting meeting, Point point)
        {
            InsertMeetingPointRelationSt.Parameters["@meetingId"].Value = meeting.Id;
            InsertMeetingPointRelationSt.Parameters["@pointId"].Value = point.Id;
            InsertMeetingPointRelationSt.ExecuteNonQuery();
        }

        public void InsertSymptomPointRelation(Symptom symptom, Point point, int importance, string comment)
        {
            InsertSymptomPointRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            InsertSymptomPointRelationSt.Parameters["@pointId"].Value = point.Id;
            InsertSymptomPointRelationSt.Parameters["@importance"].Value = importance;
            InsertSymptomPointRelationSt.Parameters["@comment"].Value = comment;
            InsertSymptomPointRelationSt.ExecuteNonQuery();
        }

        public void InsertSymptomMeetingRelation(Symptom symptom, Meeting meeting)
        {
            InsertSymptomMeetingRelationSt.Parameters["@meetingId"].Value = meeting.Id;
            InsertSymptomMeetingRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            try
            {
                InsertSymptomMeetingRelationSt.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                if (e.ErrorCode == (int)SQLiteErrorCode.Constraint)
                {
                    throw new Exceptions.UniqueNameException();
                }
                throw e;
            }
        }

        public Symptom InsertSymptom(Symptom symptom)
        {
            InsertSymptomSt.Parameters["@name"].Value = symptom.Name;
            InsertSymptomSt.Parameters["@comment"].Value = symptom.Comment;
            InsertSymptomSt.ExecuteNonQuery();

            long rowId = Connection.LastInsertRowId;

            if (rowId != 0)
            {

                int id = (int)new SQLiteCommand("select SYMPTOM_ID from SYMPTOM where rowId = " + rowId, Connection).ExecuteScalar();
                return new Symptom(id, symptom);
            }
            throw new Exception("ERORR:Insert didn't accure");
        }

        public Meeting InsertMeeting(Meeting meeting)
        {
            InsertMeetingSt.Parameters["@patientId"].Value = meeting.PatientId;
            InsertMeetingSt.Parameters["@purpose"].Value = meeting.Purpose;
            InsertMeetingSt.Parameters["@date"].Value = meeting.Date;
            InsertMeetingSt.Parameters["@description"].Value = meeting.Description;
            InsertMeetingSt.Parameters["@summery"].Value = meeting.Summery;
            InsertMeetingSt.Parameters["@resultDescription"].Value = meeting.ResultDescription;
            InsertMeetingSt.Parameters["@resultValue"].Value = meeting.Result.Value;
            InsertMeetingSt.Parameters["@diagnosticId"].Value = meeting.DiagnosticId;
            InsertMeetingSt.ExecuteNonQuery();

            long rowId = Connection.LastInsertRowId;

            if (rowId != 0)
            {
                int id = (int)(long)new SQLiteCommand("select ID from MEETING where rowId = " + rowId, Connection).ExecuteScalar();
                return new Meeting(id, meeting);
            }
            throw new Exception("ERORR:insert meeting didn't accure");
        }

        public Patient InsertPatient(Patient patient)
        {
            try
            {
                InsertPatientSt.Parameters["@name"].Value = patient.Name;
                InsertPatientSt.Parameters["@telephone"].Value = patient.Telephone;
                InsertPatientSt.Parameters["@cellphone"].Value = patient.Cellphone;
                InsertPatientSt.Parameters["@birthday"].Value = patient.Birthday;
                InsertPatientSt.Parameters["@gender"].Value = patient.Gend.Value;
                InsertPatientSt.Parameters["@address"].Value = patient.Address;
                InsertPatientSt.Parameters["@email"].Value = patient.Email;
                InsertPatientSt.Parameters["@medicalDescription"].Value = patient.MedicalDescription;
                InsertPatientSt.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                if (e.ErrorCode == (int)SQLiteErrorCode.Constraint)
                    throw new Exceptions.UniqueNameException();
                else
                    throw e;
            }
            long rowId = Connection.LastInsertRowId;
            if (rowId != 0)
            {
                int id = (int)(long)new SQLiteCommand("select ID from PATIENT where rowId = " + rowId + ";", Connection).ExecuteScalar();
                return new Patient(id, patient);
            }
            throw new Exception("ERORR:insert patient didn't accure");
        }

        public Point InsertPoint(Point point)
        {
            InsertPointsSt.Parameters["@name"].Value = point.Name;
            InsertPointsSt.Parameters["@minNeedleDepth"].Value = point.MinNeedleDepth;
            InsertPointsSt.Parameters["@maxNeedleDepth"].Value = point.MaxNeedleDepth;
            InsertPointsSt.Parameters["@position"].Value = point.Position;
            InsertPointsSt.Parameters["@importance"].Value = point.Importance;
            InsertPointsSt.Parameters["@comment1"].Value = point.Comment1;
            InsertPointsSt.Parameters["@comment2"].Value = point.Comment2;
            InsertPointsSt.Parameters["@note"].Value = point.Note;
            InsertPointsSt.Parameters["@image"].Value = point.Image;
            InsertPointsSt.ExecuteNonQuery();

            long rowId = Connection.LastInsertRowId;
            if (rowId != 0)
            {
                int id = (int)new SQLiteCommand("select POINT_ID from POINTS where rowId = " + rowId, Connection).ExecuteScalar();
                return new Point(id, point);
            }
            throw new Exception("ERORR:insert point did't accure");
        }
        //TODO delete DIAGNOSTIC
        #region treatment inserts
        public Treatment.Treatment InsertTreatment(Treatment.Treatment treatment)
        {
            InsertTreatment_TreatmentSt.Parameters["@name"].Value = treatment.Name;
            InsertTreatment_TreatmentSt.ExecuteNonQuery();

            long rowId = Connection.LastInsertRowId;
            if (rowId != 0)
            {
                int id = (int)new SQLiteCommand("select ID from TREATMENT where rowId = " + rowId, Connection).ExecuteScalar();
                return new Treatment.Treatment(id, treatment);
            }
            throw new Exception("ERORR:insert point did't accure");
        }
        public Treatment.Operation InsertOperation(Treatment.Operation operation)
        {
            InsertTreatment_OperationSt.Parameters["@treatmentId"].Value = operation.TreatmentId;
            InsertTreatment_OperationSt.Parameters["@text"].Value = operation.Text;
            InsertTreatment_OperationSt.ExecuteNonQuery();

            long rowId = Connection.LastInsertRowId;
            if (rowId != 0)
            {
                int id = (int)new SQLiteCommand("select ID from OPERATION_TREATMENT where rowId = " + rowId, Connection).ExecuteScalar();
                return new Treatment.Operation(id, operation);
            }
            throw new Exception("ERORR:insert point did't accure");
        }
        public Treatment.Note InsertNote(Treatment.Note note)
        {
            InsertTreatment_NoteSt.Parameters["@text"].Value = note.Text;
            InsertTreatment_NoteSt.ExecuteNonQuery();

            long rowId = Connection.LastInsertRowId;
            if (rowId != 0)
            {
                int id = (int)new SQLiteCommand("select ID from NOTE_TREATMENT where rowId = " + rowId, Connection).ExecuteScalar();
                return new Treatment.Note(id, note);
            }
            throw new Exception("ERORR:insert point did't accure");
        }
        public Treatment.Symptom InsertSymptom(Treatment.Symptom symptom)
        {
            InsertTreatment_SymptomSt.Parameters["@text"].Value = symptom.Text;
            InsertTreatment_SymptomSt.ExecuteNonQuery();

            long rowId = Connection.LastInsertRowId;
            if (rowId != 0)
            {
                int id = (int)new SQLiteCommand("select ID from SYMPTOM_TREATMENT where rowId = " + rowId, Connection).ExecuteScalar();
                return new Treatment.Symptom(id, symptom);
            }
            throw new Exception("ERORR:insert point did't accure");
        }
        public void InsertNoteTreatmentRelation(Treatment.Note note,Treatment.Treatment treatment)
        {
            InsertTreatment_NoteTreatmentRelationSt.Parameters["@noteTreatmentId"].Value = note.Id;
            InsertTreatment_NoteTreatmentRelationSt.Parameters["@treatmentId"].Value = treatment.Id;
            InsertTreatment_NoteTreatmentRelationSt.ExecuteNonQuery();
        }
        public void InsertNoteOperationRelation(Treatment.Note note,Treatment.Operation operation)
        {
            InsertTreatment_NoteOperationSt.Parameters["@noteTreatmentId"].Value = note.Id;
            InsertTreatment_NoteOperationSt.Parameters["@operationId"].Value = operation.Id;
            InsertTreatment_NoteOperationSt.ExecuteNonQuery();
        }
        public void InsertNoteTreatmentPointRelation(Treatment.Note note,Treatment.Treatment treatment,Point point)
        {
            InsertTreatment_NotePointSt.Parameters["@noteTreatmentId"].Value = note.Id;
            InsertTreatment_NotePointSt.Parameters["@pointId"].Value = point.Id;
            InsertTreatment_NotePointSt.ExecuteNonQuery();
        }
        public void InsertSymptomPointRelation(Treatment.Symptom symptom,Point point)
        {
            InsertTreatment_SymptomPointRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            InsertTreatment_SymptomPointRelationSt.Parameters["@pointId"].Value = point.Id;
            InsertTreatment_SymptomPointRelationSt.ExecuteNonQuery();
        }
        public void InsertSymptomOperation(Treatment.Symptom symptom,Treatment.Operation operation)
        {
            InsertTreatment_SymptomOperationRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            InsertTreatment_SymptomOperationRelationSt.Parameters["@operationId"].Value = operation.Id;
            InsertTreatment_SymptomOperationRelationSt.ExecuteNonQuery();
        }
        #endregion
        
        #endregion
        #region get from SQLiteDataReader the objects
        private Channel GetChannel(SQLiteDataReader rs)
        {
            return new Channel(rs.GetIntL(ID), rs.GetStringL(Channel.NAME), rs.GetStringL(Channel.RT),
                    rs.GetIntL(Channel.MAIN_POINT), rs.GetIntL(Channel.EVEN_POINT), rs.GetStringL(Channel.PATH),
                    rs.GetStringL(Channel.ROLE), rs.GetStringL(Channel.COMMENT));
        }

        private Meeting GetMeeting(SQLiteDataReader rs)
        {
            return new Meeting(rs.GetIntL(ID), rs.GetIntL(Meeting.PATIENT_ID), rs.GetStringL(Meeting.PURPOSE),
                    rs.GetDateTimeL(Meeting.DATE), rs.GetStringL(Meeting.DESCRIPTION), rs.GetStringL(Meeting.SUMMERY),
                    rs.GetStringL(Meeting.RESULT_DESCRIPTION),
                    Meeting.ResultValue.FromValue(rs.GetIntL(Meeting.RESULT_VALUE)));
        }

        private List<Meeting> GetMeetings(SQLiteDataReader rs)
        {
            List<Meeting> o = new List<Meeting>();
            while (rs.Read())
                o.Add(GetMeeting(rs));
            return o;
        }

        private Patient GetPatient(SQLiteDataReader rs)
        {
            return new Patient(rs.GetIntL(ID), rs.GetStringL(Patient.NAME), rs.GetStringL(Patient.TELEPHONE),
                    rs.GetStringL(Patient.CELLPHONE), rs.GetDateTimeL(Patient.BIRTHDAY),
                    Patient.Gender.FromValue(rs.GetIntL(Patient.GENDER)), rs.GetStringL(Patient.ADDRESS), rs.GetStringL(Patient.EMAIL),
                    rs.GetStringL(Patient.MEDICAL_DESCRIPTION));
        }

        private List<Patient> GetPatients(SQLiteDataReader rs)
        {
            List<Patient> o = new List<Patient>();
            while (rs.Read())
                o.Add(GetPatient(rs));
            return o;
        }

        private Point GetPoint(SQLiteDataReader rs)
        {
            return new Point(rs.GetIntL(ID), rs.GetStringL(Point.NAME), rs.GetIntL(Point.MIN_NEEDLE_DEPTH),
                    rs.GetIntL(Point.MAX_NEEDLE_DEPTH), rs.GetStringL(Point.NEEDLE_DESCRIPTION), rs.GetStringL(Point.POSITION),
                    rs.GetIntL(Point.IMPORTENCE), rs.GetStringL(Point.COMMENT1), rs.GetStringL(Point.COMMENT2),
                    rs.GetStringL(Point.NOTE), rs.GetStringL(Point.IMAGE));
        }

        private List<Point> GetPoints(SQLiteDataReader rs)
        {
            List<Point> o = new List<Point>();
            while (rs.Read())
                o.Add(GetPoint(rs));
            return o;
        }

        private Symptom GetSymptom(SQLiteDataReader rs)
        {
            return new Symptom(rs.GetIntL(ID), rs.GetStringL(Symptom.NAME), rs.GetStringL(Symptom.COMMENT));
        }

        private List<Symptom> GetSymptoms(SQLiteDataReader rs)
        {
            List<Symptom> o = new List<Symptom>();
            while (rs.Read())
                o.Add(GetSymptom(rs));
            return o;
        }

        private ConnectionValue<Symptom> GetSymptomConnection(SQLiteDataReader rs)
        {
            return new ConnectionValue<Symptom>(GetSymptom(rs), rs.GetIntL(ConnectionValue<Symptom>.IMPORTENCE), rs.GetStringL(ConnectionValue<Symptom>.COMMENT));
        }

        private List<ConnectionValue<Symptom>> GetSymptomsConnection(SQLiteDataReader rs)
        {
            List<ConnectionValue<Symptom>> o = new List<ConnectionValue<Symptom>>();
            while (rs.Read())
                o.Add(GetSymptomConnection(rs));
            return o;
        }

        private Diagnostic GetDiagnostic(SQLiteDataReader rs)
        {
            return new Diagnostic(rs.GetIntL(ID))
            {
                Profession = rs.GetStringL(Diagnostic.PROFESSION),
                MainComplaint = rs.GetStringL(Diagnostic.MAIN_COMPLAINT),
                SeconderyComlaint = rs.GetStringL(Diagnostic.SECONDERY_COMPLAINT),
                DrugsUsed = rs.GetStringL(Diagnostic.DRUGS_USED),
                TestsMade = rs.GetStringL(Diagnostic.TESTS_MADE),
                Pain = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IN_PAIN), rs.GetStringL(Diagnostic.PAIN_INFO)),
                PainPreviousEvaluations = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_PAIN_PREVIOUS_EVALUATIONS), rs.GetStringL(Diagnostic.PAIN_PREVIOUS_EVALUATION_INFO)),
                Scans = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_THERE_ANYSORT_OF_SCANS), rs.GetStringL(Diagnostic.THE_SCANS_INFO)),
                UnderStress = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_UNDER_STRESS), rs.GetStringL(Diagnostic.STRESS_INFO)),
                TenseMuscles = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_TENSE_MUSCLES), rs.GetStringL(Diagnostic.TENSE_MUSCLES_INFO)),
                HighBloodPressureOrColesterol = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_HIGH_BLOOD_PRESSURE_OR_COLESTEROL), rs.GetStringL(Diagnostic.HIGH_BLOOD_PRESSURE_OR_COLESTEROL_INFO)),
                GoodSleep = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_GOOD_SLEEP), rs.GetStringL(Diagnostic.GOOD_SLEEP_INFO)),
                FallenToSleepProblem = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_FALLEN_TO_SLEEP_PROBLEM), rs.GetStringL(Diagnostic.FALLEN_TO_SLEEP_PROBLEM_INFO)),
                Palpitations = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_PALPITATIONS), rs.GetStringL(Diagnostic.PALPITATIONS_INFO)),
                DefeationRegularity = rs.GetStringL(Diagnostic.DEFECATION_REGULARITY),
                FatigueOrFeelsFulAfterEating = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_FATIGUE_OR_FEELS_FULL_AFTER_EATING), rs.GetStringL(Diagnostic.FATIGUE_OR_FEELS_FULL_AFTER_EATING_INFO)),
                DesireForSweetsAfterEating = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_DESIRE_FOR_SWEETS_AFTER_EATING), rs.GetStringL(Diagnostic.DESIRE_FOR_SWEETS_AFTER_EATING_INFO)),
                DifficultyConcentating = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_DIFFICULTY_CONCENTRATING), rs.GetStringL(Diagnostic.DIFFICULTY_CONCENTRATING_INFO)),
                OftenIll = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_OFTEN_ILL), rs.GetStringL(Diagnostic.OFTEN_ILL_INFO)),
                SufferingFromMucus = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_SUFFERING_FROM_MUCUS), rs.GetStringL(Diagnostic.SUFFERING_FROM_MUCUS_INFO)),
                CoughOrAllergySuffers = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_COUGH_OR_ALLERGY_SUFFERS), rs.GetStringL(Diagnostic.COUGH_OR_ALLERGY_SUFFERS_INFO)),
                Smoking = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_SMOKING), rs.GetStringL(Diagnostic.SMOKING_INFO)),
                FrequentOrUrgentUrination = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_FREQUENT_OR_URGENT_URINATION), rs.GetStringL(Diagnostic.FREQUENT_OR_URGENT_URINATION_INFO)),
                PreferColdOrHot = new ValueInfo<PreferColdOrHotType>((PreferColdOrHotType)rs.GetIntL(Diagnostic.PREFER_COLD_OR_HOT), rs.GetStringL(Diagnostic.PREFER_COLD_OR_HOT_INFO)),
                SuffersFromColdOrHot = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_SUFFERS_FROM_COLD_OR_HOT), rs.GetStringL(Diagnostic.SUFFERS_FROM_COLD_OR_HOT_INFO)),
                SatisfiedDients = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_SATISFIED_DIETS), rs.GetStringL(Diagnostic.SATISFIED_DIETS_INFO)),
                WantToLostWeight = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_WANT_TO_LOST_WEIGHT), rs.GetStringL(Diagnostic.WANT_TO_LOST_WEIGHT_INFO)),
                UsingContraception = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_USING_CONTRACEPTION), rs.GetStringL(Diagnostic.USING_CONTRACEPTION_INFO)),
                CycleRegular = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_CYCLE_REGULAR), rs.GetStringL(Diagnostic.CYCLE_REGULAR_INFO)),
                SufferingFromCrampsOrNervousBeforeMenstruation = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION), rs.GetStringL(Diagnostic.SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION_INFO)),
                SufferingFromMenpause = new ValueInfo<bool?>(rs.GetBoolL(Diagnostic.IS_SUFFERING_FROM_MENOPAUSE), rs.GetStringL(Diagnostic.SUFFERING_FROM_MENOPAUSE_INFO)),
                HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife = rs.GetStringL(Diagnostic.HOW_MANY_HOURS_A_WEEK_ARE_YOU_WILLING_TO_INVEST_TO_IMPROVE_THE_QUALITY_OF_LIFE),
                WhatChangesDoYouExpectToSeeFromTreatment = rs.GetStringL(Diagnostic.WHAT_CHANGES_DO_YOU_EXPECT_TO_SEE_FROM_TREATMENT),
                PatientId = rs.GetIntL(Diagnostic.PATIENT_ID),
                CreationDate = rs.GetDateTimeL(Diagnostic.CREATION_DATE)
            };
        }
        #endregion

        public Patient GetPatientRelativeToDiagnostic(Diagnostic diagnostic)
        {
            GetPatientRelativeToDiagnosticSt.Parameters["@patientId"].Value = diagnostic.PatientId;
            using (var rs = GetPatientRelativeToDiagnosticSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetPatient(rs);
                return null;
            }
        }
    }
}
