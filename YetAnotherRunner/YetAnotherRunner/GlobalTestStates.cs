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
        private static SpinLock lockHash = new SpinLock();
        private static SpinLock featureHasLock = new SpinLock();
        private static SpinLock lockCountRw = new SpinLock();
        private static int scenarioCount = 0;

        public static void AddFeature(IndividualFeatureTestState itfs)
        {
            bool isLock = false;
            featureHasLock.Enter(ref isLock);
            featureState.Add(itfs);
            if (isLock) featureHasLock.Exit();
        }

        //public static long StartTick
        //{
        //    get;
        //    set;
        //}

        //public static long EndTick
        //{
        //    get;
        //    set;
        //}

        //public static TimeSpan TotalExecP()
        //{
        //    return new TimeSpan(EndTick - StartTick);
        //}

        public static void IncrementScenarioCount()
        {
            bool isLock = false;
            lockCountRw.Enter(ref isLock);
            scenarioCount++;
            if(isLock) lockCountRw.Exit();
        }

        public static void DecrementScenarioCount()
        {
            bool isLock = false;
            lockCountRw.Enter(ref isLock);
            scenarioCount--;
            if(isLock) lockCountRw.Exit();
        }

        public static int GetScenarioCount
        {
            get
            {
                while (lockCountRw.IsHeld) ;
                return scenarioCount;
            }
        }

        public static void Add(IndividualTestState its)
        {
            bool isLock = false;
            lockHash.Enter(ref isLock);
            manageState.Add(its);
            if (isLock) lockHash.Exit();     
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
