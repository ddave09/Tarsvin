namespace Tarsvin.Runner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class GlobalTestStates
    {
        internal static bool onlyOnce = true;
        internal static HashSet<IndividualTestState> manageState = new HashSet<IndividualTestState>();
        internal static HashSet<IndividualFeatureTestState> featureState = new HashSet<IndividualFeatureTestState>();
        internal static Dictionary<string, IndividualFeatureTestState> featureBook = new Dictionary<string, IndividualFeatureTestState>();
        internal static Dictionary<string, ReRunCase> repeatBook = new Dictionary<string, ReRunCase>();
        internal static Dictionary<string, ReRunCase> repeatBookCopy = new Dictionary<string, ReRunCase>();
        static Object sceCount = new Object();
        static Object lockFeatureAdd = new Object();
        static Object lockScenarioAdd = new Object();
        static Object reRunCaseAdd = new Object();
        private static int scenarioCount = 0;

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

        internal static void AddFeature(IndividualFeatureTestState itfs)
        {
            lock (lockFeatureAdd)
            {
                featureState.Add(itfs);
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

        internal static int GetScenarioCount
        {
            get
            {
                return scenarioCount;
            }
        }

        internal static void Add(IndividualTestState its)
        {
            lock (lockScenarioAdd)
            {
                manageState.Add(its);
            }            
        }

    }
}
