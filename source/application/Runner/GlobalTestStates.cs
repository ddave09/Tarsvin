namespace Runner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public static class GlobalTestStates
    {
        public static HashSet<IndividualTestState> manageState = new HashSet<IndividualTestState>();
        public static HashSet<IndividualFeatureTestState> featureState = new HashSet<IndividualFeatureTestState>();
        public static List<ReRunCase> reRunList = new List<ReRunCase>();
        static Object lockFeatureAdd = new Object();
        static Object lockScenarioAdd = new Object();
        static Object reRunCaseAdd = new Object();
        private static int scenarioCount = 0;

        public static long GrandStartTime
        {
            get;
            set;
        }

        public static long GrandEndTime
        {
            get;
            set;
        }

        public static TimeSpan GrandExecTime
        {
            get
            {
                return new TimeSpan(GrandEndTime - GrandStartTime);
            }
        }

        public static void AddFeature(IndividualFeatureTestState itfs)
        {
            lock (lockFeatureAdd)
            {
                featureState.Add(itfs);
            }
        }

        public static void IncrementScenarioCount()
        {
            Interlocked.Increment(ref GlobalTestStates.scenarioCount);
        }

        public static void DecrementScenarioCount()
        {
            Interlocked.Decrement(ref GlobalTestStates.scenarioCount);            
        }

        public static int GetScenarioCount
        {
            get
            {
                return scenarioCount;
            }
        }

        public static void Add(IndividualTestState its)
        {
            lock (lockScenarioAdd)
            {
                manageState.Add(its);
            }            
        }

        public static void PushToReRunList(Object obj, MethodInfo testMethod, string nameSpace, List<string> attrs)
        {
            lock (reRunCaseAdd)
            {
                reRunList.Add(new ReRunCase(obj, testMethod, nameSpace, ref attrs));           
            }            
        }
    }
}
