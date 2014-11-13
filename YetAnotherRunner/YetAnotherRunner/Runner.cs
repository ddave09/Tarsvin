using System;
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

        public void Run(Object typeObject, MethodInfo testMethod)
        {
            Task task = Task.Factory.StartNew((Object obj) =>
            {
                IndividualTestState its = obj as IndividualTestState;
                testMethod.Invoke(typeObject, null);
            },
            new IndividualTestState() { TestName = testMethod.Name, StartTime = DateTime.Now.Ticks });

            Task continueTask = task.ContinueWith((continuation) =>
            {
                var data = task.AsyncState as IndividualTestState;
                if (data != null)
                {
                    data.EndTime = DateTime.Now.Ticks;
                    long elapsedTicks = data.EndTime - data.StartTime;
                    TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                    data.ExecTime = elapsedSpan;
                    GlobalTestStates.manageState.Add(data);
                    Console.WriteLine("Time span {0} Test name {1}", elapsedSpan, data.TestName);
                    if (GlobalTestStates.GetScenarioCount > 0)
                    {
                        GlobalTestStates.DecrementScenarioCount();
                    }
                }
            });
        }
    }
}
