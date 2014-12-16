using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YetAnotherRunner
{
    public static class GlobalTestStates
    {
        public static HashSet<IndividualTestState> manageState = new HashSet<IndividualTestState>();
        public static HashSet<IndividualFeatureTestState> featureState = new HashSet<IndividualFeatureTestState>();
        static Object lockFeatureAdd = new Object();
        static Object lockScenarioAdd = new Object();
        private static int scenarioCount = 0;
        
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


        /// <summary>
        /// This function is not implemented yet!!!
        /// </summary>
        public static void remove()
        {

        }

        public static void removeAll()
        {
            manageState.Clear();
        }

        public static bool Find(IndividualTestState its)
        {
            if (!manageState.Contains(its))
            {
                return false;
            }
            return true;
        }
    }
}
