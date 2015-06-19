namespace Tarsvin.Runner
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	internal class ReRunCase
	{
		internal Type type;
		internal List<MethodInfo> testMethods;
		internal string nameSpace;
		internal List<string> attrs;
		internal ReRunCase(Type type, List<MethodInfo> testMethods, string nameSpace, List<string> attrs)
		{
			this.type = type;
			this.testMethods = testMethods;
			this.nameSpace = nameSpace;
			this.attrs = attrs;
		}
	}
}
