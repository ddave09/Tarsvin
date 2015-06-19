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
			string actualPath = @"../../FPBRTest.sln";
			string slnDirPath = Path.GetDirectoryName(actualPath) + @"\";
			Solution sln = new Solution(actualPath);
			string findTestProj = "specs";
			List<DllInfo> addRefList = new List<DllInfo>();
			foreach (SolutionProject sp in sln.Projects)
			{
				string[] pName = sp.ProjectName.Split('.');
				if (StringComparer.OrdinalIgnoreCase.Equals(pName.Last<string>(), findTestProj))
				{
					DllInfo info = new DllInfo();
					info.path = slnDirPath + Path.GetDirectoryName(sp.RelativePath) + @"\bin\debug\" + sp.RelativePath.Split('\\').Last<string>().Replace(".csproj", ".dll");
					addRefList.Add(info);
				}
			}
			new StepLoader(addRefList);
		}

		[TestMethod]
		public void StepLoaderStepTest()
		{
			//SearchFeature csl = new SearchFeature();
			//csl.UserClicksOnLogoutLink();
		}
	}
}