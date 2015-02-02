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

    public class Executor
    {
        IRunner run = null;
        BackgroundWorker bw;
        public Executor(string selection)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(selection, "Sequential"))
                run = new SequentialRunner();              
            else if (StringComparer.OrdinalIgnoreCase.Equals(selection, "Parallel"))
                run = new ParallelRunner();
            bw = new BackgroundWorker();
            bw.DoWork += Temp;
        }

        private void Temp(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Async Event Test");
        }

        public void ExcecuteTest(List<DllInfo> dlls)
        {
            ILogManager logActivatorException = new Logger();
            Assembly runtimeAssembly = null;
            GlobalTestStates.GrandStartTime = DateTime.Now.Ticks;
            foreach (DllInfo dllinfo in dlls)
            {
                runtimeAssembly = Assembly.Load(AssemblyName.GetAssemblyName(dllinfo.path));
                List<Type> types = TestTypes(runtimeAssembly.GetTypes().ToList());
                foreach (Type type in types)
                {
                    IndividualFeatureTestState itfs = new IndividualFeatureTestState();
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
                    List<MethodInfo> methods = TestMethods(type.GetMethods().ToList(), ref TearDownFeature);
                    itfs.FeatureName = type.FullName;
                    itfs.StartTick = DateTime.Now.Ticks;
                    foreach (MethodInfo method in methods)
                    {
                        List<string> attrsValues = this.GetAttributesConstructorValues(method);
                        GlobalTestStates.IncrementScenarioCount();
                        run.Run(obj, method, type.FullName, attrsValues, bw);
                    }
                    while (Convert.ToBoolean(GlobalTestStates.GetScenarioCount)) ;
                    TearDownFeature.Invoke(obj, null);
                    itfs.EndTick = DateTime.Now.Ticks;
                    GlobalTestStates.AddFeature(itfs);
                }
            }
            GlobalTestStates.GrandEndTime = DateTime.Now.Ticks;
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

        public List<Type> TestTypes(List<Type> types)
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

        public bool IsTestClass(List<CustomAttributeData> li)
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

        public bool IsTestMethod(List<Attribute> li, ref MethodInfo TearDownFeature, MethodInfo method)
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
    }
}
