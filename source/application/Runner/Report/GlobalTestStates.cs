namespace Tarsvin.Runner.Report
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

	internal class ReRunCase
	{
		internal Type type = null;
		internal List<MethodInfo> testMethods = null;
		internal string nameSpace = string.Empty;
		internal List<string> attrs = null;
		internal MethodInfo TearDownFeature = null;

		internal ReRunCase(Type type, List<MethodInfo> testMethods, string nameSpace, List<string> attrs,
			MethodInfo TearDownFeature)
		{
			this.type = type;
			this.testMethods = testMethods;
			this.nameSpace = nameSpace;
			this.attrs = attrs;
			this.TearDownFeature = TearDownFeature;
		}
	}

	internal static class GlobalTestStates
	{
		#region Variables
		internal static bool onlyOnce = true;
		internal static Dictionary<string, Result> resultSet = new Dictionary<string, Result>();
		internal static Dictionary<string, ReRunCase> repeatBook = new Dictionary<string, ReRunCase>();
		
		static Object lockTestInc = new Object();
		static Object sceCount = new Object();
		static Object lockSkipped = new Object();

		private static int notRun = 0;
		private static int inconclusive = 0;
		private static int ignored = 0;
		private static int skipped = 0;
		private static int invalid = 0;
		private static int error = 0;
		private static int reError = 0;
		private static int scenarioCount = 0;
		private static int failureCount = 0;
		private static int testsRun = 0;
		private static int reFailureCount = 0;
		private static int reTestsRun = 0;
		#endregion

		#region Properties
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

		internal static bool Success
		{
			get
			{
				if (FailureCount == 0)
				{
					return true;
				}
				else if (FailureCount != 0 && ReFailureCount == 0)
				{
					return true;
				}
				return false;
			}
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

		internal static int NotRun
		{
			get
			{
				return GlobalTestStates.notRun;
			}
		}

		internal static int Inconclusive
		{
			get
			{
				return GlobalTestStates.inconclusive;
			}
		}

		internal static int Ignored
		{
			get
			{
				return GlobalTestStates.ignored;
			}
		}

		internal static int Skipped
		{
			get
			{
				return GlobalTestStates.skipped;
			}
		}

		internal static int Invalid
		{
			get
			{
				return GlobalTestStates.invalid;
			}
		}

		internal static int Error
		{
			get
			{
				return GlobalTestStates.error;
			}
		}

		internal static int ReError
		{
			get
			{
				return GlobalTestStates.reError;
			}
		}
		#endregion

		#region Members
		internal static void IncrementNotRun()
		{
			Interlocked.Increment(ref GlobalTestStates.notRun);
		}

		internal static void IncrementInconclusive()
		{
			Interlocked.Increment(ref GlobalTestStates.inconclusive);
		}

		internal static void IncrementIgnored()
		{
			Interlocked.Increment(ref GlobalTestStates.ignored);
		}

		internal static void IncrementSkipped()
		{
			Interlocked.Increment(ref GlobalTestStates.skipped);
		}

		internal static void IncrementSkipped(int count)
		{
			lock (lockSkipped)
			{
				GlobalTestStates.skipped += count;
			}
		}

		internal static void IncrementInvalid()
		{
			Interlocked.Increment(ref GlobalTestStates.invalid);
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

		internal static void IncrementError()
		{
			Interlocked.Increment(ref GlobalTestStates.error);
		}

		internal static void IncrementReError()
		{
			Interlocked.Increment(ref GlobalTestStates.reError);
		}
		#endregion
	}
}
