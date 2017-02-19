using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace AcupunctureProject.database
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

        public static bool GetBoolL(this SQLiteDataReader reader, string str)
        {
            return (bool)reader[str];
        }

        public static Color ReadColor(this BinaryReader reader)
        {
            return new Color() { R = reader.ReadByte(), G = reader.ReadByte(), B = reader.ReadByte(), A = 255 };
        }

        public static void Write(this BinaryWriter writer,Color color)
        {
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
        }
    }

    public class Database
    {
        private static readonly string ID = "ID";

        private static Database database = null;
        public static Database Instance
        {
            get
            {
                if (database == null)
                    database = new Database();
                return database;
            }
        }

        private SQLiteConnection connection;
        #region all SQLiteCommand declirations
        private SQLiteCommand getAllChannelRelativeToSymptomSt;
        private SQLiteCommand getAllPointRelativeToSymptomSt;
        private SQLiteCommand getAllSymptomRelativeToPointSt;
        private SQLiteCommand getAllMeetingsRelativeToPatientOrderByDateSt;
        private SQLiteCommand getAllSymptomRelativeToMeetingSt;
        private SQLiteCommand getAllPointRelativeToMeetingSt;

        private SQLiteCommand getPatientRelativeToMeetingSt;

        private SQLiteCommand insertSymptomSt;
        private SQLiteCommand insertMeetingSt;
        private SQLiteCommand insertPatientSt;
        private SQLiteCommand insertPointsSt;
        private SQLiteCommand insertChannelSt;
        private SQLiteCommand insertDiagnosticSt;

        private SQLiteCommand updateSymptomSt;
        private SQLiteCommand updateMeetingSt;
        private SQLiteCommand updatePatientSt;
        private SQLiteCommand updatePointSt;
        private SQLiteCommand updateChannelSt;
        private SQLiteCommand updateChannelSymptomRelationSt;
        private SQLiteCommand updatePointSymptomRelationSt;
        private SQLiteCommand updateDiagnosticSt;

        private SQLiteCommand deleteSymptomSt;
        private SQLiteCommand deleteMeetingSt;
        private SQLiteCommand deletePatientSt;
        private SQLiteCommand deletePointSt;
        private SQLiteCommand deleteChannelSt;
        private SQLiteCommand deleteSymptomPointRelationSt;
        private SQLiteCommand deleteSymptomMeetingRelationSt;
        private SQLiteCommand deleteSymptomChannelRelationSt;
        private SQLiteCommand deleteMeetingPointSt;
        private SQLiteCommand deleteDiagnosticSt;

        private SQLiteCommand insertSymptomPointRelationSt;
        private SQLiteCommand insertSymptomMeetingRelationSt;
        private SQLiteCommand insertMeetingPointRelationSt;
        private SQLiteCommand insertSymptomChannelRelationSt;

        private SQLiteCommand getSymptomSt;
        private SQLiteCommand getPointByNameSt;
        private SQLiteCommand getPointByIdSt;
        private SQLiteCommand getAllDiagnosticByPatientSt;
        private SQLiteCommand getDiagnosticByMeetingSt;

        private SQLiteCommand getChannelByIdSt;
        private SQLiteCommand getTheLastMeetingSt;

        private SQLiteCommand findSymptomSt;
        private SQLiteCommand findPatientSt;
        private SQLiteCommand findPointSt;

        private SQLiteCommand getAllMeetingsRelativeToSymptomsSt;

        private SQLiteCommand getAllPointsSt;

        private Color[] priorityColors;
        public readonly static int NUM_OF_PRIORITIES = 6;
        private readonly static string SETTING_COLOR_FILE = "setting_color.dat";
        private readonly string folder;
        #endregion
        private Database()
        {
            string[] tempFolder = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');
            folder = "";
            for (int i = 0; i < tempFolder.Length - 1; i++)
            {
                folder += tempFolder[i] + "\\";
            }
            connection = new SQLiteConnection("Data Source=" + folder + "database.db;Version=3;");
            connection.Open();

            getAllPointRelativeToSymptomSt = new SQLiteCommand("select POINTS.* , SYMPTOM_POINTS.IMPORTENCE , SYMPTOM_POINTS.COMMENT from POINTS INNER JOIN SYMPTOM_POINTS ON POINTS.ID = SYMPTOM_POINTS.POINT_ID and SYMPTOM_POINTS.SYMPTOM_ID = @symptomId order by SYMPTOM_POINTS.IMPORTENCE DESC;", connection);
            getAllPointRelativeToSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));

            getAllSymptomRelativeToPointSt = new SQLiteCommand("select SYMPTOM.* , SYMPTOM_POINTS.IMPORTENCE , SYMPTOM_POINTS.COMMENT from SYMPTOM INNER JOIN SYMPTOM_POINTS ON SYMPTOM.ID = SYMPTOM_POINTS.SYMPTOM_ID where SYMPTOM_POINTS.POINT_ID = @pointId order by SYMPTOM_POINTS.IMPORTENCE DESC;", connection);
            getAllSymptomRelativeToPointSt.Parameters.Add(new SQLiteParameter("@pointId"));

            getAllChannelRelativeToSymptomSt = new SQLiteCommand("SELECT CHANNEL.* , SYMPTOM_CHANNEL.IMPORTENCE , SYMPTOM_CHANNEL.COMMENT from CHANNEL INNER JOIN SYMPTOM_CHANNEL ON CHANNEL.ID = SYMPTOM_CHANNEL.CHANNEL_ID where SYMPTOM_CHANNEL.SYMPTOM_ID = @symptomId order by SYMPTOM_CHANNEL.IMPORTENCE DESC;", connection);
            getAllChannelRelativeToSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));

            getAllSymptomRelativeToMeetingSt = new SQLiteCommand("SELECT SYMPTOM.* FROM MEETING_SYMPTOM INNER JOIN MEETING ON MEETING_SYMPTOM.MEETING_ID = MEETING.ID INNER JOIN SYMPTOM ON SYMPTOM.ID = MEETING_SYMPTOM.SYMPTOM_ID WHERE MEETING.ID = @meetingId;", connection);
            getAllSymptomRelativeToMeetingSt.Parameters.Add(new SQLiteParameter("@meetingId"));

            getAllPointRelativeToMeetingSt = new SQLiteCommand("SELECT POINTS.* FROM MEETING_POINTS INNER JOIN MEETING ON MEETING_POINTS.MEETING_ID = MEETING.ID INNER JOIN POINTS ON MEETING_POINTS.POINT_ID = POINTS.ID WHERE MEETING.ID = @meetingId", connection);
            getAllPointRelativeToMeetingSt.Parameters.Add(new SQLiteParameter("@meetingId"));

            getPatientRelativeToMeetingSt = new SQLiteCommand("SELECT PATIENT.* FROM MEETING INNER JOIN PATIENT ON MEETING.PATIENT_ID = PATIENT.ID WHERE MEETING.ID = @meetingId", connection);
            getPatientRelativeToMeetingSt.Parameters.Add(new SQLiteParameter("@meetingId"));

            insertSymptomSt = new SQLiteCommand("insert into SYMPTOM(NAME,COMMENT) values(@name,@comment);", connection);
            insertSymptomSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@comment") });

            insertMeetingSt = new SQLiteCommand("insert into MEETING(PATIENT_ID,PURPOSE,DATE,DESCRIPTION,SUMMERY,RESULT_DESCRIPTION,RESULT_VALUE,DIAGNOSTIC_ID) values(@patientId,@purpose,@date,@description,@summery,@resultDescription,@resultValue,@diagnosticId);", connection);
            insertMeetingSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@patientId"), new SQLiteParameter("@purpose"), new SQLiteParameter("@date"), new SQLiteParameter("@description"), new SQLiteParameter("@summery"), new SQLiteParameter("@resultDescription"), new SQLiteParameter("@resultValue") ,new SQLiteParameter("@diagnosticId")});

            insertPatientSt = new SQLiteCommand("insert into PATIENT(NAME,TELEPHONE,CELLPHONE,BIRTHDAY,GENDER,ADDRESS,EMAIL,MEDICAL_DESCRIPTION) values(@name,@telephone,@cellphone,@birthday,@gender,@address,@email,@medicalDescription);", connection);
            insertPatientSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@telephone"), new SQLiteParameter("@cellphone"), new SQLiteParameter("@birthday"), new SQLiteParameter("@gender"), new SQLiteParameter("@address"), new SQLiteParameter("@email"), new SQLiteParameter("@medicalDescription") });

            insertPointsSt = new SQLiteCommand("insert into POINTS(NAME,MIN_NEEDLE_DEPTH,MAX_NEEDLE_DEPTH,POSITION,IMPORTENCE,COMMENT1,COMMENT2,NOTE,IMAGE) values(@name,@minNeedleDepth,@maxNeedleDepth,@position,@importance,@comment1,@comment2,@note,@image);", connection);
            insertPointsSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@minNeedleDepth"), new SQLiteParameter("@maxNeedleDepth"), new SQLiteParameter("@position"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment1"), new SQLiteParameter("@comment2"), new SQLiteParameter("@note"), new SQLiteParameter("@image") });

            insertSymptomPointRelationSt = new SQLiteCommand("insert into SYMPTOM_POINTS values(@symptomId,@pointId,@importance,@comment);", connection);
            insertSymptomPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });

            insertSymptomMeetingRelationSt = new SQLiteCommand("insert into MEETING_SYMPTOM(MEETING_ID,SYMPTOM_ID) values(@meetingId,@symptomId);", connection);
            insertSymptomMeetingRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@meetingId"), new SQLiteParameter("@symptomId") });

            insertMeetingPointRelationSt = new SQLiteCommand("insert into MEETING_POINTS(MEETING_ID,POINT_ID) values(@meetingId,@pointId);", connection);
            insertMeetingPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@meetingId"), new SQLiteParameter("@pointId") });

            insertChannelSt = new SQLiteCommand("insert into CHANNEL values(@channelId,@name,@rt,@mainPoint,@evenPoint,@path,@role,@comments);", connection);
            insertChannelSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@channelId"), new SQLiteParameter("@name"), new SQLiteParameter("@rt"), new SQLiteParameter("@mainPoint"), new SQLiteParameter("@evenPoint"), new SQLiteParameter("@path"), new SQLiteParameter("@role"), new SQLiteParameter("@comments") });

            insertSymptomChannelRelationSt = new SQLiteCommand("insert into SYMPTOM_CHANNEL values(@symptomId,@channelId,@importance,@comment);", connection);
            insertSymptomChannelRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@channelId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });

            insertDiagnosticSt = new SQLiteCommand("insert into DIAGNOSTIC(PROFESSION,MAIN_COMPLAINT,SECONDERY_COMPLAINT,DRUGS_USED,TESTS_MADE,IN_PAIN BOOLEAN,PAIN_INFO,IS_PAIN_PREVIOUS_EVALUATIONS,PAIN_PREVIOUS_EVALUATION_INFO,IS_THERE_ANYSORT_OF_SCANS,THE_SCANS_INFO,IS_UNDER_STRESS,STRESS_INFO,IS_TENSE_MUSCLES,TENSE_MUSCLES_INFO,IS_HIGH_BLOOD_PRESSURE_OR_COLESTEROL,HIGH_BLOOD_PRESSURE_OR_COLESTEROL_INFO,IS_GOOD_SLEEP,GOOD_SLEEP_INFO,IS_FALLEN_TO_SLEEP_PROBLEM,FALLEN_TO_SLEEP_PROBLEM_INFO,IS_PALPITATIONS,PALPITATIONS_INFO,DEFECATION_REGULARITY,IS_FATIGUE_OR_FEELS_FULL_AFTER_EATING,FATIGUE_OR_FEELS_FULL_AFTER_EATING_INFO,IS_DESIRE_FOR_SWEETS_AFTER_EATING,DESIRE_FOR_SWEETS_AFTER_EATING_INFO,IS_DIFFICULTY_CONCENTRATING,DIFFICULTY_CONCENTRATING_INFO,IS_OFTEN_ILL,OFTEN_ILL_INFO,IS_SUFFERING_FROM_MUCUS,SUFFERING_FROM_MUCUS_INFO,IS_COUGH_OR_ALLERGY_SUFFERS,COUGH_OR_ALLERGY_SUFFERS_INFO,IS_SMOKING,SMOKING_INFO,IS_FREQUENT_OR_URGENT_URINATION,FREQUENT_OR_URGENT_URINATION_INFO,PREFER_COLD_OR_HOT,PREFER_COLD_OR_HOT_INFO,IS_SUFFERS_FROM_COLD_OR_HOT,SUFFERS_FROM_COLD_OR_HOT_INFO,IS_SATISFIED_DIETS,SATISFIED_DIETS_INFO,IS_WANT_TO_LOST_WEIGHT,WANT_TO_LOST_WEIGHT_INFO,IS_USING_CONTRACEPTION,USING_CONTRACEPTION_INFO,IS_CYCLE_REGULAR,CYCLE_REGULAR_INFO,IS_SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION,SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION_INFO,IS_SUFFERING_FROM_MENOPAUSE,SUFFERING_FROM_MENOPAUSE_INFO,HOW_MANY_HOURS_A_WEEK_ARE_YOU_WILLING_TO_INVEST_TO_IMPROVE_THE_QUALITY_OF_LIFE,WHAT_CHANGES_DO_YOU_EXPECT_TO_SEE_FROM_TREATMENT,PATIENT_ID,CREATION_DATE) values(@Profession, @MainComplaint, @SeconderyComlaint, @DrugsUsed, @TestsMade, @IsPain, @PainInfo , @IsPainPreviousEvaluations, @PainPreviousEvaluationsInfo, @IsScans, @ScansInfo, @IsUnderStress, @UnderStressInfo, @IsTenseMuscles, @TenseMusclesInfo, @IsHighBloodPressureOrColesterol, @HighBloodPressureOrColesterolInfo, @IsGoodSleep, @GoodSleepInfo, @IsFallenToSleepProblem, @FallenToSleepProblemInfo, @IsPalpitations, @PalpitationsInfo, @DefeationRegularity, @IsFatigueOrFeelsFulAfterEating, @FatigueOrFeelsFulAfterEatingInfo, @IsDesireForSweetsAfterEating, @DesireForSweetsAfterEatingInfo, @IsDifficultyConcentating, @DifficultyConcentatingInfo, @IsOftenIll, @OftenIllInfo, @IsSufferingFromMucus, @SufferingFromMucusInfo, @IsCoughOrAllergySuffers, @CoughOrAllergySuffersInfo, @IsSmoking, @SmokingInfo, @IsFrequentOrUrgentUrination, @FrequentOrUrgentUrinationInfo, @PreferColdOrHot, @PreferColdOrHotInfo, @IsSuffersFromColdOrHot, @SuffersFromColdOrHotInfo, @IsSatisfiedDients, @SatisfiedDientsInfo, @IsWantToLostWeight, @WantToLostWeightInfo, @IsUsingContraception, @UsingContraceptionInfo, @IsCycleRegular, @CycleRegularInfo, @IsSufferingFromCrampsOrNervousBeforeMenstruation, @SufferingFromCrampsOrNervousBeforeMenstruationInfo, @IsSufferingFromMenpause, @SufferingFromMenpauseInfo, @HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife, @WhatChangesDoYouExpectToSeeFromTreatment,@patientId,@creationDate);", connection);
            insertDiagnosticSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@Profession"), new SQLiteParameter("@MainComplaint"), new SQLiteParameter("@SeconderyComlaint"), new SQLiteParameter("@DrugsUsed"), new SQLiteParameter("@TestsMade"), new SQLiteParameter("@IsPain"), new SQLiteParameter("@PainInfo"), new SQLiteParameter("@IsPainPreviousEvaluations"), new SQLiteParameter("@PainPreviousEvaluationsInfo"), new SQLiteParameter("@IsScans"), new SQLiteParameter("@ScansInfo"), new SQLiteParameter("@IsUnderStress"), new SQLiteParameter("@UnderStressInfo"), new SQLiteParameter("@IsTenseMuscles"), new SQLiteParameter("@TenseMusclesInfo"), new SQLiteParameter("@IsHighBloodPressureOrColesterol"), new SQLiteParameter("@HighBloodPressureOrColesterolInfo"), new SQLiteParameter("@IsGoodSleep"), new SQLiteParameter("@GoodSleepInfo"), new SQLiteParameter("@IsFallenToSleepProblem"), new SQLiteParameter("@FallenToSleepProblemInfo"), new SQLiteParameter("@IsPalpitations"), new SQLiteParameter("@PalpitationsInfo"), new SQLiteParameter("@DefeationRegularity"), new SQLiteParameter("@IsFatigueOrFeelsFulAfterEating"), new SQLiteParameter("@FatigueOrFeelsFulAfterEatingInfo"), new SQLiteParameter("@IsDesireForSweetsAfterEating"), new SQLiteParameter("@DesireForSweetsAfterEatingInfo"), new SQLiteParameter("@IsDifficultyConcentating"), new SQLiteParameter("@DifficultyConcentatingInfo"), new SQLiteParameter("@IsOftenIll"), new SQLiteParameter("@OftenIllInfo"), new SQLiteParameter("@IsSufferingFromMucus"), new SQLiteParameter("@SufferingFromMucusInfo"), new SQLiteParameter("@IsCoughOrAllergySuffers"), new SQLiteParameter("@CoughOrAllergySuffersInfo"), new SQLiteParameter("@IsSmoking"), new SQLiteParameter("@SmokingInfo"), new SQLiteParameter("@IsFrequentOrUrgentUrination"), new SQLiteParameter("@FrequentOrUrgentUrinationInfo"), new SQLiteParameter("@PreferColdOrHot"), new SQLiteParameter("@PreferColdOrHotInfo"), new SQLiteParameter("@IsSuffersFromColdOrHot"), new SQLiteParameter("@SuffersFromColdOrHotInfo"), new SQLiteParameter("@IsSatisfiedDients"), new SQLiteParameter("@SatisfiedDientsInfo"), new SQLiteParameter("@IsWantToLostWeight"), new SQLiteParameter("@WantToLostWeightInfo"), new SQLiteParameter("@IsUsingContraception"), new SQLiteParameter("@UsingContraceptionInfo"), new SQLiteParameter("@IsCycleRegular"), new SQLiteParameter("@CycleRegularInfo"), new SQLiteParameter("@IsSufferingFromCrampsOrNervousBeforeMenstruation"), new SQLiteParameter("@SufferingFromCrampsOrNervousBeforeMenstruationInfo"), new SQLiteParameter("@IsSufferingFromMenpause"), new SQLiteParameter("@SufferingFromMenpauseInfo"), new SQLiteParameter("@HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife"), new SQLiteParameter("@WhatChangesDoYouExpectToSeeFromTreatment"),new SQLiteParameter("@patientId") ,new SQLiteParameter("@creationDate")});

            updateSymptomSt = new SQLiteCommand("update SYMPTOM set NAME = @name ,COMMENT = @comment where ID = @symptomId;", connection);
            updateSymptomSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@comment") });

            updateMeetingSt = new SQLiteCommand("update MEETING set PATIENT_ID = @patientId ,PURPOSE = @purpose ,DATE = @date ,DESCRIPTION = @description ,SUMMERY = @summery ,RESULT_DESCRIPTION = @resultDecription ,RESULT_VALUE = @resultValue, DIAGNOSTIC_ID = @diagnosticId where ID = @meetingId;", connection);
            updateMeetingSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@patientId"), new SQLiteParameter("@purpose"), new SQLiteParameter("@date"), new SQLiteParameter("@description"), new SQLiteParameter("@summery"), new SQLiteParameter("@resultDecription"), new SQLiteParameter("@resultValue"), new SQLiteParameter("@meetingId"), new SQLiteParameter("@diagnosticId") });

            updatePatientSt = new SQLiteCommand("update PATIENT set NAME = @name ,TELEPHONE = @telephone ,CELLPHONE = @cellphone ,BIRTHDAY = @birthday ,GENDER = @gander ,ADDRESS = @address ,EMAIL = @email ,MEDICAL_DESCRIPTION = @medicalDescription where ID = @patientId;", connection);
            updatePatientSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@telephone"), new SQLiteParameter("@cellphone"), new SQLiteParameter("@birthday"), new SQLiteParameter("@gander"), new SQLiteParameter("@address"), new SQLiteParameter("@email"), new SQLiteParameter("@medicalDescription"), new SQLiteParameter("@patientId") });

            updatePointSt = new SQLiteCommand("update POINTS set NAME = @name ,MIN_NEEDLE_DEPTH = @minNeedleDepth ,MAX_NEEDLE_DEPTH = @maxNeedleDepth ,POSITION = @position ,IMPORTENCE = @importance ,COMMENT1 = @comment1 ,COMMENT2 = @comment2,NOTE = @note,IMAGE = @image where ID = @pointId;", connection);
            updatePointSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@name"), new SQLiteParameter("@minNeedleDepth"), new SQLiteParameter("@maxNeedleDepth"), new SQLiteParameter("@position"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment1"), new SQLiteParameter("@comment2"), new SQLiteParameter("@note"), new SQLiteParameter("@image"), new SQLiteParameter("@pointId") });

            updateChannelSt = new SQLiteCommand("update CHANNEL set NAME = @name, RT = @rt ,MAIN_POINT = @mainPoint ,EVEN_POINT = @evenPoint ,PATH = @path ,ROLE = @role ,COMMENT = @comment where ID = @channelId;", connection);
            updateChannelSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@channelId"), new SQLiteParameter("@name"), new SQLiteParameter("@rt"), new SQLiteParameter("@mainPoint"), new SQLiteParameter("@evenPoint"), new SQLiteParameter("@path"), new SQLiteParameter("@role"), new SQLiteParameter("@comments") });

            updateChannelSymptomRelationSt = new SQLiteCommand("update SYMPTOM_CHANNEL set IMPORTENCE = @importance ,COMMENT = @comment where CHANNEL_ID = @channelId and SYMPTOM_ID = @symptomId;", connection);
            updateChannelSymptomRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@channelId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });

            updatePointSymptomRelationSt = new SQLiteCommand("update SYMPTOM_POINTS set IMPORTENCE = @importance ,COMMENT = @comment where POINT_ID = @pointId and SYMPTOM_ID = @symptomId;", connection);
            updatePointSymptomRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId"), new SQLiteParameter("@importance"), new SQLiteParameter("@comment") });

            updateDiagnosticSt = new SQLiteCommand("update DIAGNOSTIC set PROFESSION = @Profession, MAIN_COMPLAINT = @MainComplaint, SECONDERY_COMPLAINT = @SeconderyComlaint, DRUGS_USED = @DrugsUsed, TESTS_MADE = @TestsMade, IN_PAIN = @IsPain, PAIN_INFO = @PainInfo , IS_PAIN_PREVIOUS_EVALUATIONS = @IsPainPreviousEvaluations, PAIN_PREVIOUS_EVALUATION_INFO = @PainPreviousEvaluationsInfo, IS_THERE_ANYSORT_OF_SCANS = @IsScans, THE_SCANS_INFO = @ScansInfo, IS_UNDER_STRESS = @IsUnderStress, STRESS_INFO = @UnderStressInfo, IS_TENSE_MUSCLES = @IsTenseMuscles, TENSE_MUSCLES_INFO = @TenseMusclesInfo, IS_HIGH_BLOOD_PRESSURE_OR_COLESTEROL = @IsHighBloodPressureOrColesterol, HIGH_BLOOD_PRESSURE_OR_COLESTEROL_INFO = @HighBloodPressureOrColesterolInfo, IS_GOOD_SLEEP = @IsGoodSleep, GOOD_SLEEP_INFO = @GoodSleepInfo, IS_FALLEN_TO_SLEEP_PROBLEM = @IsFallenToSleepProblem, FALLEN_TO_SLEEP_PROBLEM_INFO = @FallenToSleepProblemInfo, IS_PALPITATIONS = @IsPalpitations, PALPITATIONS_INFO = @PalpitationsInfo, DEFECATION_REGULARITY = @DefeationRegularity, IS_FATIGUE_OR_FEELS_FULL_AFTER_EATING = @IsFatigueOrFeelsFulAfterEating, FATIGUE_OR_FEELS_FULL_AFTER_EATING_INFO = @FatigueOrFeelsFulAfterEatingInfo, IS_DESIRE_FOR_SWEETS_AFTER_EATING = @IsDesireForSweetsAfterEating, DESIRE_FOR_SWEETS_AFTER_EATING_INFO = @DesireForSweetsAfterEatingInfo, IS_DIFFICULTY_CONCENTRATING = @IsDifficultyConcentating, DIFFICULTY_CONCENTRATING_INFO = @DifficultyConcentatingInfo, IS_OFTEN_ILL = @IsOftenIll, OFTEN_ILL_INFO = @OftenIllInfo, IS_SUFFERING_FROM_MUCUS = @IsSufferingFromMucus, SUFFERING_FROM_MUCUS_INFO = @SufferingFromMucusInfo, IS_COUGH_OR_ALLERGY_SUFFERS = @IsCoughOrAllergySuffers, COUGH_OR_ALLERGY_SUFFERS_INFO = @CoughOrAllergySuffersInfo, IS_SMOKING = @IsSmoking, SMOKING_INFO = @SmokingInfo, IS_FREQUENT_OR_URGENT_URINATION = @IsFrequentOrUrgentUrination, FREQUENT_OR_URGENT_URINATION_INFO = @FrequentOrUrgentUrinationInfo, PREFER_COLD_OR_HOT = @PreferColdOrHot, PREFER_COLD_OR_HOT_INFO = @PreferColdOrHotInfo, IS_SUFFERS_FROM_COLD_OR_HOT = @IsSuffersFromColdOrHot, SUFFERS_FROM_COLD_OR_HOT_INFO = @SuffersFromColdOrHotInfo, IS_SATISFIED_DIETS = @IsSatisfiedDients, SATISFIED_DIETS_INFO = @SatisfiedDientsInfo, IS_WANT_TO_LOST_WEIGHT = @IsWantToLostWeight, WANT_TO_LOST_WEIGHT_INFO = @WantToLostWeightInfo, IS_USING_CONTRACEPTION = @IsUsingContraception, USING_CONTRACEPTION_INFO = @UsingContraceptionInfo, IS_CYCLE_REGULAR = @IsCycleRegular, CYCLE_REGULAR_INFO = @CycleRegularInfo, IS_SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION = @IsSufferingFromCrampsOrNervousBeforeMenstruation, SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION_INFO = @SufferingFromCrampsOrNervousBeforeMenstruationInfo, IS_SUFFERING_FROM_MENOPAUSE = @IsSufferingFromMenpause, SUFFERING_FROM_MENOPAUSE_INFO = @SufferingFromMenpauseInfo, HOW_MANY_HOURS_A_WEEK_ARE_YOU_WILLING_TO_INVEST_TO_IMPROVE_THE_QUALITY_OF_LIFE = @HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife, WHAT_CHANGES_DO_YOU_EXPECT_TO_SEE_FROM_TREATMENT = @WhatChangesDoYouExpectToSeeFromTreatment , PATIENT_ID = @patientId , CREATION_DATE = @creationDate where ID = @Id;",connection);
            updateDiagnosticSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@Profession"), new SQLiteParameter("@MainComplaint"), new SQLiteParameter("@SeconderyComlaint"), new SQLiteParameter("@DrugsUsed"), new SQLiteParameter("@TestsMade"), new SQLiteParameter("@IsPain"), new SQLiteParameter("@PainInfo"), new SQLiteParameter("@IsPainPreviousEvaluations"), new SQLiteParameter("@PainPreviousEvaluationsInfo"), new SQLiteParameter("@IsScans"), new SQLiteParameter("@ScansInfo"), new SQLiteParameter("@IsUnderStress"), new SQLiteParameter("@UnderStressInfo"), new SQLiteParameter("@IsTenseMuscles"), new SQLiteParameter("@TenseMusclesInfo"), new SQLiteParameter("@IsHighBloodPressureOrColesterol"), new SQLiteParameter("@HighBloodPressureOrColesterolInfo"), new SQLiteParameter("@IsGoodSleep"), new SQLiteParameter("@GoodSleepInfo"), new SQLiteParameter("@IsFallenToSleepProblem"), new SQLiteParameter("@FallenToSleepProblemInfo"), new SQLiteParameter("@IsPalpitations"), new SQLiteParameter("@PalpitationsInfo"), new SQLiteParameter("@DefeationRegularity"), new SQLiteParameter("@IsFatigueOrFeelsFulAfterEating"), new SQLiteParameter("@FatigueOrFeelsFulAfterEatingInfo"), new SQLiteParameter("@IsDesireForSweetsAfterEating"), new SQLiteParameter("@DesireForSweetsAfterEatingInfo"), new SQLiteParameter("@IsDifficultyConcentating"), new SQLiteParameter("@DifficultyConcentatingInfo"), new SQLiteParameter("@IsOftenIll"), new SQLiteParameter("@OftenIllInfo"), new SQLiteParameter("@IsSufferingFromMucus"), new SQLiteParameter("@SufferingFromMucusInfo"), new SQLiteParameter("@IsCoughOrAllergySuffers"), new SQLiteParameter("@CoughOrAllergySuffersInfo"), new SQLiteParameter("@IsSmoking"), new SQLiteParameter("@SmokingInfo"), new SQLiteParameter("@IsFrequentOrUrgentUrination"), new SQLiteParameter("@FrequentOrUrgentUrinationInfo"), new SQLiteParameter("@PreferColdOrHot"), new SQLiteParameter("@PreferColdOrHotInfo"), new SQLiteParameter("@IsSuffersFromColdOrHot"), new SQLiteParameter("@SuffersFromColdOrHotInfo"), new SQLiteParameter("@IsSatisfiedDients"), new SQLiteParameter("@SatisfiedDientsInfo"), new SQLiteParameter("@IsWantToLostWeight"), new SQLiteParameter("@WantToLostWeightInfo"), new SQLiteParameter("@IsUsingContraception"), new SQLiteParameter("@UsingContraceptionInfo"), new SQLiteParameter("@IsCycleRegular"), new SQLiteParameter("@CycleRegularInfo"), new SQLiteParameter("@IsSufferingFromCrampsOrNervousBeforeMenstruation"), new SQLiteParameter("@SufferingFromCrampsOrNervousBeforeMenstruationInfo"), new SQLiteParameter("@IsSufferingFromMenpause"), new SQLiteParameter("@SufferingFromMenpauseInfo"), new SQLiteParameter("@HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife"), new SQLiteParameter("@WhatChangesDoYouExpectToSeeFromTreatment"),new SQLiteParameter("@Id") ,new SQLiteParameter("@patientId"),new SQLiteParameter("@creationDate")});

            deleteSymptomSt = new SQLiteCommand("delete from SYMPTOM where ID = @symptomId;", connection);
            deleteSymptomSt.Parameters.Add(new SQLiteParameter("@symptomId"));

            deleteMeetingSt = new SQLiteCommand("delete from MEETING where ID = @meetingId;", connection);
            deleteMeetingSt.Parameters.Add(new SQLiteParameter("@meetingId"));

            deletePatientSt = new SQLiteCommand("delete from PATIENT where ID = @patientId;", connection);
            deletePatientSt.Parameters.Add(new SQLiteParameter("@patientId"));

            deletePointSt = new SQLiteCommand("delete from POINTS where ID = @pointId;", connection);
            deletePointSt.Parameters.Add(new SQLiteParameter("@pointId"));

            deleteChannelSt = new SQLiteCommand("delete from CHANNEL where ID = @channelId;", connection);
            deleteChannelSt.Parameters.Add(new SQLiteParameter("@channelId"));

            deleteSymptomPointRelationSt = new SQLiteCommand("delete from SYMPTOM_POINTS where SYMPTOM_ID = @symptomId and POINT_ID = @pointId;", connection);
            deleteSymptomPointRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@pointId") });

            deleteSymptomMeetingRelationSt = new SQLiteCommand("delete from MEETING_SYMPTOM where SYMPTOM_ID = @symptomId and MEETING_ID = @meetingId;", connection);
            deleteSymptomMeetingRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@meetingId") });

            deleteSymptomChannelRelationSt = new SQLiteCommand("delete from SYMPTOM_CHANNEL where SYMPTOM_ID = @symptomId and CHANNEL_ID = @channelId;", connection);
            deleteSymptomChannelRelationSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@symptomId"), new SQLiteParameter("@channelId") });

            deleteMeetingPointSt = new SQLiteCommand("delete from MEETING_POINTS where MEETING_ID = @meetingId and POINT_ID = @pointId;", connection);
            deleteMeetingPointSt.Parameters.AddRange(new SQLiteParameter[] { new SQLiteParameter("@meetingId"), new SQLiteParameter("@pointId") });

            getSymptomSt = new SQLiteCommand("select * from SYMPTOM where NAME = @name;", connection);
            getSymptomSt.Parameters.Add(new SQLiteParameter("@name"));

            getPointByNameSt = new SQLiteCommand("select * from POINTS where NAME = @name;", connection);
            getPointByNameSt.Parameters.Add(new SQLiteParameter("@name"));

            getPointByIdSt = new SQLiteCommand("select * from POINTS where ID = @id;", connection);
            getPointByIdSt.Parameters.Add(new SQLiteParameter("@id"));

            getChannelByIdSt = new SQLiteCommand("select * from CHANNEL where ID = @id;", connection);
            getChannelByIdSt.Parameters.Add(new SQLiteParameter("@id"));

            getAllMeetingsRelativeToPatientOrderByDateSt = new SQLiteCommand("SELECT * FROM MEETING WHERE PATIENT_ID=@patientId ORDER BY DATE DESC;", connection);
            getAllMeetingsRelativeToPatientOrderByDateSt.Parameters.Add(new SQLiteParameter("@patientId"));

            getAllDiagnosticByPatientSt = new SQLiteCommand("select * from DIAGNOSTIC where PATIENT_ID = @patientId order by CREATION_DATE DESC;", connection);
            getAllDiagnosticByPatientSt.Parameters.Add(new SQLiteParameter("@patientId"));

            getDiagnosticByMeetingSt = new SQLiteCommand("select * from DIAGNOSTIC where ID = @Id;", connection);
            getDiagnosticByMeetingSt.Parameters.Add(new SQLiteParameter("@Id"));

            getAllPointsSt = new SQLiteCommand("select * from POINTS;", connection);

            findSymptomSt = new SQLiteCommand(connection);

            findPatientSt = new SQLiteCommand(connection);

            findPointSt = new SQLiteCommand(connection);

            getTheLastMeetingSt = new SQLiteCommand("select * from MEETING where MEETING.PATIENT_ID = @patientId order by date desc limit 1;", connection);
            getTheLastMeetingSt.Parameters.Add(new SQLiteParameter("@patientId"));

            getAllMeetingsRelativeToSymptomsSt = new SQLiteCommand(connection);

            InitColors();
        }

        #region colors handler
        private void InitColors()
        {
            priorityColors = new Color[NUM_OF_PRIORITIES];
            try
            {
                using (BinaryReader reader = new BinaryReader(new FileStream(folder + SETTING_COLOR_FILE, FileMode.Open)))
                {
                    for (int i = 0; i < NUM_OF_PRIORITIES; i++)
                        priorityColors[i] = reader.ReadColor();
                }
            }
            catch (FileNotFoundException)
            {
                priorityColors[0] = new Color() { A = 255, R = 51, G = 0, B = 0 };
                priorityColors[1] = new Color() { A = 255, R = 102, G = 102, B = 0 };
                priorityColors[2] = new Color() { A = 255, R = 0, G = 102, B = 102 };
                priorityColors[3] = new Color() { A = 255, R = 51, G = 0, B = 102 };
                priorityColors[4] = new Color() { A = 255, R = 102, G = 0, B = 51 };
                priorityColors[5] = new Color() { A = 255, R = 0, G = 0, B = 102 };
                using (BinaryWriter writer = new BinaryWriter(new FileStream(folder + SETTING_COLOR_FILE, FileMode.Create)))
                {
                    for (int i = 0; i < NUM_OF_PRIORITIES; i++)
                    {
                        writer.Write(priorityColors[i]);
                    }
                }
            }
        }
        public Color GetLevel(int level)
        {
            return priorityColors[level];
        }

        public void SetLevel(int level, Color color)
        {
            priorityColors[level] = color;
            using (BinaryWriter writer = new BinaryWriter(new FileStream(folder + SETTING_COLOR_FILE, FileMode.OpenOrCreate)))
            {
                for (int i = 0; i < NUM_OF_PRIORITIES; i++)
                {
                    writer.Write(priorityColors[i]);
                }
            }
        }
        #endregion
        #region updates
        public void UpdateSymptom(Symptom symptom)
        {
            updateSymptomSt.Parameters["@name"].Value = symptom.Name;
            updateSymptomSt.Parameters["@comment"].Value = symptom.Comment;
            updateSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            updateSymptomSt.ExecuteNonQuery();
        }

        public void UpdateMeeting(Meeting meeting)
        {
            updateMeetingSt.Parameters["@patientId"].Value = meeting.PatientId;
            updateMeetingSt.Parameters["@purpose"].Value = meeting.Purpose;
            updateMeetingSt.Parameters["@date"].Value = meeting.Date;
            updateMeetingSt.Parameters["@description"].Value = meeting.Description;
            updateMeetingSt.Parameters["@summery"].Value = meeting.Summery;
            updateMeetingSt.Parameters["@resultDecription"].Value = meeting.ResultDescription;
            updateMeetingSt.Parameters["@resultValue"].Value = meeting.Result.Value;
            updateMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            updateMeetingSt.Parameters["@diagnosticId"].Value = meeting.DiagnosticId;
            updateMeetingSt.ExecuteNonQuery();
        }

        public void UpdatePatient(Patient patient)
        {
            updatePatientSt.Parameters["@name"].Value = patient.Name;
            updatePatientSt.Parameters["@telephone"].Value = patient.Telephone;
            updatePatientSt.Parameters["@cellphone"].Value = patient.Cellphone;
            updatePatientSt.Parameters["@birthday"].Value = patient.Birthday;
            updatePatientSt.Parameters["@gander"].Value = patient.Gend.Value;
            updatePatientSt.Parameters["@address"].Value = patient.Address;
            updatePatientSt.Parameters["@email"].Value = patient.Email;
            updatePatientSt.Parameters["@medicalDescription"].Value = patient.MedicalDescription;
            updatePatientSt.Parameters["@patientId"].Value = patient.Id;
            updatePatientSt.ExecuteNonQuery();
        }

        public void UpdatePoint(Point point)
        {
            updatePointSt.Parameters["@name"].Value = point.Name;
            updatePointSt.Parameters["@minNeedleDepth"].Value = point.MinNeedleDepth;
            updatePointSt.Parameters["@maxNeedleDepth"].Value = point.MaxNeedleDepth;
            updatePointSt.Parameters["@position"].Value = point.Position;
            updatePointSt.Parameters["@importance"].Value = point.Importance;
            updatePointSt.Parameters["@comment1"].Value = point.Comment1;
            updatePointSt.Parameters["@comment2"].Value = point.Comment2;
            updatePointSt.Parameters["@note"].Value = point.Note;
            updatePointSt.Parameters["@image"].Value = point.Image;
            updatePointSt.Parameters["@pointId"].Value = point.Id;
            updatePointSt.ExecuteNonQuery();
        }

        public void UpdateChannel(Channel channel)
        {
            updateChannelSt.Parameters["@name"].Value = channel.Name;
            updateChannelSt.Parameters["@rt"].Value = channel.Rt;
            updateChannelSt.Parameters["@mainPoint"].Value = channel.MainPoint;
            updateChannelSt.Parameters["@evenPoint"].Value = channel.EvenPoint;
            updateChannelSt.Parameters["@path"].Value = channel.Path;
            updateChannelSt.Parameters["@role"].Value = channel.Role;
            updateChannelSt.Parameters["@comments"].Value = channel.Comments;
            updateChannelSt.Parameters["@channelId"].Value = channel.Id;
            updateChannelSt.ExecuteNonQuery();
        }

        public void UpdateChannelSymptomRelation(Channel channel, Symptom symptom, int importance, string comment)
        {
            updateChannelSymptomRelationSt.Parameters["@importance"].Value = importance;
            updateChannelSymptomRelationSt.Parameters["@comment"].Value = comment;
            updateChannelSymptomRelationSt.Parameters["@channelId"].Value = channel.Id;
            updateChannelSymptomRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            updateChannelSymptomRelationSt.ExecuteNonQuery();
        }

        public void UpdatePointSymptomRelation(Point point, Symptom symptom, int importance, string comment)
        {
            updatePointSymptomRelationSt.Parameters["@importance"].Value = importance;
            updatePointSymptomRelationSt.Parameters["@comment"].Value = comment;
            updatePointSymptomRelationSt.Parameters["@pointId"].Value = point.Id;
            updatePointSymptomRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            updatePointSymptomRelationSt.ExecuteNonQuery();
        }

        private void UpdateDiagnostic(Diagnostic diagnostic)
        {
            updateDiagnosticSt.Parameters["@Id"].Value = diagnostic.Id;
            updateDiagnosticSt.Parameters["@Profession"].Value = diagnostic.Profession;
            updateDiagnosticSt.Parameters["@MainComplaint"].Value = diagnostic.MainComplaint;
            updateDiagnosticSt.Parameters["@SeconderyComlaint"].Value = diagnostic.SeconderyComlaint;
            updateDiagnosticSt.Parameters["@DrugsUsed"].Value = diagnostic.DrugsUsed;
            updateDiagnosticSt.Parameters["@TestsMade"].Value = diagnostic.TestsMade;
            updateDiagnosticSt.Parameters["@IsPain"].Value = diagnostic.Pain.Value;
            updateDiagnosticSt.Parameters["@PainInfo"].Value = diagnostic.Pain.Info;
            updateDiagnosticSt.Parameters["@IsPainPreviousEvaluations"].Value = diagnostic.PainPreviousEvaluations.Value;
            updateDiagnosticSt.Parameters["@PainPreviousEvaluationsInfo"].Value = diagnostic.PainPreviousEvaluations.Info;
            updateDiagnosticSt.Parameters["@IsScans"].Value = diagnostic.Scans.Value;
            updateDiagnosticSt.Parameters["@ScansInfo"].Value = diagnostic.Scans.Info;
            updateDiagnosticSt.Parameters["@IsUnderStress"].Value = diagnostic.UnderStress.Value;
            updateDiagnosticSt.Parameters["@UnderStressInfo"].Value = diagnostic.UnderStress.Info;
            updateDiagnosticSt.Parameters["@IsTenseMuscles"].Value = diagnostic.TenseMuscles.Value;
            updateDiagnosticSt.Parameters["@TenseMusclesInfo"].Value = diagnostic.TenseMuscles.Info;
            updateDiagnosticSt.Parameters["@IsHighBloodPressureOrColesterol"].Value = diagnostic.HighBloodPressureOrColesterol.Value;
            updateDiagnosticSt.Parameters["@HighBloodPressureOrColesterolInfo"].Value = diagnostic.HighBloodPressureOrColesterol.Info;
            updateDiagnosticSt.Parameters["@IsGoodSleep"].Value = diagnostic.GoodSleep.Value;
            updateDiagnosticSt.Parameters["@GoodSleepInfo"].Value = diagnostic.GoodSleep.Info;
            updateDiagnosticSt.Parameters["@IsFallenToSleepProblem"].Value = diagnostic.FallenToSleepProblem.Value;
            updateDiagnosticSt.Parameters["@FallenToSleepProblemInfo"].Value = diagnostic.FallenToSleepProblem.Info;
            updateDiagnosticSt.Parameters["@IsPalpitations"].Value = diagnostic.Palpitations.Value;
            updateDiagnosticSt.Parameters["@PalpitationsInfo"].Value = diagnostic.Palpitations.Info;
            updateDiagnosticSt.Parameters["@DefeationRegularity"].Value = diagnostic.DefeationRegularity;
            updateDiagnosticSt.Parameters["@IsFatigueOrFeelsFulAfterEating"].Value = diagnostic.FatigueOrFeelsFulAfterEating.Value;
            updateDiagnosticSt.Parameters["@FatigueOrFeelsFulAfterEatingInfo"].Value = diagnostic.FatigueOrFeelsFulAfterEating.Info;
            updateDiagnosticSt.Parameters["@IsDesireForSweetsAfterEating"].Value = diagnostic.DesireForSweetsAfterEating.Value;
            updateDiagnosticSt.Parameters["@DesireForSweetsAfterEatingInfo"].Value = diagnostic.DesireForSweetsAfterEating.Info;
            updateDiagnosticSt.Parameters["@IsDifficultyConcentating"].Value = diagnostic.DifficultyConcentating.Value;
            updateDiagnosticSt.Parameters["@DifficultyConcentatingInfo"].Value = diagnostic.DifficultyConcentating.Info;
            updateDiagnosticSt.Parameters["@IsOftenIll"].Value = diagnostic.OftenIll.Value;
            updateDiagnosticSt.Parameters["@OftenIllInfo"].Value = diagnostic.OftenIll.Info;
            updateDiagnosticSt.Parameters["@IsSufferingFromMucus"].Value = diagnostic.SufferingFromMucus.Value;
            updateDiagnosticSt.Parameters["@SufferingFromMucusInfo"].Value = diagnostic.SufferingFromMucus.Info;
            updateDiagnosticSt.Parameters["@IsCoughOrAllergySuffers"].Value = diagnostic.CoughOrAllergySuffers.Value;
            updateDiagnosticSt.Parameters["@CoughOrAllergySuffersInfo"].Value = diagnostic.CoughOrAllergySuffers.Info;
            updateDiagnosticSt.Parameters["@IsSmoking"].Value = diagnostic.Smoking.Value;
            updateDiagnosticSt.Parameters["@SmokingInfo"].Value = diagnostic.Smoking.Info;
            updateDiagnosticSt.Parameters["@IsFrequentOrUrgentUrination"].Value = diagnostic.FrequentOrUrgentUrination.Value;
            updateDiagnosticSt.Parameters["@FrequentOrUrgentUrinationInfo"].Value = diagnostic.FrequentOrUrgentUrination.Info;
            updateDiagnosticSt.Parameters["@PreferColdOrHot"].Value = diagnostic.PreferColdOrHot;
            updateDiagnosticSt.Parameters["@PreferColdOrHotInfo"].Value = diagnostic.PreferColdOrHot.Info;
            updateDiagnosticSt.Parameters["@IsSuffersFromColdOrHot"].Value = diagnostic.SuffersFromColdOrHot.Value;
            updateDiagnosticSt.Parameters["@SuffersFromColdOrHotInfo"].Value = diagnostic.SuffersFromColdOrHot.Info;
            updateDiagnosticSt.Parameters["@IsSatisfiedDients"].Value = diagnostic.SatisfiedDients.Value;
            updateDiagnosticSt.Parameters["@SatisfiedDientsInfo"].Value = diagnostic.SatisfiedDients.Info;
            updateDiagnosticSt.Parameters["@IsWantToLostWeight"].Value = diagnostic.WantToLostWeight.Value;
            updateDiagnosticSt.Parameters["@WantToLostWeightInfo"].Value = diagnostic.WantToLostWeight.Info;
            updateDiagnosticSt.Parameters["@IsUsingContraception"].Value = diagnostic.UsingContraception.Value;
            updateDiagnosticSt.Parameters["@UsingContraceptionInfo"].Value = diagnostic.UsingContraception.Info;
            updateDiagnosticSt.Parameters["@IsCycleRegular"].Value = diagnostic.CycleRegular.Value;
            updateDiagnosticSt.Parameters["@CycleRegularInfo"].Value = diagnostic.CycleRegular.Info;
            updateDiagnosticSt.Parameters["@IsSufferingFromCrampsOrNervousBeforeMenstruation"].Value = diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation.Value;
            updateDiagnosticSt.Parameters["@SufferingFromCrampsOrNervousBeforeMenstruationInfo"].Value = diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation.Info;
            updateDiagnosticSt.Parameters["@IsSufferingFromMenpause"].Value = diagnostic.SufferingFromMenpause.Value;
            updateDiagnosticSt.Parameters["@SufferingFromMenpauseInfo"].Value = diagnostic.SufferingFromMenpause.Info;
            updateDiagnosticSt.Parameters["@HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife"].Value = diagnostic.HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
            updateDiagnosticSt.Parameters["@WhatChangesDoYouExpectToSeeFromTreatment"].Value = diagnostic.WhatChangesDoYouExpectToSeeFromTreatment;
            updateDiagnosticSt.Parameters["@patientId"].Value = diagnostic.PatientId;
            updateDiagnosticSt.Parameters["@creationDate"].Value = diagnostic.CreationDate;
            updateDiagnosticSt.ExecuteNonQuery();
        }
        #endregion
        #region deletes
        public void DeleteSymptom(Symptom symptom)
        {
            deleteSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            deleteSymptomSt.ExecuteNonQuery();
        }

        public void DeleteMeeting(Meeting meeting)
        {
            deleteMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            deleteMeetingSt.ExecuteNonQuery();
        }

        public void DeletePatient(Patient patient)
        {
            deletePatientSt.Parameters["@patientId"].Value = patient.Id;
            deletePatientSt.ExecuteNonQuery();
        }

        public void DeletePoint(Point point)
        {
            deletePointSt.Parameters["@pointId"].Value = point.Id;
            deletePointSt.ExecuteNonQuery();
        }

        public void DeleteChannel(Channel channel)
        {
            deleteChannelSt.Parameters["@channelId"].Value = channel.Id;
            deleteChannelSt.ExecuteNonQuery();
        }

        public void DeleteSymptomPointRelation(Symptom symptom, Point point)
        {
            deleteSymptomPointRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            deleteSymptomPointRelationSt.Parameters["@pointId"].Value = point.Id;
            deleteSymptomPointRelationSt.ExecuteNonQuery();
        }

        public void DeleteSymptomMeetingRelation(Symptom symptom, Meeting meeting)
        {
            deleteSymptomMeetingRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            deleteSymptomMeetingRelationSt.Parameters["@meetingId"].Value = meeting.Id;
            deleteSymptomMeetingRelationSt.ExecuteNonQuery();
        }

        public void DeleteSymptomChannelRelation(Symptom symptom, Channel channel)
        {
            deleteSymptomChannelRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            deleteSymptomChannelRelationSt.Parameters["@channelId"].Value = channel.Id;
            deleteSymptomChannelRelationSt.ExecuteNonQuery();
        }

        public void DeleteMeetingPoint(Meeting meeting, Point point)
        {
            deleteMeetingPointSt.Parameters["@meetingId"].Value = meeting.Id;
            deleteMeetingPointSt.Parameters["@pointId"].Value = point.Id;
            deleteMeetingPointSt.ExecuteNonQuery();
        }
        #endregion
        #region finds objects
        public List<Diagnostic> GetAllDiagnosticByPatient(Patient patient)
        {
            getAllDiagnosticByPatientSt.Parameters["@patientId"].Value = patient.Id;
            List<Diagnostic> o = new List<Diagnostic>();
            using (SQLiteDataReader rs = getAllDiagnosticByPatientSt.ExecuteReader())
            {
                while(rs.Read())
                {
                    o.Add(GetDiagnostic(rs));
                }
            }
            return o;
        }
        public Diagnostic GetDiagnosticByMeeting(Meeting meeting)
        {
            getDiagnosticByMeetingSt.Parameters["@diagnosticId"].Value = meeting.DiagnosticId;
            using (SQLiteDataReader rs = getDiagnosticByMeetingSt.ExecuteReader())
            {
                if(rs.Read())
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
            getAllMeetingsRelativeToPatientOrderByDateSt.Parameters["@patientId"].Value = patient.Id;
            using (SQLiteDataReader rs = getAllMeetingsRelativeToPatientOrderByDateSt.ExecuteReader())
            {
                return GetMeetings(rs);
            }
        }
        public Channel GetChannel(int id)
        {
            getChannelByIdSt.Parameters["@id"].Value = id;
            using (SQLiteDataReader rs = getChannelByIdSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetChannel(rs);
                return null;
            }
        }

        public Point GetPoint(int id)
        {
            getPointByIdSt.Parameters["@id"].Value = id;
            using (SQLiteDataReader rs = getPointByIdSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetPoint(rs);
                return null;
            }
        }

        public List<Point> GetAllPoints()
        {
            using (SQLiteDataReader rs = getAllPointsSt.ExecuteReader())
            {
                return GetPoints(rs);
            }
        }

        public Point GetPoint(string name)
        {
            getPointByNameSt.Parameters["@name"].Value = name;
            using (SQLiteDataReader rs = getPointByNameSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetPoint(rs);
                return null;
            }
        }

        public Symptom GetSymptom(string name)
        {
            getSymptomSt.Parameters["@name"].Value = name;
            using (SQLiteDataReader rs = getSymptomSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetSymptom(rs);
                return null;
            }
        }

        public List<Symptom> FindSymptom(string name)
        {
            findSymptomSt.CommandText = "SELECT * FROM SYMPTOM where NAME like '%" + name + "%';";
            using (SQLiteDataReader rs = findSymptomSt.ExecuteReader())
            {
                return GetSymptoms(rs);
            }
        }

        public List<Point> FindPoint(string name)
        {
            findPointSt.CommandText = "SELECT * FROM POINTS where NAME like '%" + name + "%';";
            using (SQLiteDataReader rs = findPointSt.ExecuteReader())
            {
                return GetPoints(rs);
            }
        }


        public List<Patient> FindPatient(string name)
        {
            findPatientSt.CommandText = "SELECT * FROM PATIENT where NAME like '%" + name + "%';";
            using (SQLiteDataReader rs = findPatientSt.ExecuteReader())
            {
                return GetPatients(rs);
            }
        }

        public List<ConnectionValue<Channel>> GetAllChannelRelativeToSymptom(Symptom symptom)
        {
            getAllChannelRelativeToSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            using (SQLiteDataReader rs = getAllChannelRelativeToSymptomSt.ExecuteReader())
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
            getAllPointRelativeToSymptomSt.Parameters["@symptomId"].Value = symptom.Id;
            using (SQLiteDataReader rs = getAllPointRelativeToSymptomSt.ExecuteReader())
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
            getAllSymptomRelativeToPointSt.Parameters["@pointId"].Value = point.Id;
            using (SQLiteDataReader rs = getAllSymptomRelativeToPointSt.ExecuteReader())
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
            getAllMeetingsRelativeToSymptomsSt.CommandText = sql;
            using (SQLiteDataReader rs = getAllMeetingsRelativeToSymptomsSt.ExecuteReader())
            {
                return GetMeetings(rs);
            }
        }
        public List<Symptom> GetAllSymptomRelativeToMeeting(Meeting meeting)
        {
            getAllSymptomRelativeToMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            using (SQLiteDataReader rs = getAllSymptomRelativeToMeetingSt.ExecuteReader())
                return GetSymptoms(rs);
        }

        public Patient GetPatientRelativeToMeeting(Meeting meeting)
        {
            getPatientRelativeToMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            using (SQLiteDataReader rs = getPatientRelativeToMeetingSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetPatient(rs);
                else
                    return null;
            }
        }

        public List<Point> GetAllPointsRelativeToMeeting(Meeting meeting)
        {
            getAllPointRelativeToMeetingSt.Parameters["@meetingId"].Value = meeting.Id;
            using (SQLiteDataReader rs = getAllPointRelativeToMeetingSt.ExecuteReader())
                return GetPoints(rs);
        }

        public Meeting GetTheLastMeeting(Patient patient)
        {
            getTheLastMeetingSt.Parameters["@patientId"].Value = patient.Id;
            using (SQLiteDataReader rs = getTheLastMeetingSt.ExecuteReader())
            {
                if (rs.Read())
                    return GetMeeting(rs);
                else
                    return null;
                
            }
        }
        #endregion
        #region inserts
        public void SetDiagnostic(Diagnostic diagnostic)
        {
            if(diagnostic.Id == -1)
            {
                InsertDiagnostic(diagnostic);
            }
            else
            {
                UpdateDiagnostic(diagnostic);
            }
        }
        public Channel InsertChannel(Channel channel)
        {
            insertChannelSt.Parameters["@channelId"].Value = channel.Id;
            insertChannelSt.Parameters["@name"].Value = channel.Name;
            insertChannelSt.Parameters["@rt"].Value = channel.Rt;
            insertChannelSt.Parameters["@aminPoint"].Value = channel.MainPoint;
            insertChannelSt.Parameters["@evenPoint"].Value = channel.EvenPoint;
            insertChannelSt.Parameters["@path"].Value = channel.Path;
            insertChannelSt.Parameters["@role"].Value = channel.Role;
            insertChannelSt.Parameters["@comments"].Value = channel.Comments;
            insertChannelSt.ExecuteNonQuery();
            return channel;
        }

        private Diagnostic InsertDiagnostic(Diagnostic diagnostic)
        {
            insertDiagnosticSt.Parameters["@Id"].Value = diagnostic.Id;
            insertDiagnosticSt.Parameters["@Profession"].Value = diagnostic.Profession;
            insertDiagnosticSt.Parameters["@MainComplaint"].Value = diagnostic.MainComplaint;
            insertDiagnosticSt.Parameters["@SeconderyComlaint"].Value = diagnostic.SeconderyComlaint;
            insertDiagnosticSt.Parameters["@DrugsUsed"].Value = diagnostic.DrugsUsed;
            insertDiagnosticSt.Parameters["@TestsMade"].Value = diagnostic.TestsMade;
            insertDiagnosticSt.Parameters["@IsPain"].Value = diagnostic.Pain.Value;
            insertDiagnosticSt.Parameters["@PainInfo"].Value = diagnostic.Pain.Info;
            insertDiagnosticSt.Parameters["@IsPainPreviousEvaluations"].Value = diagnostic.PainPreviousEvaluations.Value;
            insertDiagnosticSt.Parameters["@PainPreviousEvaluationsInfo"].Value = diagnostic.PainPreviousEvaluations.Info;
            insertDiagnosticSt.Parameters["@IsScans"].Value = diagnostic.Scans.Value;
            insertDiagnosticSt.Parameters["@ScansInfo"].Value = diagnostic.Scans.Info;
            insertDiagnosticSt.Parameters["@IsUnderStress"].Value = diagnostic.UnderStress.Value;
            insertDiagnosticSt.Parameters["@UnderStressInfo"].Value = diagnostic.UnderStress.Info;
            insertDiagnosticSt.Parameters["@IsTenseMuscles"].Value = diagnostic.TenseMuscles.Value;
            insertDiagnosticSt.Parameters["@TenseMusclesInfo"].Value = diagnostic.TenseMuscles.Info;
            insertDiagnosticSt.Parameters["@IsHighBloodPressureOrColesterol"].Value = diagnostic.HighBloodPressureOrColesterol.Value;
            insertDiagnosticSt.Parameters["@HighBloodPressureOrColesterolInfo"].Value = diagnostic.HighBloodPressureOrColesterol.Info;
            insertDiagnosticSt.Parameters["@IsGoodSleep"].Value = diagnostic.GoodSleep.Value;
            insertDiagnosticSt.Parameters["@GoodSleepInfo"].Value = diagnostic.GoodSleep.Info;
            insertDiagnosticSt.Parameters["@IsFallenToSleepProblem"].Value = diagnostic.FallenToSleepProblem.Value;
            insertDiagnosticSt.Parameters["@FallenToSleepProblemInfo"].Value = diagnostic.FallenToSleepProblem.Info;
            insertDiagnosticSt.Parameters["@IsPalpitations"].Value = diagnostic.Palpitations.Value;
            insertDiagnosticSt.Parameters["@PalpitationsInfo"].Value = diagnostic.Palpitations.Info;
            insertDiagnosticSt.Parameters["@DefeationRegularity"].Value = diagnostic.DefeationRegularity;
            insertDiagnosticSt.Parameters["@IsFatigueOrFeelsFulAfterEating"].Value = diagnostic.FatigueOrFeelsFulAfterEating.Value;
            insertDiagnosticSt.Parameters["@FatigueOrFeelsFulAfterEatingInfo"].Value = diagnostic.FatigueOrFeelsFulAfterEating.Info;
            insertDiagnosticSt.Parameters["@IsDesireForSweetsAfterEating"].Value = diagnostic.DesireForSweetsAfterEating.Value;
            insertDiagnosticSt.Parameters["@DesireForSweetsAfterEatingInfo"].Value = diagnostic.DesireForSweetsAfterEating.Info;
            insertDiagnosticSt.Parameters["@IsDifficultyConcentating"].Value = diagnostic.DifficultyConcentating.Value;
            insertDiagnosticSt.Parameters["@DifficultyConcentatingInfo"].Value = diagnostic.DifficultyConcentating.Info;
            insertDiagnosticSt.Parameters["@IsOftenIll"].Value = diagnostic.OftenIll.Value;
            insertDiagnosticSt.Parameters["@OftenIllInfo"].Value = diagnostic.OftenIll.Info;
            insertDiagnosticSt.Parameters["@IsSufferingFromMucus"].Value = diagnostic.SufferingFromMucus.Value;
            insertDiagnosticSt.Parameters["@SufferingFromMucusInfo"].Value = diagnostic.SufferingFromMucus.Info;
            insertDiagnosticSt.Parameters["@IsCoughOrAllergySuffers"].Value = diagnostic.CoughOrAllergySuffers.Value;
            insertDiagnosticSt.Parameters["@CoughOrAllergySuffersInfo"].Value = diagnostic.CoughOrAllergySuffers.Info;
            insertDiagnosticSt.Parameters["@IsSmoking"].Value = diagnostic.Smoking.Value;
            insertDiagnosticSt.Parameters["@SmokingInfo"].Value = diagnostic.Smoking.Info;
            insertDiagnosticSt.Parameters["@IsFrequentOrUrgentUrination"].Value = diagnostic.FrequentOrUrgentUrination.Value;
            insertDiagnosticSt.Parameters["@FrequentOrUrgentUrinationInfo"].Value = diagnostic.FrequentOrUrgentUrination.Info;
            insertDiagnosticSt.Parameters["@PreferColdOrHot"].Value = diagnostic.PreferColdOrHot;
            insertDiagnosticSt.Parameters["@PreferColdOrHotInfo"].Value = diagnostic.PreferColdOrHot.Info;
            insertDiagnosticSt.Parameters["@IsSuffersFromColdOrHot"].Value = diagnostic.SuffersFromColdOrHot.Value;
            insertDiagnosticSt.Parameters["@SuffersFromColdOrHotInfo"].Value = diagnostic.SuffersFromColdOrHot.Info;
            insertDiagnosticSt.Parameters["@IsSatisfiedDients"].Value = diagnostic.SatisfiedDients.Value;
            insertDiagnosticSt.Parameters["@SatisfiedDientsInfo"].Value = diagnostic.SatisfiedDients.Info;
            insertDiagnosticSt.Parameters["@IsWantToLostWeight"].Value = diagnostic.WantToLostWeight.Value;
            insertDiagnosticSt.Parameters["@WantToLostWeightInfo"].Value = diagnostic.WantToLostWeight.Info;
            insertDiagnosticSt.Parameters["@IsUsingContraception"].Value = diagnostic.UsingContraception.Value;
            insertDiagnosticSt.Parameters["@UsingContraceptionInfo"].Value = diagnostic.UsingContraception.Info;
            insertDiagnosticSt.Parameters["@IsCycleRegular"].Value = diagnostic.CycleRegular.Value;
            insertDiagnosticSt.Parameters["@CycleRegularInfo"].Value = diagnostic.CycleRegular.Info;
            insertDiagnosticSt.Parameters["@IsSufferingFromCrampsOrNervousBeforeMenstruation"].Value = diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation.Value;
            insertDiagnosticSt.Parameters["@SufferingFromCrampsOrNervousBeforeMenstruationInfo"].Value = diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation.Info;
            insertDiagnosticSt.Parameters["@IsSufferingFromMenpause"].Value = diagnostic.SufferingFromMenpause.Value;
            insertDiagnosticSt.Parameters["@SufferingFromMenpauseInfo"].Value = diagnostic.SufferingFromMenpause.Info;
            insertDiagnosticSt.Parameters["@HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife"].Value = diagnostic.HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife;
            insertDiagnosticSt.Parameters["@WhatChangesDoYouExpectToSeeFromTreatment"].Value = diagnostic.WhatChangesDoYouExpectToSeeFromTreatment;
            insertDiagnosticSt.Parameters["@patientId"].Value = diagnostic.PatientId;
            insertDiagnosticSt.Parameters["@creationDate"].Value = diagnostic.CreationDate;
            insertDiagnosticSt.ExecuteNonQuery();

            long rowId = connection.LastInsertRowId;

            if (rowId != 0)
            {
                int id = (int)new SQLiteCommand("select ID from DIAGNOSTIC where rowId = " + rowId, connection).ExecuteScalar();
                return new Diagnostic(id, diagnostic);
            }
            throw new Exception("ERORR:Insert didn't accure");
        }

        public void InsertSymptomChannelRelation(Symptom symptom, Channel channel, int importance, string comment)
        {
            insertSymptomChannelRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            insertSymptomChannelRelationSt.Parameters["@channelId"].Value = channel.Id;
            insertSymptomChannelRelationSt.Parameters["@importance"].Value = importance;
            insertSymptomChannelRelationSt.Parameters["@comment"].Value = comment;
            insertSymptomChannelRelationSt.ExecuteNonQuery();
        }

        public void InsertMeetingPointRelation(Meeting meeting, Point point)
        {
            insertMeetingPointRelationSt.Parameters["@meetingId"].Value = meeting.Id;
            insertMeetingPointRelationSt.Parameters["@pointId"].Value = point.Id;
            insertMeetingPointRelationSt.ExecuteNonQuery();
        }

        public void InsertSymptomPointRelation(Symptom symptom, Point point, int importance, string comment)
        {
            insertSymptomPointRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            insertSymptomPointRelationSt.Parameters["@pointId"].Value = point.Id;
            insertSymptomPointRelationSt.Parameters["@importance"].Value = importance;
            insertSymptomPointRelationSt.Parameters["@comment"].Value = comment;
            insertSymptomPointRelationSt.ExecuteNonQuery();
        }

        public void InsertSymptomMeetingRelation(Symptom symptom, Meeting meeting)
        {
            insertSymptomMeetingRelationSt.Parameters["@meetingId"].Value = meeting.Id;
            insertSymptomMeetingRelationSt.Parameters["@symptomId"].Value = symptom.Id;
            try {
                insertSymptomMeetingRelationSt.ExecuteNonQuery();
            }catch(SQLiteException e)
            {
                if(e.ErrorCode == (int)SQLiteErrorCode.Constraint)
                {
                    throw new Exceptions.UniqueNameException();
                }
                throw e;
            }
        }

        public Symptom InsertSymptom(Symptom symptom)
        {
            insertSymptomSt.Parameters["@name"].Value = symptom.Name;
            insertSymptomSt.Parameters["@comment"].Value = symptom.Comment;
            insertSymptomSt.ExecuteNonQuery();

            long rowId = connection.LastInsertRowId;

            if (rowId != 0)
            {

                int id = (int)new SQLiteCommand("select SYMPTOM_ID from SYMPTOM where rowId = " + rowId, connection).ExecuteScalar();
                return new Symptom(id, symptom);
            }
            throw new Exception("ERORR:Insert didn't accure");
        }

        public Meeting InsertMeeting(Meeting meeting)
        {
            insertMeetingSt.Parameters["@patientId"].Value = meeting.PatientId;
            insertMeetingSt.Parameters["@purpose"].Value = meeting.Purpose;
            insertMeetingSt.Parameters["@date"].Value = meeting.Date;
            insertMeetingSt.Parameters["@description"].Value = meeting.Description;
            insertMeetingSt.Parameters["@summery"].Value = meeting.Summery;
            insertMeetingSt.Parameters["@resultDescription"].Value = meeting.ResultDescription;
            insertMeetingSt.Parameters["@resultValue"].Value = meeting.Result.Value;
            insertMeetingSt.Parameters["@diagnosticId"].Value = meeting.DiagnosticId;
            insertMeetingSt.ExecuteNonQuery();

            long rowId = connection.LastInsertRowId;

            if (rowId != 0)
            {
                int id = (int)(long)new SQLiteCommand("select ID from MEETING where rowId = " + rowId, connection).ExecuteScalar();
                return new Meeting(id, meeting);
            }
            throw new Exception("ERORR:insert meeting didn't accure");
        }

        public Patient InsertPatient(Patient patient)
        {
            try
            {
                insertPatientSt.Parameters["@name"].Value = patient.Name;
                insertPatientSt.Parameters["@telephone"].Value = patient.Telephone;
                insertPatientSt.Parameters["@cellphone"].Value = patient.Cellphone;
                insertPatientSt.Parameters["@birthday"].Value = patient.Birthday;
                insertPatientSt.Parameters["@gender"].Value = patient.Gend.Value;
                insertPatientSt.Parameters["@address"].Value = patient.Address;
                insertPatientSt.Parameters["@email"].Value = patient.Email;
                insertPatientSt.Parameters["@medicalDescription"].Value = patient.MedicalDescription;
                insertPatientSt.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                if (e.ErrorCode == (int)SQLiteErrorCode.Constraint)
                    throw new Exceptions.UniqueNameException();
                else
                    throw e;
            }
            long rowId = connection.LastInsertRowId;
            if (rowId != 0)
            {
                int id = (int)(long)new SQLiteCommand("select ID from PATIENT where rowId = " + rowId + ";", connection).ExecuteScalar();
                return new Patient(id, patient);
            }
            throw new Exception("ERORR:insert patient didn't accure");
        }

        public Point InsertPoint(Point point)
        {
            insertPointsSt.Parameters["@name"].Value = point.Name;
            insertPointsSt.Parameters["@minNeedleDepth"].Value = point.MinNeedleDepth;
            insertPointsSt.Parameters["@maxNeedleDepth"].Value = point.MaxNeedleDepth;
            insertPointsSt.Parameters["@position"].Value = point.Position;
            insertPointsSt.Parameters["@importance"].Value = point.Importance;
            insertPointsSt.Parameters["@comment1"].Value = point.Comment1;
            insertPointsSt.Parameters["@comment2"].Value = point.Comment2;
            insertPointsSt.Parameters["@note"].Value = point.Note;
            insertPointsSt.Parameters["@image"].Value = point.Image;
            insertPointsSt.ExecuteNonQuery();

            long rowId = connection.LastInsertRowId;
            if (rowId != 0)
            {
                int id = (int)new SQLiteCommand("select POINT_ID from POINTS where rowId = " + rowId, connection).ExecuteScalar();
                return new Point(id, point);
            }
            throw new Exception("ERORR:insert point did't accure");
        }
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
                Pain = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IN_PAIN), rs.GetStringL(Diagnostic.PAIN_INFO)),
                PainPreviousEvaluations = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_PAIN_PREVIOUS_EVALUATIONS), rs.GetStringL(Diagnostic.PAIN_PREVIOUS_EVALUATION_INFO)),
                Scans = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_THERE_ANYSORT_OF_SCANS), rs.GetStringL(Diagnostic.THE_SCANS_INFO)),
                UnderStress = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_UNDER_STRESS), rs.GetStringL(Diagnostic.STRESS_INFO)),
                TenseMuscles = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_TENSE_MUSCLES), rs.GetStringL(Diagnostic.TENSE_MUSCLES_INFO)),
                HighBloodPressureOrColesterol = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_HIGH_BLOOD_PRESSURE_OR_COLESTEROL), rs.GetStringL(Diagnostic.HIGH_BLOOD_PRESSURE_OR_COLESTEROL_INFO)),
                GoodSleep = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_GOOD_SLEEP), rs.GetStringL(Diagnostic.GOOD_SLEEP_INFO)),
                FallenToSleepProblem = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_FALLEN_TO_SLEEP_PROBLEM), rs.GetStringL(Diagnostic.FALLEN_TO_SLEEP_PROBLEM_INFO)),
                Palpitations = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_PALPITATIONS), rs.GetStringL(Diagnostic.PALPITATIONS_INFO)),
                DefeationRegularity = rs.GetStringL(Diagnostic.DEFECATION_REGULARITY),
                FatigueOrFeelsFulAfterEating = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_FATIGUE_OR_FEELS_FULL_AFTER_EATING), rs.GetStringL(Diagnostic.FATIGUE_OR_FEELS_FULL_AFTER_EATING_INFO)),
                DesireForSweetsAfterEating = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_DESIRE_FOR_SWEETS_AFTER_EATING), rs.GetStringL(Diagnostic.DESIRE_FOR_SWEETS_AFTER_EATING_INFO)),
                DifficultyConcentating = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_DIFFICULTY_CONCENTRATING), rs.GetStringL(Diagnostic.DIFFICULTY_CONCENTRATING_INFO)),
                OftenIll = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_OFTEN_ILL), rs.GetStringL(Diagnostic.OFTEN_ILL_INFO)),
                SufferingFromMucus = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_SUFFERING_FROM_MUCUS), rs.GetStringL(Diagnostic.SUFFERING_FROM_MUCUS_INFO)),
                CoughOrAllergySuffers = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_COUGH_OR_ALLERGY_SUFFERS), rs.GetStringL(Diagnostic.COUGH_OR_ALLERGY_SUFFERS_INFO)),
                Smoking = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_SMOKING), rs.GetStringL(Diagnostic.SMOKING_INFO)),
                FrequentOrUrgentUrination = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_FREQUENT_OR_URGENT_URINATION), rs.GetStringL(Diagnostic.FREQUENT_OR_URGENT_URINATION_INFO)),
                PreferColdOrHot = new ValueInfo<PreferColdOrHotType>((PreferColdOrHotType)rs.GetIntL(Diagnostic.PREFER_COLD_OR_HOT), rs.GetStringL(Diagnostic.PREFER_COLD_OR_HOT_INFO)),
                SuffersFromColdOrHot = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_SUFFERS_FROM_COLD_OR_HOT), rs.GetStringL(Diagnostic.SUFFERS_FROM_COLD_OR_HOT_INFO)),
                SatisfiedDients = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_SATISFIED_DIETS), rs.GetStringL(Diagnostic.SATISFIED_DIETS_INFO)),
                WantToLostWeight = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_WANT_TO_LOST_WEIGHT), rs.GetStringL(Diagnostic.WANT_TO_LOST_WEIGHT_INFO)),
                UsingContraception = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_USING_CONTRACEPTION), rs.GetStringL(Diagnostic.USING_CONTRACEPTION_INFO)),
                CycleRegular = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_CYCLE_REGULAR), rs.GetStringL(Diagnostic.CYCLE_REGULAR_INFO)),
                SufferingFromCrampsOrNervousBeforeMenstruation = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION), rs.GetStringL(Diagnostic.SUFFERING_FROM_CRAMPS_OR_NERVOUS_BEFORE_OR_DURING_MENSTRUATION_INFO)),
                SufferingFromMenpause = new ValueInfo<bool>(rs.GetBoolL(Diagnostic.IS_SUFFERING_FROM_MENOPAUSE), rs.GetStringL(Diagnostic.SUFFERING_FROM_MENOPAUSE_INFO)),
                HowManyHoursAWeekAreYouWillingToInvestToImproveTheQualtyOfLife = rs.GetStringL(Diagnostic.HOW_MANY_HOURS_A_WEEK_ARE_YOU_WILLING_TO_INVEST_TO_IMPROVE_THE_QUALITY_OF_LIFE),
                WhatChangesDoYouExpectToSeeFromTreatment = rs.GetStringL(Diagnostic.WHAT_CHANGES_DO_YOU_EXPECT_TO_SEE_FROM_TREATMENT),
                PatientId = rs.GetIntL(Diagnostic.PATIENT_ID),
                CreationDate = rs.GetDateTimeL(Diagnostic.CREATION_DATE)
            };
        }
        #endregion
    }
}
