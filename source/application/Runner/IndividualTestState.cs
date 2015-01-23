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
        public IndividualTestState()
        {
            
        }

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
            get;
            set;
        }

        public Exception ThrownException
        {
            get;
            set;
        }

        private void DesiredInnerException(ref Exception e)
        {
            while (e is TargetInvocationException && e.InnerException != null)
                e = e.InnerException;
        }

        public string ExceptionMessageStackTrace
        {
            get
            {
                Exception e = this.ThrownException;
                string ignoreStackTraceException = ConfigurationManager.AppSettings["IgnoreStackTrace"];
                string retVal = null;
                DesiredInnerException(ref e);
                if (!StringComparer.OrdinalIgnoreCase.Equals(ignoreStackTraceException, e.GetType().ToString()))
                    retVal = e.StackTrace + "\r\n\r\n";
                return retVal += "  Message: " + e.Message;
            }
        }

        public bool CatchTimeOut
        {
            get
            {
                Exception e = this.ThrownException;
                string captureTimeOut = ConfigurationManager.AppSettings["TimeOut"];
                DesiredInnerException(ref e);
                if (!StringComparer.OrdinalIgnoreCase.Equals(captureTimeOut, e.GetType().ToString()))
                    return true;
                else
                    return false;
            }
        }
    }
}
