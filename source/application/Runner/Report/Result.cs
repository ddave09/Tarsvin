namespace Tarsvin.Runner.Report
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.Reflection;

	public class Super
	{
		protected bool Result
		{
			get;
			set;
		}

		protected long StartTime
		{
			get;
			set;
		}

		protected long EndTime
		{
			get;
			set;
		}

		protected TimeSpan ExecutionTime
		{
			get
			{
				return new TimeSpan(EndTime - StartTime);
			}
		}
	}

	public class Master : Super
	{
		protected string Name
		{
			get;
			set;
		}

		protected List<string> Attributes
		{
			get;
			set;
		}
	}

	public class _Result : Super
	{
		ulong errorCount = 0;
		ulong failureCount = 0;
		ulong testCount = 0;
		List<Feature> features;
	}

	public class Feature : Master
	{
		List<Scenario> scenarios;
	}

	public class Scenario : Master
	{
		public string FailureMessage
		{
			get
			{
				Exception e = this.ThrownException;
				__Exception(ref e);
				return e.Message;
			}
		}

		public Exception ThrownException
		{
			get;
			set;
		}

		public string ExceptionStackTrace
		{
			get
			{
				Exception e = this.ThrownException;
				__Exception(ref e);
				return e.StackTrace;
			}
		}

		public bool IsFailure
		{
			get
			{
				Exception e = this.ThrownException;
				__Exception(ref e);
				if (Regex.IsMatch(e.GetType().ToString(), "[a-z|A-Z|.]*[a|A]ssert[a-z|A-Z|.]*"))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		private void __Exception(ref Exception e)
		{
			while (e is TargetInvocationException && e.InnerException != null)
				e = e.InnerException;
		}
	}
}
