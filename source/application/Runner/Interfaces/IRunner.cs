using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Runner.Interfaces
{
    interface IRunner
    {
        void Run(Object typeObject, MethodInfo testMethod, string nameSpace, List<string> attrs);
    }
}
