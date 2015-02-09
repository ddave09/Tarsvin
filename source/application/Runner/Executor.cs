namespace Tarsvin.Runner
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using TestPipe.Common;
    using Tarsvin.Runner.Interfaces;

    internal class Executor
    {
        internal delegate void AsyncReRunHandler(KeyValuePair<string, ReRunCase> casePair);
        internal delegate void AsyncTaskHandler();
        internal delegate void AsyncMethodHandler(Object obj, Type type, MethodInfo TearDownFeature);

        IRunner run = null;
        BackgroundWorker bw;
        List<DllInfo> dlls = new List<DllInfo>();
        List<Type> types = new List<Type>();
        List<MethodInfo> methods = new List<MethodInfo>();
        ILogManager logActivatorException = new Logger();
        internal Executor(string selection, List<DllInfo> dlls)
        {
            this.dlls = dlls;
            if (StringComparer.OrdinalIgnoreCase.Equals(selection, "Sequential"))
                run = new SequentialRunner();
            else if (StringComparer.OrdinalIgnoreCase.Equals(selection, "Parallel"))
                run = new ParallelRunner();
            bw = new BackgroundWorker();
            bw.DoWork += MethodHandlerInterface;
            bw.RunWorkerCompleted += MethodHandlerInterfaceCompleted;
        }

        internal void ExecuteReRunCase(KeyValuePair<string, ReRunCase> casePair)
        {
            Console.WriteLine("\n**********Re-Run of Case {0}**********\n", casePair.Key);
            this.types = new List<Type> { casePair.Value.type };
            this.methods = casePair.Value.testMethods;
            AsyncTaskHandler handler = new AsyncTaskHandler(TypeHandler);
            handler.BeginInvoke(null, null);
        }

        internal void InitializeSystem(Executor exe)
        {
            GlobalTestStates.GrandStartTime = DateTime.Now.Ticks;
            exe.ExecuteTest();
        }

        internal Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>
            (Dictionary<TKey, TValue> original)
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
                                                                    original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)entry.Value);
            }
            return ret;
        }

        internal void ExecuteTest()
        {
            if (this.dlls.Count == 0 && GlobalTestStates.repeatBookCopy.Count == 0 && GlobalTestStates.repeatInitiated)
            {
                InitiateSummary();
            }

            if (Convert.ToBoolean(this.dlls.Count))
            {
                DllInfo dll = this.dlls.Last();
                this.dlls.RemoveAt(this.dlls.Count - 1);
                if (dll.path == null)
                    this.types = TestTypes(Assembly.GetExecutingAssembly().GetTypes().ToList());
                else
                    this.types = TestTypes(Assembly.Load(AssemblyName.GetAssemblyName(dll.path)).GetTypes().ToList());
                AsyncTaskHandler handler = new AsyncTaskHandler(TypeHandler);
                handler.BeginInvoke(null, null);
            }
            else if (GlobalTestStates.onlyOnce)
            {
                GlobalTestStates.onlyOnce = false;
                GlobalTestStates.repeatInitiated = true;
                GlobalTestStates.repeatBookCopy = CloneDictionary<string, ReRunCase>(GlobalTestStates.repeatBook);
            }
            if (Convert.ToBoolean(GlobalTestStates.repeatBookCopy.Count))
            {
                KeyValuePair<string, ReRunCase> casePair = GlobalTestStates.repeatBookCopy.First();
                GlobalTestStates.repeatBookCopy.Remove(casePair.Key);
                AsyncReRunHandler handler = new AsyncReRunHandler(ExecuteReRunCase);
                handler.BeginInvoke(casePair, null, null);
            }
        }

        private void InitiateSummary()
        {
            GlobalTestStates.GrandEndTime = DateTime.Now.Ticks;
            Console.WriteLine("Done!");
            this.LogTestSummary();
        }

        private void TypeHandler()
        {
            if (Convert.ToBoolean(this.types.Count))
            {
                Type type = this.types.Last();
                this.types.RemoveAt(this.types.Count - 1);
                if (!GlobalTestStates.featureBook.ContainsKey(type.FullName))
                {
                    GlobalTestStates.featureBook.Add(type.FullName, new IndividualFeatureTestState()
                    {
                        FeatureName = type.FullName,
                        StartTick = DateTime.Now.Ticks
                    });
                }
                Object obj = null;
                try
                {
                    obj = Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not find Step class, Most Probable: Check feature file namespace or step class name");
                    logActivatorException.Fatal("Could not find Step class, Most Probable: Check feature file namespace or step class name", e);
                    System.Environment.Exit(-1);
                }
                MethodInfo TearDownFeature = null;
                if (Convert.ToBoolean(this.methods.Count))
                {
                    TestMethods(type.GetMethods().ToList(), ref TearDownFeature);
                }
                else
                {
                    this.methods = TestMethods(type.GetMethods().ToList(), ref TearDownFeature);
                }
                // Need to change when feature will run in parallel
                GlobalTestStates.SetScenarioCount(this.methods.Count);
                AsyncMethodHandler handler = new AsyncMethodHandler(MethodHandler);
                handler.BeginInvoke(obj, type, TearDownFeature, null, null);
            }
            else
            {
                Console.WriteLine("All available types are done for given project!");
                AsyncTaskHandler handler = new AsyncTaskHandler(ExecuteTest);
                handler.BeginInvoke(null, null);
            }
        }

        private void MethodHandler(Object obj, Type type, MethodInfo TearDownFeature)
        {
            if (Convert.ToBoolean(this.methods.Count))
            {
                MethodInfo method = this.methods.Last();
                this.methods.RemoveAt(this.methods.Count - 1);
                List<string> attrsValues = this.GetAttributesConstructorValues(method);
                run.Run(obj, method, type, attrsValues, bw, this.methods.Count, TearDownFeature);
            }
            else if (!Convert.ToBoolean(GlobalTestStates.GetScenarioCount))
            {
                Console.WriteLine("All available scenarios are done for given type!");
                AsyncTaskHandler handler = new AsyncTaskHandler(TypeHandler);
                handler.BeginInvoke(null, null);
            }
        }

        internal void MethodHandlerInterface(object sender, DoWorkEventArgs e)
        {
            Inter i = e.Argument as Inter;
            if (i.direct)
            {
                AsyncMethodHandler handler = new AsyncMethodHandler(MethodHandler);
                handler.BeginInvoke(i.obj, i.type, i.TearDownFeature, null, null);
            }
            else
            {
                AsyncTaskHandler handler = new AsyncTaskHandler(TypeHandler);
                handler.BeginInvoke(null, null);
            }

        }

        private void MethodHandlerInterfaceCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("\nMethod Handler Completed\n");
        }

        private List<string> GetAttributesConstructorValues(MethodInfo method)
        {
            List<string> attrValues = new List<string>();
            IList<CustomAttributeData> cadS = method.GetCustomAttributesData();
            foreach (CustomAttributeData cad in cadS)
            {
                IList<CustomAttributeTypedArgument> cadAs = cad.ConstructorArguments;
                foreach (CustomAttributeTypedArgument cata in cadAs)
                {
                    attrValues.Add(cata.ToString());
                }
            }
            return attrValues;
        }

        private List<MethodInfo> TestMethods(List<MethodInfo> methods, ref MethodInfo TearDownFeature)
        {
            List<MethodInfo> tempMethods = new List<MethodInfo>();
            foreach (MethodInfo method in methods)
            {
                List<Attribute> li = method.GetCustomAttributes().ToList();
                if (IsTestMethod(li, ref TearDownFeature, method))
                {
                    tempMethods.Add(method);
                }
            }
            return tempMethods;
        }

        private List<Type> TestTypes(List<Type> types)
        {
            List<Type> tempTypes = new List<Type>();
            foreach (Type type in types)
            {
                List<CustomAttributeData> li = type.CustomAttributes.ToList();
                if (IsTestClass(li))
                {
                    tempTypes.Add(type);
                }
            }
            return tempTypes;
        }

        private bool IsTestClass(List<CustomAttributeData> li)
        {
            foreach (CustomAttributeData attr in li)
            {
                IList<CustomAttributeTypedArgument> lii = attr.ConstructorArguments;
                if (StringComparer.OrdinalIgnoreCase.Equals(attr.AttributeType.ToString(), "Tarsvin.customAttributes.fixtureattr"))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsTestMethod(List<Attribute> li, ref MethodInfo TearDownFeature, MethodInfo method)
        {
            foreach (Attribute attr in li)
            {
                string x = attr.ToString();

                if (StringComparer.OrdinalIgnoreCase.Equals(attr.ToString(), "Tarsvin.customAttributes.CaseAttr"))
                {
                    return true;
                }
                else if (StringComparer.OrdinalIgnoreCase.Equals(attr.ToString(), "Tarsvin.customAttributes.FixtureEndAttr"))
                {
                    TearDownFeature = method;
                }
            }
            return false;
        }

        private void LogTestSummary()
        {
            IEnumerator ie = GlobalTestStates.manageState.GetEnumerator();
            ILogManager log = new Logger();
            while (ie.MoveNext())
            {
                IndividualTestState its = ie.Current as IndividualTestState;
                string formattedScenario = string.Format("\n********************************************************************************\nScenario Namespace {0}\nScenario Name {1}\nExecution Time {2}\nAttributes:  ",
                    its.NameSpace, its.TestName, its.ExecTime);
                log.Info(formattedScenario);
                Console.Write(formattedScenario);
                foreach (string s in its.Attributes)
                {
                    string formattedAttr = string.Format("{0}   ", s.Trim('"'));
                    log.Info(formattedAttr);
                    Console.Write(formattedAttr);
                }
                string formattedResult = string.Format("\nResult {0}", its.Result);
                log.Info(formattedResult);
                Console.WriteLine(formattedResult);
                if (its.ThrownException != null)
                {
                    string formattedException = string.Format("\n--------------------\n{0}\n--------------------\n",
                        its.ExceptionMessageStackTrace);
                    log.Error(formattedException);
                    Console.Write(formattedException);
                }
            }

            IEnumerator fie = GlobalTestStates.featureState.GetEnumerator();
            while (fie.MoveNext())
            {
                IndividualFeatureTestState itfs = fie.Current as IndividualFeatureTestState;
                string formattedFeature = string.Format("\n********************************************************************************\nFeature Namespace {0}\nExecution Time {1}\n",
                    itfs.FeatureName, itfs.FeatureExecutionTime);
                log.Info(formattedFeature);
                Console.Write(formattedFeature);
            }

            string formattedTrollTime = string.Format("\n*******************************************************************************\nTroll Time: {0}\n*******************************************************************************\nAll available tests are completed\n*******************************************************************************\n",
                GlobalTestStates.GrandExecTime);
            log.Info(formattedTrollTime);
            Console.Write(formattedTrollTime);

            Console.WriteLine("\n**********************Dispalaying ReRun Cases**********************\n");
            foreach (KeyValuePair<string, ReRunCase> element in GlobalTestStates.repeatBook)
            {
                Console.WriteLine(element.Value.nameSpace);
            }
            Console.WriteLine("\n**********************xxxxxxxxxxxxxxxxxxxxxxx**********************\n");
        }

    }
}
