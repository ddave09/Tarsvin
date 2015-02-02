namespace Tarsvin.Runner.Interfaces
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRunner
    {
        void Run(Object typeObject, MethodInfo testMethod, string nameSpace, List<string> attrs, BackgroundWorker bw);
    }
}
