namespace Tarsvin.FPBRTest.Specs
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.Build.Construction;
	using Microsoft.Build.Evaluation;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Runner;
	using StepBinder;
	using Tarsvin.FPBRTest.Specs.Features;

	[TestClass]
	public class FPBR
	{
		public FPBR()
		{
			//Test does not support solution file anymore.
			//Rebuild test having only one dll in mind. As Tarsvin is not a library which exists in test project itself.
		}

		[TestMethod]
		public void StepLoaderStepTest()
		{
			//SearchFeature csl = new SearchFeature();
			//csl.UserClicksOnLogoutLink();
		}
	}
}