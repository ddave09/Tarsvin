namespace Tarsvin.Runner
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	internal class ReRunCase
	{
		internal Type type = null;
		internal List<MethodInfo> testMethods = null;
		internal string nameSpace = string.Empty;
		internal List<string> attrs = null;
		internal MethodInfo TearDownFeature = null;

		internal ReRunCase(Type type, List<MethodInfo> testMethods, string nameSpace, List<string> attrs,
			MethodInfo TearDownFeature)
		{
			this.type = type;
			this.testMethods = testMethods;
			this.nameSpace = nameSpace;
			this.attrs = attrs;
			this.TearDownFeature = TearDownFeature;
		}
	}
}
