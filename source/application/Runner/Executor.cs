namespace Tarsvin.Runner
{
	using System;
	using System.Configuration;
	using System.Diagnostics;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using TestPipe.Common;
	using Tarsvin.Runner.Interfaces;

	public enum SystemState : sbyte
	{
		Initial,
		Repeat
	};

	internal class Executor
	{
		internal delegate void AsyncReRunHandler(KeyValuePair<string, ReRunCase> casePair);
		internal delegate void AsyncTaskHandler();
		internal delegate void AsyncMethodHandler(Object obj, Type type, MethodInfo TearDownFeature);

		SystemState ss = SystemState.Initial;
		IRunner run = null;
		BackgroundWorker bw;
		List<DllInfo> dlls = new List<DllInfo>();
		List<Type> types = new List<Type>();
		List<MethodInfo> methods = new List<MethodInfo>();
		ILogManager logActivatorException = new Logger();
		string projectPath = string.Empty;
		string resultPath = string.Empty;
		List<string> include = null;
		List<string> exclude = null;

		internal Executor(string selection, List<DllInfo> dlls, string projectPath, 
			string resultPath, string[] include, string[] exclude)
		{
			this.dlls = dlls;
			if (StringComparer.OrdinalIgnoreCase.Equals(selection, "Sequential"))
				run = new SequentialRunner();
			else if (StringComparer.OrdinalIgnoreCase.Equals(selection, "Parallel"))
				run = new ParallelRunner();
			bw = new BackgroundWorker();
			bw.DoWork += MethodHandlerInterface;
			bw.RunWorkerCompleted += MethodHandlerInterfaceCompleted;
			this.projectPath = projectPath;
			this.resultPath = resultPath;
			if(include != null)
				this.include = new List<string>(include);
			if(exclude != null)
				this.exclude = new List<string>(exclude);

		}

		internal void ExecuteReRunCase(KeyValuePair<string, ReRunCase> casePair)
		{
			ss = SystemState.Repeat;
			Console.WriteLine("\n***Re-Run of Case {0}***\n", casePair.Key);
			this.types = new List<Type> { casePair.Value.type };
			this.methods = casePair.Value.testMethods;
			AsyncTaskHandler handler = new AsyncTaskHandler(TypeHandler);
			handler.BeginInvoke(null, null);
		}

		internal void InitializeSystem(Executor exe)
		{
			GlobalTestStates.GrandStartTime = DateTime.Now.Ticks;
			exe.ExecuteTest();
		}

		internal void ExecuteTest()
		{
			if (this.dlls.Count == 0 && GlobalTestStates.repeatBook.Count == 0)
			{
				InitiateSummary();
			}
			else if (Convert.ToBoolean(this.dlls.Count))
			{
				DllInfo dll = this.dlls.Last();
				this.dlls.RemoveAt(this.dlls.Count - 1);
				if (dll.path == null)
					this.types = TestTypes(Assembly.GetExecutingAssembly().GetTypes().ToList());
				else
					this.types = TestTypes(Assembly.Load(AssemblyName.GetAssemblyName(dll.path)).GetTypes().ToList());
				AsyncTaskHandler handler = new AsyncTaskHandler(TypeHandler);
				handler.BeginInvoke(null, null);
			}
			else if (Convert.ToBoolean(GlobalTestStates.repeatBook.Count))
			{
				GlobalTestStates.onlyOnce = false;
				KeyValuePair<string, ReRunCase> casePair = GlobalTestStates.repeatBook.First();
				GlobalTestStates.repeatBook.Remove(casePair.Key);
				AsyncReRunHandler handler = new AsyncReRunHandler(ExecuteReRunCase);
				handler.BeginInvoke(casePair, null, null);
			}
		}

		private void InitiateSummary()
		{
			GlobalTestStates.GrandEndTime = DateTime.Now.Ticks;
			Console.WriteLine("\n***Done!***\n");
			this.LogTestSummary();
		}

		private void TypeHandler()
		{
			if (Convert.ToBoolean(this.types.Count))
			{
				Type type = this.types.Last();
				this.types.RemoveAt(this.types.Count - 1);

				//Lock below once feature run in parallel
				if (!GlobalTestStates.ResultSet.ContainsKey(type.FullName))
				{
					Result result = new Result();
					result.itfs.FeatureName = type.FullName;
					result.itfs.StartTick = DateTime.Now.Ticks;
					GlobalTestStates.ResultSet.Add(type.FullName, result);
				}

				Object obj = null;
				try
				{
					obj = Activator.CreateInstance(type);
				}
				catch (Exception e)
				{
					Console.WriteLine("\n***Error while instantiating type : {0}***\n{1}", type.Name,
						"\n***Could not find Step class, Most Probable: Check feature file namespace or step class name***\n");
					logActivatorException.Fatal("\n***Could not find Step class, Most Probable: Check feature file namespace or step class name***\n", e);
					System.Environment.Exit(-1);
				}
				MethodInfo TearDownFeature = null;
				if (Convert.ToBoolean(this.methods.Count))
				{
					TestMethods(type.GetMethods().ToList(), ref TearDownFeature);
				}
				else
				{
					this.methods = TestMethods(type.GetMethods().ToList(), ref TearDownFeature);
				}

				if (ss == SystemState.Initial)
					GlobalTestStates.IncrementTestCount(this.methods.Count);
				else if (ss == SystemState.Repeat)
					GlobalTestStates.IncrementReTestCount(this.methods.Count);

				// Need to change when feature will run in parallel
				GlobalTestStates.SetScenarioCount(this.methods.Count);
				AsyncMethodHandler handler = new AsyncMethodHandler(MethodHandler);
				handler.BeginInvoke(obj, type, TearDownFeature, null, null);
			}
			else
			{
				Console.WriteLine("\n***All available types are done for given project!***\n");
				AsyncTaskHandler handler = new AsyncTaskHandler(ExecuteTest);
				handler.BeginInvoke(null, null);
			}
		}

		private void MethodHandler(Object obj, Type type, MethodInfo TearDownFeature)
		{
			if (Convert.ToBoolean(this.methods.Count))
			{
				MethodInfo method = this.methods.Last();
				this.methods.RemoveAt(this.methods.Count - 1);
				List<string> attrsValues = this.GetAttributesConstructorValues(method);
				run.Run(obj, method, type, attrsValues, bw, ss, this.methods.Count, TearDownFeature);
			}
			else if (!Convert.ToBoolean(GlobalTestStates.GetScenarioCount))
			{
				Console.WriteLine("\n***All available scenarios are done for given type!***\n");
				AsyncTaskHandler handler = new AsyncTaskHandler(TypeHandler);
				handler.BeginInvoke(null, null);
			}
		}

		internal void MethodHandlerInterface(object sender, DoWorkEventArgs e)
		{
			Inter i = e.Argument as Inter;
			e.Result = new Inter(i);

			if (i.direct)
			{
				AsyncMethodHandler handler = new AsyncMethodHandler(MethodHandler);
				handler.BeginInvoke(i.obj, i.type, i.TearDownFeature, null, null);
			}
			else
			{
				AsyncTaskHandler handler = new AsyncTaskHandler(TypeHandler);
				handler.BeginInvoke(null, null);
			}

		}

		private void MethodHandlerInterfaceCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Inter i = e.Result as Inter;

			if (i.direct)
			{
				Console.WriteLine("\n***Scenario Initiated***\n");
			}
			else
			{
				Console.WriteLine("\n***Feature Initiated***\n");
			}
		}

		private List<string> GetAttributesConstructorValues(MethodInfo method)
		{
			List<string> attrValues = new List<string>();
			IList<CustomAttributeData> cadS = method.GetCustomAttributesData();
			foreach (CustomAttributeData cad in cadS)
			{
				IList<CustomAttributeTypedArgument> cadAs = cad.ConstructorArguments;
				foreach (CustomAttributeTypedArgument cata in cadAs)
				{
					attrValues.Add(cata.ToString());
				}
			}
			return attrValues;
		}

		private List<MethodInfo> TestMethods(List<MethodInfo> methods, ref MethodInfo TearDownFeature)
		{
			List<MethodInfo> tempMethods = new List<MethodInfo>();
			foreach (MethodInfo method in methods)
			{
				List<Attribute> li = method.GetCustomAttributes().ToList();
				if (IsTestMethod(li, ref TearDownFeature, method))
				{
					tempMethods.Add(method);
				}
			}
			return tempMethods;
		}

		private List<Type> TestTypes(List<Type> types)
		{
			List<Type> tempTypes = new List<Type>();
			foreach (Type type in types)
			{
				List<CustomAttributeData> li = type.CustomAttributes.ToList();
				if (IsTestClass(li))
				{
					tempTypes.Add(type);
				}
			}
			return tempTypes;
		}

		private bool IsTestClass(List<CustomAttributeData> li)
		{
			foreach (CustomAttributeData attr in li)
			{
				IList<CustomAttributeTypedArgument> lii = attr.ConstructorArguments;
				if (StringComparer.OrdinalIgnoreCase.Equals(attr.AttributeType.ToString(), "Tarsvin.customAttributes.fixtureattr"))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsTestMethod(List<Attribute> li, ref MethodInfo TearDownFeature, MethodInfo method)
		{
			foreach (Attribute attr in li)
			{
				string x = attr.ToString();

				if (StringComparer.OrdinalIgnoreCase.Equals(attr.ToString(), "Tarsvin.customAttributes.CaseAttr"))
				{
					return true;
				}
				else if (StringComparer.OrdinalIgnoreCase.Equals(attr.ToString(), "Tarsvin.customAttributes.FixtureEndAttr"))
				{
					TearDownFeature = method;
				}
			}
			return false;
		}

		private void LogTestSummary()
		{
			Console.WriteLine("\n***Features : {0}***\n", GlobalTestStates.ResultSet.Count);

			Console.WriteLine(string.Format("\n***Tests: {0} \n ReTests: {1} \n Failures: {2} \n ReFailures: {3} \n Errors: {4}" +
				"\n ReErrors: {5} ***\n",
				GlobalTestStates.TestsRun, GlobalTestStates.ReTestsRun,
				GlobalTestStates.FailureCount, GlobalTestStates.ReFailureCount,
				GlobalTestStates.Error, GlobalTestStates.ReError));

			XmlResultWriter xmlWriter = new XmlResultWriter(this.resultPath, this.projectPath);
			foreach (KeyValuePair<string, Result> element in GlobalTestStates.ResultSet)
			{
				xmlWriter.StartSuiteElement(element.Value.itfs);
				IEnumerator<IndividualTestState> its = element.Value.its.GetEnumerator();
				while (its.MoveNext())
				{
					xmlWriter.SaveTestResult(its.Current);
				}
				xmlWriter.EndSuiteElement(element.Value.itfs);
				Console.WriteLine("\n***Feature: {0} Scenarios: {1}***\n", element.Key.Split('.').Last(), element.Value.its.Count);
			}
			xmlWriter.Terminate();
			System.Environment.Exit(GlobalTestStates.FailureCount - (GlobalTestStates.ReTestsRun - GlobalTestStates.ReFailureCount));
		}
	}
}
