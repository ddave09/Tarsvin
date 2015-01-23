namespace Tarsvin.StepBinder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public static class FunctionBinder
    {
        public static void FindKey(string nameSpace, string hierarchy, string type, out InvokeInfo iI)
        {
            if (!BindMap.bitMap.TryGetValue(GenerateTypeName(nameSpace, type, hierarchy), out iI))
            {
                throw new ArgumentNullException();
            }
        }

        public static string GenerateTypeName(string nameSpace, string type, string hierarchy)
        {
            string rootNamespace = string.Format("{0}.", nameSpace);

            if (hierarchy == string.Empty)
            {

                string x = string.Format("{0}{1}Steps", rootNamespace, type);
                return x;
            }

            string y = string.Format("{0}{1}.{2}Steps", rootNamespace, hierarchy, type);
            return y;
        }

        public static MethodInfo FetchX(List<MethodInfo> methods, string beforeX, string before_after)
        {
            foreach (MethodInfo method in methods)
            {
                List<Attribute> li = method.GetCustomAttributes().ToList();
                foreach (Attribute a in li)
                {
                    if (StringComparer.OrdinalIgnoreCase.Equals(a.ToString(), "Tarsvin.CustomAttributes." + before_after + beforeX + "attribute"))
                    {
                        return method;
                    }
                }
            }
            return null;
        }

        public static void CallBeforeFeature(string nameSpace, string hierarchy, string type)
        {
            InvokeInfo iI = null;
            FindKey(nameSpace, hierarchy, type, out iI);
            string dllPath = iI.dllPath;
            Type t = iI.type;
            List<MethodInfo> methods = iI.methods;
            MethodInfo methodToInvoke = null;
            if ((methodToInvoke = FetchX(methods, "feature", "before")) == null)
            {
                throw new MissingMethodException();
            }
            List<string> ltags = new List<string>();
            ltags.Add(nameSpace);
            if (hierarchy != string.Empty)
            {
                ltags.AddRange(hierarchy.Split('.').ToList());
            }
            object[] parameters = BindFeatureParameters(ltags, type);
            methodToInvoke.Invoke(Activator.CreateInstance(t), parameters);
        }

        public static Object CallBeforeScenario(string nameSpace, string hierarchy, string type, string scenarioName, string scenarioAttrs)
        {
            InvokeInfo iI = null;
            FindKey(nameSpace, hierarchy, type, out iI);
            string dllPath = iI.dllPath;
            Type t = iI.type;
            List<MethodInfo> methods = iI.methods;
            MethodInfo methodToInvoke = null;
            if ((methodToInvoke = FetchX(methods, "scenario", "before")) == null)
            {
                throw new MissingMethodException();
            }
            List<string> ltags = new List<string>();
            ltags.AddRange(scenarioAttrs.Split('.').ToList());
            object[] parameters = BindScenarioParameters(ltags, type, scenarioName);
            Object obj = Activator.CreateInstance(t);
            methodToInvoke.Invoke(obj, parameters);
            return obj;
        }

        private static object[] BindScenarioParameters(List<string> ltags, string type, string scenarioName)
        {
            string[] tags = ltags.ToArray();
            List<object> param = new List<object>();
            param.Add(tags);
            param.Add(type);
            param.Add(scenarioName);
            object[] parameters = param.ToArray();
            return parameters;
        }

        private static object[] BindFeatureParameters(List<string> ltags, string type)
        {
            string[] tags = ltags.ToArray();
            List<object> param = new List<object>();
            param.Add(tags);
            param.Add(type);
            object[] parameters = param.ToArray();
            return parameters;
        }

        public static void CallAfterX(string nameSpace, string hierarchy, string type)
        {
            InvokeInfo iI = null;
            FindKey(nameSpace, hierarchy, type, out iI);
            string dllPath = iI.dllPath;
            Type t = iI.type;
            List<MethodInfo> methods = iI.methods;
            MethodInfo methodToInvoke = null;

            if ((methodToInvoke = FetchX(methods, "feature", "after")) == null)
            {
                throw new MissingMethodException();
            }

            methodToInvoke.Invoke(Activator.CreateInstance(t), null);
        }

        public static void CallAfterX(string nameSpace, string hierarchy, string type, Object obj)
        {
            InvokeInfo iI = null;
            FindKey(nameSpace, hierarchy, type, out iI);
            string dllPath = iI.dllPath;
            List<MethodInfo> methods = iI.methods;
            MethodInfo methodToInvoke = null;

            if ((methodToInvoke = FetchX(methods, "scenario", "after")) == null)
            {
                throw new MissingMethodException();
            }

            methodToInvoke.Invoke(obj, null);
        }

        public static MethodInfo FetchFunctionByName(List<MethodInfo> methods, string functionName)
        {
            foreach (MethodInfo method in methods)
            {
                List<Attribute> li = method.GetCustomAttributes().ToList();
                foreach (Attribute a in li)
                {
                    string x = a.ToString();
                }

                if (StringComparer.OrdinalIgnoreCase.Equals(functionName, method.Name))
                {
                    return method;
                }
            }
            return null;
        }

        public static void Given(string nameSpace, string hierarchy, string type, string functionName, Object obj)
        {
            FetchExecuteStep(nameSpace, hierarchy, type, functionName, obj);
        }

        public static void And(string nameSpace, string hierarchy, string type, string functionName, Object obj)
        {
            FetchExecuteStep(nameSpace, hierarchy, type, functionName, obj);
        }

        public static void When(string nameSpace, string hierarchy, string type, string functionName, Object obj)
        {
            FetchExecuteStep(nameSpace, hierarchy, type, functionName, obj);
        }

        public static void Then(string nameSpace, string hierarchy, string type, string functionName, Object obj)
        {
            FetchExecuteStep(nameSpace, hierarchy, type, functionName, obj);
        }

        public static void But(string nameSpace, string hierarchy, string type, string functionName, Object obj)
        {
            FetchExecuteStep(nameSpace, hierarchy, type, functionName, obj);
        }

        private static void FetchExecuteStep(string nameSpace, string hierarchy, string type, string functionName, Object obj)
        {
            InvokeInfo iI = null;
            FindKey(nameSpace, hierarchy, type, out iI);
            string dllPath = iI.dllPath;
            Type t = iI.type;
            List<MethodInfo> methods = iI.methods;
            MethodInfo methodToInvoke = null;
            if ((methodToInvoke = FetchFunctionByName(methods, functionName)) == null)
            {
                throw new MissingMethodException(t.ToString(), functionName);
            }
            methodToInvoke.Invoke(obj, null);
        }
    }
}
