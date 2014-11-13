using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FixtureAttr : Attribute
    {
        public string value;
        public FixtureAttr(string value)
        {
            this.value = value;
        }

        string GetValue
        {
            get
            {
                return this.value;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CaseAttr : Attribute
    {
        public string value;
        public CaseAttr(string value)
        {
            this.value = value;
        }

        string GetValue
        {
            get
            {
                return this.value;
            }
        }
    }
}
