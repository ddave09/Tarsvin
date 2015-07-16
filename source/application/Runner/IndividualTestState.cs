namespace Tarsvin.Runner
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	public class IndividualTestState
	{
		public Object InvokeObject
		{
			get;
			set;
		}

		public MethodInfo InvokeMethod
		{
			get;
			set;
		}

		/// <summary>
		/// This property stores the namespace for the scenario
		/// </summary>
		public string NameSpace
		{
			get;
			set;
		}

		public bool Executed
		{
			get;
			set;
		}

		/// <summary>
		/// This property stores attributes for the scenario
		/// </summary>
		public List<string> Attributes
		{
			get;
			set;
		}

		public string TestName
		{
			get;
			set;
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

		public TimeSpan ExecTime
		{
			get
			{
				return new TimeSpan(this.EndTime - this.StartTime);
			}
		}

		/// <summary>
		/// This property can have two values Pass/Fail
		/// </summary>
		public bool Result
		{
			get;
			set;
		}

		/// <summary>
		/// In case of Failure this property can be used record failure reason
		/// </summary>
		public string ReasonOfFailure
		{
			get
			{
				Exception e = this.ThrownException;
				DesiredInnerException(ref e);
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
				DesiredInnerException(ref e);
				return e.StackTrace;
			}
		}

		public bool IsFailure
		{
			get
			{
				Exception e = this.ThrownException;
				string captureFailure = ConfigurationManager.AppSettings["failure"];
				DesiredInnerException(ref e);
				if (StringComparer.OrdinalIgnoreCase.Equals(captureFailure, e.GetType().ToString()))
					return true;
				else
					return false;
			}
		}

		public bool IsTimeOut
		{
			get
			{
				Exception e = this.ThrownException;
				string captureTimeOut = ConfigurationManager.AppSettings["timeout"];
				DesiredInnerException(ref e);
				if (StringComparer.OrdinalIgnoreCase.Equals(captureTimeOut, e.GetType().ToString()))
					return true;
				else
					return false;
			}
		}

		private void DesiredInnerException(ref Exception e)
		{
			while (e is TargetInvocationException && e.InnerException != null)
				e = e.InnerException;
		}
	}
}
