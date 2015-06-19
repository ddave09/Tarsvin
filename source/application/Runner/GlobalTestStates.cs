namespace Tarsvin.Runner
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	internal class Result
	{
		internal IndividualFeatureTestState itfs = null;
		internal List<IndividualTestState> its = null;
		internal Result()
		{
			itfs = new IndividualFeatureTestState();
			its = new List<IndividualTestState>();
		}
	}

	internal static class GlobalTestStates
	{
		internal static bool onlyOnce = true;
		internal static Dictionary<string, Result> ResultSet = new Dictionary<string, Result>();
		internal static Dictionary<string, ReRunCase> repeatBook = new Dictionary<string, ReRunCase>();
		static Object lockTestInc = new Object();
		static Object sceCount = new Object();
		private static int scenarioCount = 0;
		private static int failureCount = 0;
		private static int testsRun = 0;
		private static int reFailureCount = 0;
		private static int reTestsRun = 0;

		internal static long GrandStartTime
		{
			get;
			set;
		}

		internal static long GrandEndTime
		{
			get;
			set;
		}

		internal static TimeSpan GrandExecTime
		{
			get
			{
				return new TimeSpan(GrandEndTime - GrandStartTime);
			}
		}

		internal static void SetScenarioCount(int count)
		{
			lock (sceCount)
			{
				scenarioCount = count;
			}
		}

		internal static void IncrementScenarioCount()
		{
			Interlocked.Increment(ref GlobalTestStates.scenarioCount);
		}

		internal static void DecrementScenarioCount()
		{
			Interlocked.Decrement(ref GlobalTestStates.scenarioCount);
		}

		internal static void IncrementTestCount(int count)
		{
			lock (lockTestInc)
			{
				testsRun += count;
			}
		}

		internal static void IncrementFailure()
		{
			Interlocked.Increment(ref GlobalTestStates.failureCount);
		}

		internal static void IncrementReTestCount(int count)
		{
			lock (lockTestInc)
			{
				reTestsRun += count;
			}
		}

		internal static void IncrementReFailure()
		{
			Interlocked.Increment(ref GlobalTestStates.reFailureCount);
		}

		internal static int FailureCount
		{
			get
			{
				return GlobalTestStates.failureCount;
			}
		}

		internal static int ReFailureCount
		{
			get
			{
				return GlobalTestStates.reFailureCount;
			}
		}

		internal static int TestsRun
		{
			get
			{
				return GlobalTestStates.testsRun;
			}
		}

		internal static int ReTestsRun
		{
			get
			{
				return GlobalTestStates.reTestsRun;
			}
		}

		internal static int GetScenarioCount
		{
			get
			{
				return scenarioCount;
			}
		}
	}
}
