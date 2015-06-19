namespace Tarsvin.Runner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IndividualFeatureTestState
    {
		private bool success;
		public IndividualFeatureTestState()
		{
			success = true;
		}

		public bool Success
		{
			get
			{
				return success;
			}
			set
			{
				success = value;
			}
		}

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