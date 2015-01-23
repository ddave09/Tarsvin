namespace Tarsvin.Runner
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Tarsvin.Runner.Interfaces;

    public class SequentialRunner : IRunner
    {
        public void Run(Object typeObject, MethodInfo testMethod, string nameSpace, List<string> attrs)
        {
            IndividualTestState its = new IndividualTestState();
            its.NameSpace = nameSpace;
            its.Attributes = attrs;
            its.TestName = testMethod.Name;
            its.StartTime = DateTime.Now.Ticks;
            try
            {
                testMethod.Invoke(typeObject, null);
                its.EndTime = DateTime.Now.Ticks;
                its.Result = true;
                its.ThrownException = null;
                Console.WriteLine("***\n\n\n{0}.{1}: Passed\n\n\n", its.NameSpace, its.TestName);
            }
            catch (Exception e)
            {
                its.EndTime = DateTime.Now.Ticks;
                its.Result = false;
                its.ThrownException = e;
                if (its.CatchTimeOut)
                    GlobalTestStates.PushToReRunList(its.InvokeObject, its.InvokeMethod, its.NameSpace, its.Attributes);
                Console.WriteLine("***\n\n\n{0}.{1}: Failed\n\n\n", its.NameSpace, its.TestName);
            }
            GlobalTestStates.Add(its);
            if (GlobalTestStates.GetScenarioCount > 0)
            {
                GlobalTestStates.DecrementScenarioCount();
            }
        }
    }

    public class ParallelRunner : IRunner
    {
        public event EventHandler InvokeCmpt;

        protected virtual void OnInvokeCmpt(EventArgs e)
        {
            if (InvokeCmpt != null)
                InvokeCmpt(this, e);
        }

        public void Run(Object typeObject, MethodInfo testMethod, string nameSpace, List<string> attrs)
        {
            Task finalContinuation = null;
            Task task = Task.Factory.StartNew((Object obj) =>
            {
                IndividualTestState its = obj as IndividualTestState;
                testMethod.Invoke(typeObject, null);
                OnInvokeCmpt(EventArgs.Empty);
            },
            new IndividualTestState() { InvokeObject = typeObject, InvokeMethod = testMethod, NameSpace = nameSpace, Attributes = attrs, TestName = testMethod.Name, StartTime = DateTime.Now.Ticks });

            finalContinuation = task.ContinueWith(continuation =>
                continuation.Exception.Handle(ex =>
                {
                    var dataFault = task.AsyncState as IndividualTestState;
                    dataFault.EndTime = DateTime.Now.Ticks;
                    dataFault.Result = false;
                    dataFault.ThrownException = ex;
                    if (dataFault.CatchTimeOut)
                        GlobalTestStates.PushToReRunList(dataFault.InvokeObject, dataFault.InvokeMethod, dataFault.NameSpace, dataFault.Attributes);
                    Console.WriteLine("***\n\n\n{0}.{1}: Failed\n\n\n", dataFault.NameSpace, dataFault.TestName);
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
                    Console.WriteLine("***\n\n\n{0}.{1}: Passed\n\n\n", dataPass.NameSpace, dataPass.TestName);
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
                    }
                });
        }
    }
}
