namespace Tarsvin.Runner
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ReRunCase
    {
        internal Object obj;
        internal MethodInfo testMethod;
        internal string nameSpace;
        internal List<string> attrs;
        public ReRunCase(Object obj, MethodInfo testMethod, string nameSpace, ref List<string> attrs)
        {
            this.obj = obj;
            this.testMethod = testMethod;
            this.nameSpace = nameSpace;
            this.attrs = attrs;
        }
    }
}
