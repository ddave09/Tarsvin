using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarsvin.StepBinder;

namespace Tarsvin.Runner
{
	public class StepLoader
	{
		public StepLoader(List<DllInfo> list)
		{
			Assembly runtimeAssembly = null;
			foreach (DllInfo dllinfo in list)
			{
				if (dllinfo.path == null)
					runtimeAssembly = Assembly.GetExecutingAssembly();
				else
					runtimeAssembly = Assembly.Load(AssemblyName.GetAssemblyName(dllinfo.path));
				List<Type> types = TestTypes(runtimeAssembly.GetTypes().ToList());
				foreach (Type type in types)
				{
					List<MethodInfo> methods = type.GetMethods().ToList();
					BindMap.AddToBindMap(type.FullName, dllinfo.path, type, methods);
				}
			}
		}

		public List<Type> TestTypes(List<Type> types)
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

		public bool IsTestClass(List<CustomAttributeData> li)
		{
			foreach (CustomAttributeData attr in li)
			{
				if (StringComparer.OrdinalIgnoreCase.Equals(attr.AttributeType.ToString(), "techtalk.specflow.bindingattribute"))
				{
					return true;
				}
			}
			return false;
		}
	}
}
