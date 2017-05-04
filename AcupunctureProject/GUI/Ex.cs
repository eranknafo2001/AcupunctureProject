using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AcupunctureProject.Database;

namespace AcupunctureProject.GUI
{
	public static partial class Ex
	{
		public static List<T> ToList<T>(this ItemCollection i, Func<object, T> f)
		{
			var o = new List<T>();
			foreach (var k in i)
				o.Add(f(k));
			return o;
		}

		public static List<K> ToList<T, K>(this IEnumerable<T> i, Func<T, K> f)
		{
			var o = new List<K>();
			foreach (var k in i)
				o.Add(f(k));
			return o;
		}

		public static List<object> ToList(this ItemCollection i)
		{
			var o = new List<object>();
			foreach (var k in i)
				o.Add(k);
			return o;
		}

		public static List<T> ToList<T>(this ItemCollection i)
		{
			var o = new List<T>();
			foreach (var k in i)
				o.Add((T)k);
			return o;
		}

		//public static void SetIfNotNull(this Diagnostic diagnostic)
		//{
		//	if (diagnostic.Pain == null)
		//		diagnostic.Pain = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.PainPreviousEvaluations == null)
		//		diagnostic.PainPreviousEvaluations = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.Scans == null)
		//		diagnostic.Scans = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.UnderStress == null)
		//		diagnostic.UnderStress = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.TenseMuscles == null)
		//		diagnostic.TenseMuscles = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.HighBloodPressureOrColesterol == null)
		//		diagnostic.HighBloodPressureOrColesterol = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.GoodSleep == null)
		//		diagnostic.GoodSleep = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.FallenToSleepProblem == null)
		//		diagnostic.FallenToSleepProblem = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.Palpitations == null)
		//		diagnostic.Palpitations = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.FatigueOrFeelsFulAfterEating == null)
		//		diagnostic.FatigueOrFeelsFulAfterEating = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.DesireForSweetsAfterEating == null)
		//		diagnostic.DesireForSweetsAfterEating = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.DifficultyConcentating == null)
		//		diagnostic.DifficultyConcentating = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.OftenIll == null)
		//		diagnostic.OftenIll = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.SufferingFromMucus == null)
		//		diagnostic.SufferingFromMucus = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.CoughOrAllergySuffers == null)
		//		diagnostic.CoughOrAllergySuffers = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.Smoking == null)
		//		diagnostic.Smoking = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.FrequentOrUrgentUrination == null)
		//		diagnostic.FrequentOrUrgentUrination = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.PreferColdOrHot == null)
		//		diagnostic.PreferColdOrHot = new ValueInfo<PreferColdOrHotType?>(null, "");
		//	if (diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation == null)
		//		diagnostic.SufferingFromCrampsOrNervousBeforeMenstruation = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.SufferingFromMenpause == null)
		//		diagnostic.SufferingFromMenpause = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.SuffersFromColdOrHot == null)
		//		diagnostic.SuffersFromColdOrHot = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.SatisfiedDients == null)
		//		diagnostic.SatisfiedDients = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.CycleRegular == null)
		//		diagnostic.CycleRegular = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.UsingContraception == null)
		//		diagnostic.UsingContraception = new ValueInfo<bool?>(null, "");
		//	if (diagnostic.WantToLostWeight == null)
		//		diagnostic.WantToLostWeight = new ValueInfo<bool?>(null, "");
		//}
	}
}


namespace System
{
	public delegate void VoidFunc<in T>(T i1);
	public delegate void VoidFunc<in T, in T2>(T i1, T2 i2);
	public delegate void VoidFunc<in T, in T2, in T3>(T i1, T2 i2, T3 i3);
	public delegate void VoidFunc<in T, in T2, in T3, in T4>(T i1, T2 i2, T3 i3, T4 i4);
	public delegate void VoidFunc<in T, in T2, in T3, in T4, in T5>(T i1, T2 i2, T3 i3, T4 i4, T5 i5);
	public delegate void VoidFunc<in T, in T2, in T3, in T4, in T5, in T6>(T i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6);
	public delegate void VoidFunc<in T, in T2, in T3, in T4, in T5, in T6, in T7>(T i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7);
}