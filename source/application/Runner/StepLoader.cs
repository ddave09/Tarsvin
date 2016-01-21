namespace Tarsvin.Runner
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Tarsvin.StepBinder;

	internal class StepLoader
	{
		internal StepLoader(DllInfo dllInfo)
		{
			Assembly runtimeAssembly = Assembly.Load(AssemblyName.GetAssemblyName(dllInfo.path));
			List<Type> types = TestTypes(runtimeAssembly.GetTypes().ToList());
			foreach (Type type in types)
			{
				List<MethodInfo> methods = type.GetMethods().ToList();
				BindMap.AddToBindMap(type.FullName, dllInfo.path, type, methods);
			}
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
				if (StringComparer.OrdinalIgnoreCase.Equals(attr.AttributeType.ToString(), "techtalk.specflow.bindingattribute")
					||
					StringComparer.OrdinalIgnoreCase.Equals(attr.AttributeType.ToString(), "Tarsvin.CustomAttributes.StepTypeAttr"))
				{
					return true;
				}
			}
			return false;
		}
	}
}
