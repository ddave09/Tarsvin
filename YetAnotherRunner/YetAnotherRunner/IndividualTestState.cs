using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YetAnotherRunner
{
    public class IndividualTestState
    {
        public string testName;
        
        public IndividualTestState()
        {
            
        }

        public long StartTime
        {
            get;
            set;
        }

        public long EndTime
        {
            get;
            set;
        }

        public string TestName
        {
            get;
            set;
        }

        public TimeSpan ExecTime
        {
            get;
            set;
        }
    }
}
