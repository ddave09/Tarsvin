namespace Tarsvin.Runner
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Tarsvin.Runner.Interfaces;

    internal class Inter
    {
        internal Object obj;
        internal Type type;
        internal MethodInfo TearDownFeature;
        internal bool direct;
        internal enum MT : sbyte
        {
            Type,
            Method
        };

        internal Inter(Object obj, Type type, MT m, MethodInfo TearDownFeature)
        {
            this.obj = obj;
            this.type = type;
            this.TearDownFeature = TearDownFeature;
            this.direct = Convert.ToBoolean(m);
        }
    }

    internal class SequentialRunner : IRunner
    {
        public void Run(Object typeObject, MethodInfo testMethod, Type type, 
            List<string> attrs, BackgroundWorker bw = null, int count = 0, 
            MethodInfo TearDownFeature = null)
        {
            IndividualTestState its = new IndividualTestState();
            its.NameSpace = type.FullName;
            its.Attributes = attrs;
            its.TestName = testMethod.Name;
            its.StartTime = DateTime.Now.Ticks;
            try
            {
                testMethod.Invoke(typeObject, null);
                its.EndTime = DateTime.Now.Ticks;
                its.Result = true;
                its.ThrownException = null;
                Console.WriteLine("***\n\n\n{0}.{1}: Passed\n\n\n", 
                    its.NameSpace, its.TestName);
            }
            catch (Exception e)
            {
                its.EndTime = DateTime.Now.Ticks;
                its.Result = false;
                its.ThrownException = e;
                if (its.CatchTimeOut)
                {
                    if (GlobalTestStates.repeatBook.ContainsKey(type.FullName))
                    {
                        GlobalTestStates.repeatBook[type.FullName].testMethods.
                            Add(testMethod);
                    }
                    else
                    {
                        GlobalTestStates.repeatBook.Add(type.FullName, new ReRunCase(type,
                            new List<MethodInfo>() { testMethod }, type.FullName, attrs));
                    }
                }
                Console.WriteLine("***\n\n\n{0}.{1}: Failed\n\n\n", 
                    its.NameSpace, its.TestName);
            }
            GlobalTestStates.Add(its);
            if (GlobalTestStates.GetScenarioCount > 0)
            {
                GlobalTestStates.DecrementScenarioCount();
            }
        }
    }

    internal class ParallelRunner : IRunner
    {
        public void Run(Object typeObject, MethodInfo testMethod, Type type, 
            List<string> attrs, BackgroundWorker bw, int count = 0, 
            MethodInfo TearDownFeature = null)
        {
            Task finalContinuation = null;
            Task task = Task.Factory.StartNew((Object obj) =>
            {
                IndividualTestState its = obj as IndividualTestState;
                while (bw.IsBusy) ;
                bw.RunWorkerAsync(new Inter(typeObject, type, 
                    Inter.MT.Method,TearDownFeature));
                testMethod.Invoke(typeObject, null);
            },
            new IndividualTestState() { InvokeObject = typeObject, InvokeMethod = testMethod, 
                NameSpace = type.FullName, Attributes = attrs, TestName = testMethod.Name, 
                StartTime = DateTime.Now.Ticks });

            finalContinuation = task.ContinueWith(continuation =>
                continuation.Exception.Handle(ex =>
                {
                    var dataFault = task.AsyncState as IndividualTestState;
                    dataFault.EndTime = DateTime.Now.Ticks;
                    dataFault.Result = false;
                    dataFault.ThrownException = ex;
                    if (dataFault.CatchTimeOut)
                    {
                        if (GlobalTestStates.repeatBook.ContainsKey(type.FullName))
                        {
                            GlobalTestStates.repeatBook[type.FullName].testMethods.
                                Add(testMethod);
                        }
                        else
                        {
                            GlobalTestStates.repeatBook.Add(type.FullName, new ReRunCase(type, 
                                new List<MethodInfo>() { testMethod }, type.FullName, attrs));
                        }
                    }
                    Console.WriteLine("***\n\n\n{0}.{1}: Failed\n\n\n", 
                        dataFault.NameSpace, dataFault.TestName);
                    GlobalTestStates.Add(dataFault);
                    return false;
                })
            , TaskContinuationOptions.OnlyOnFaulted
            );

            finalContinuation = task.ContinueWith((continuation) =>
            {
                var dataPass = task.AsyncState as IndividualTestState;
                if (dataPass != null)
                {
                    dataPass.EndTime = DateTime.Now.Ticks;
                    dataPass.Result = true;
                    dataPass.ThrownException = null;
                    Console.WriteLine("***\n\n\n{0}.{1}: Passed\n\n\n", 
                        dataPass.NameSpace, dataPass.TestName);
                    GlobalTestStates.Add(dataPass);
                }
            }
            , TaskContinuationOptions.NotOnFaulted
            );

            finalContinuation.ContinueWith((continuation) =>
                {
                    if (GlobalTestStates.GetScenarioCount > 0)
                    {
                        GlobalTestStates.DecrementScenarioCount();
                        Console.WriteLine("\nRemaining Scenarios: {0}\n", GlobalTestStates.GetScenarioCount);
                        if (!Convert.ToBoolean(GlobalTestStates.GetScenarioCount))
                        {
                            if (TearDownFeature != null)
                                TearDownFeature.Invoke(typeObject, null);
                            if (GlobalTestStates.featureBook.ContainsKey(type.FullName))
                            {
                                Console.WriteLine("Feature Book Key Found");
                                GlobalTestStates.featureBook[type.FullName].EndTick = DateTime.Now.Ticks;
                                IndividualFeatureTestState itfs = GlobalTestStates.featureBook[type.FullName];
                                GlobalTestStates.AddFeature(itfs);
                            }                          
                            else
                            {
                                Console.WriteLine("Key not found in feature Book");
                                throw new ArgumentNullException();
                            }
                            while (bw.IsBusy) ;
                            bw.RunWorkerAsync(new Inter(typeObject, type, Inter.MT.Type, TearDownFeature));
                        }
                    }
                });
        }
    }
}
