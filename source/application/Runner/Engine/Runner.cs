namespace Tarsvin.Runner.Engine
{
	using System;
	using System.Configuration;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using Tarsvin.Runner.Interfaces;
	using Tarsvin.Runner.Report;

	internal class Inter
	{
		internal Object obj;
		internal Type type;
		internal MethodInfo TearDownFeature;
		internal bool direct;
		internal enum MT : sbyte
		{
			Type,
			Method
		};

		internal Inter(Inter inter)
		{
			this.obj = inter.obj;
			this.type = inter.type;
			this.TearDownFeature = inter.TearDownFeature;
			this.direct = Convert.ToBoolean(inter.direct);
		}

		internal Inter(Object obj, Type type, MT m, MethodInfo TearDownFeature)
		{
			this.obj = obj;
			this.type = type;
			this.TearDownFeature = TearDownFeature;
			this.direct = Convert.ToBoolean(m);
		}
	}

	internal class SequentialRunner : IRunner
	{
		public void Run(Object typeObject, MethodInfo testMethod, Type type,
			List<string> attrs, BackgroundWorker bw, SystemState ss, int count = 0,
			MethodInfo TearDownFeature = null)
		{
			IndividualTestState its = new IndividualTestState();
			its.NameSpace = type.FullName;
			its.Attributes = attrs;
			its.TestName = testMethod.Name;
			its.StartTime = DateTime.Now.Ticks;
			try
			{
				testMethod.Invoke(typeObject, null);
				its.EndTime = DateTime.Now.Ticks;
				its.Result = true;
				its.ThrownException = null;
				Console.WriteLine("\n***{0}: Passed***\n",
					its.TestName);
			}
			catch (Exception e)
			{
				its.EndTime = DateTime.Now.Ticks;
				its.Result = false;
				its.ThrownException = e;

				if (ss == SystemState.Initial)
				{
					if (its.IsFailure)
						GlobalTestStates.IncrementFailure();
					else
						GlobalTestStates.IncrementError();
				}
				else if (ss == SystemState.Repeat)
				{
					if (its.IsFailure)
						GlobalTestStates.IncrementReFailure();
					else
						GlobalTestStates.IncrementReError();
				}

				if (GlobalTestStates.onlyOnce)
				{
					if (its.IsTimeOut)
					{
						if (GlobalTestStates.repeatBook.ContainsKey(type.FullName))
						{
							GlobalTestStates.repeatBook[type.FullName].testMethods.
								Add(testMethod);
						}
						else
						{
							GlobalTestStates.repeatBook.Add(type.FullName, new ReRunCase(type,
								new List<MethodInfo>() { testMethod }, type.FullName, attrs, TearDownFeature));
						}
					}
				}

				if (GlobalTestStates.ResultSet.ContainsKey(type.FullName))
				{
					if (GlobalTestStates.ResultSet[type.FullName].itfs.Success)
					{
						GlobalTestStates.ResultSet[type.FullName].itfs.Success = false;
					}
				}
				Console.WriteLine("\n***{0}: Failed***\n",
					its.NameSpace, its.TestName);
			}

			if (GlobalTestStates.ResultSet.ContainsKey(type.FullName))
			{
				GlobalTestStates.ResultSet[type.FullName].its.Add(its);
			}

			GlobalTestStates.DecrementScenarioCount();
			Console.WriteLine("\n***Remaining Scenarios: {0}***\n", GlobalTestStates.GetScenarioCount);
			if (!Convert.ToBoolean(GlobalTestStates.GetScenarioCount))
			{
				if (TearDownFeature != null)
				{
					TearDownFeature.Invoke(typeObject, null);
				}
				if (GlobalTestStates.ResultSet.ContainsKey(type.FullName))
				{
					GlobalTestStates.ResultSet[type.FullName].itfs.EndTick = DateTime.Now.Ticks;
				}

				while (bw.IsBusy) ;
				bw.RunWorkerAsync(new Inter(typeObject, type, Inter.MT.Type, TearDownFeature));
			}
			else
			{
				while (bw.IsBusy) ;
				bw.RunWorkerAsync(new Inter(typeObject, type, Inter.MT.Method, TearDownFeature));
			}
		}
	}

	internal class ParallelRunner : IRunner
	{
		public void Run(Object typeObject, MethodInfo testMethod, Type type,
			List<string> attrs, BackgroundWorker bw, SystemState ss, int count = 0,
			MethodInfo TearDownFeature = null)
		{
			Task finalContinuation = null;
			Task task = Task.Factory.StartNew((Object obj) =>
			{
				IndividualTestState its = obj as IndividualTestState;
				while (bw.IsBusy) ;
				bw.RunWorkerAsync(new Inter(typeObject, type,
					Inter.MT.Method, TearDownFeature));
				testMethod.Invoke(typeObject, null);
			},
			new IndividualTestState()
			{
				InvokeObject = typeObject,
				InvokeMethod = testMethod,
				NameSpace = type.FullName,
				Attributes = attrs,
				TestName = testMethod.Name,
				StartTime = DateTime.Now.Ticks
			});

			finalContinuation = task.ContinueWith(continuation =>
				continuation.Exception.Handle(ex =>
				{
					var dataFault = task.AsyncState as IndividualTestState;
					dataFault.EndTime = DateTime.Now.Ticks;
					dataFault.Result = false;
					dataFault.ThrownException = ex;

					if (ss == SystemState.Initial)
					{
						if (dataFault.IsFailure)
							GlobalTestStates.IncrementFailure();
						else
							GlobalTestStates.IncrementError();
					}
					else if (ss == SystemState.Repeat)
					{
						if (dataFault.IsFailure)
							GlobalTestStates.IncrementReFailure();
						else
							GlobalTestStates.IncrementReError();
					}

					if (GlobalTestStates.onlyOnce)
					{
						if (GlobalTestStates.repeatBook.ContainsKey(type.FullName))
						{
							GlobalTestStates.repeatBook[type.FullName].testMethods.
								Add(testMethod);
						}
						else
						{
							GlobalTestStates.repeatBook.Add(type.FullName, new ReRunCase(type,
								new List<MethodInfo>() { testMethod }, type.FullName, attrs, TearDownFeature));
						}
					}

					Console.WriteLine("\n***{0}: Failed***\n",
						dataFault.TestName);

					if (GlobalTestStates.ResultSet.ContainsKey(type.FullName))
					{
						if (GlobalTestStates.ResultSet[type.FullName].itfs.Success)
						{
							GlobalTestStates.ResultSet[type.FullName].itfs.Success = false;
						}
						if (!GlobalTestStates.onlyOnce)
						{
							GlobalTestStates.ResultSet[type.FullName].its.Add(dataFault);
						}
					}
					return false;
				})
			, TaskContinuationOptions.OnlyOnFaulted
			);

			finalContinuation = task.ContinueWith((continuation) =>
			{
				var dataPass = task.AsyncState as IndividualTestState;
				if (dataPass != null)
				{
					dataPass.EndTime = DateTime.Now.Ticks;
					dataPass.Result = true;
					dataPass.ThrownException = null;
					Console.WriteLine("\n***{0}: Passed***\n",
						dataPass.TestName);
					if (GlobalTestStates.ResultSet.ContainsKey(type.FullName))
					{
						GlobalTestStates.ResultSet[type.FullName].its.Add(dataPass);
					}
				}
			}
			, TaskContinuationOptions.NotOnFaulted
			);

			finalContinuation.ContinueWith((continuation) =>
				{
					GlobalTestStates.DecrementScenarioCount();
					Console.WriteLine("\n***Remaining Scenarios: {0}***\n", GlobalTestStates.GetScenarioCount);
					if (!Convert.ToBoolean(GlobalTestStates.GetScenarioCount))
					{
						if (TearDownFeature != null)
							TearDownFeature.Invoke(typeObject, null);

						if (GlobalTestStates.ResultSet.ContainsKey(type.FullName))
						{
							GlobalTestStates.ResultSet[type.FullName].itfs.EndTick = DateTime.Now.Ticks;
						}

						while (bw.IsBusy) ;
						bw.RunWorkerAsync(new Inter(typeObject, type, Inter.MT.Type, TearDownFeature));
					}
				});
		}
	}
}
