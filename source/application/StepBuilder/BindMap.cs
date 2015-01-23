namespace Tarsvin.StepBinder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class InvokeInfo
    {
        public Type type;
        public List<MethodInfo> methods;
        public string dllPath = null;
        public InvokeInfo(string dllPath, Type type, List<MethodInfo> li)
        {
            this.type = type;
            this.methods = li;
            this.dllPath = dllPath;
        }
    }

    public static class BindMap
    {
        public static Dictionary<string, InvokeInfo> bitMap = new Dictionary<string, InvokeInfo>();
        public static void AddToBindMap(string fullName, string dllPath, Type type, List<MethodInfo> li)
        {
            InvokeInfo iI = new InvokeInfo(dllPath, type, li);
            bitMap.Add(fullName, iI);
        }
    }
}
