using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    public class IndividualFeatureTestState
    {
        public string FeatureName
        {
            get;
            set;
        }

        public long StartTick
        {
            get;
            set;
        }

        public long EndTick
        {
            get;
            set;
        }

        public TimeSpan FeatureExecutionTime
        {
            get
            {
                return new TimeSpan(this.EndTick - this.StartTick);
            }
        }
    }
}