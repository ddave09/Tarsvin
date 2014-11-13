using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherRunner
{

    public class Executor
    {
        Assembly afterFeatureAssembly = null;
        Type afterFeatureType = null;
        MethodInfo FeatureTear = null;
        Runner run = null;
        public Executor()
        {
            afterFeatureAssembly = Assembly.LoadFrom(@"C:\_TestPipe\source\application\TestPipe.SpecFlow\bin\Debug\TestPipe.SpecFlow.dll");
            afterFeatureType = afterFeatureAssembly.GetType("TestPipe.Specflow.CommonHooks", false, true);
            FeatureTear = afterFeatureType.GetMethod("TeardownFeature");
            run = new Runner();
        }

        public void ExcecuteTest(List<DllInfo> dlls)
        {
            Assembly runtimeAssembly = null;           
            foreach (DllInfo dllinfo in dlls)
            {
                runtimeAssembly = Assembly.Load(AssemblyName.GetAssemblyName(dllinfo.path));
                List<Type> types = TestTypes(runtimeAssembly.GetTypes().ToList());
                foreach (Type type in types)
                {
                    Object obj = Activator.CreateInstance(type);
                    List<MethodInfo> methods = TestMethods(type.GetMethods().ToList());
                    foreach (MethodInfo method in methods)
                    {
                        GlobalTestStates.IncrementScenarioCount();
                        run.Run(obj, method);
                    }
                    while (Convert.ToBoolean(GlobalTestStates.GetScenarioCount)) ;
                    FeatureTear.Invoke(Activator.CreateInstance(afterFeatureType), null);
                }
            }            
        }

        private List<MethodInfo> TestMethods(List<MethodInfo> methods)
        {
            List<MethodInfo> tempMethods = new List<MethodInfo>();
            foreach (MethodInfo method in methods)
            {
                List<Attribute> li = method.GetCustomAttributes().ToList();
                if (IsTestMethod(li))
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
                if (StringComparer.OrdinalIgnoreCase.Equals(attr.AttributeType.ToString(), "nunit.framework.testfixtureattribute"))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsTestMethod(List<Attribute> li)
        {
            foreach (Attribute attr in li)
            {
                string x = attr.ToString();
                if (StringComparer.OrdinalIgnoreCase.Equals(attr.ToString(), "nunit.framework.testattribute"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
