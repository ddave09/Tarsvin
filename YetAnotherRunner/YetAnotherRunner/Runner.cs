using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YetAnotherRunner
{
    public class Runner
    {
        public Runner()
        {

        }

        ~Runner()
        {

        }

        public void Run(Object typeObject, MethodInfo testMethod, string nameSpace, List<string> attrs)
        {
            Task task = Task.Factory.StartNew((Object obj) =>
            {
                IndividualTestState its = obj as IndividualTestState;
                testMethod.Invoke(typeObject, null);
            },
            new IndividualTestState() { NameSpace = nameSpace, Attributes = attrs, TestName = testMethod.Name, StartTime = DateTime.Now.Ticks });

            task.ContinueWith(continuation =>
                continuation.Exception.Handle(ex =>
                {
                    var dataFault = task.AsyncState as IndividualTestState;
                    dataFault.EndTime = DateTime.Now.Ticks;
                    dataFault.Result = false;
                    dataFault.ThrownException = ex;
                    GlobalTestStates.Add(dataFault);
                    if (GlobalTestStates.GetScenarioCount > 0)
                    {
                        GlobalTestStates.DecrementScenarioCount();
                    }
                    return false;
                })
            , TaskContinuationOptions.OnlyOnFaulted
            );

            Task continueTask = task.ContinueWith((continuation) =>
            {
                var dataPass = task.AsyncState as IndividualTestState;
                if (dataPass != null)
                {
                    dataPass.EndTime = DateTime.Now.Ticks;
                    dataPass.Result = true;
                    dataPass.ThrownException = null;
                    GlobalTestStates.Add(dataPass);
                    if (GlobalTestStates.GetScenarioCount > 0)
                    {
                        GlobalTestStates.DecrementScenarioCount();
                    }
                }
            }
            , TaskContinuationOptions.NotOnFaulted
            );
        }
    }
}
