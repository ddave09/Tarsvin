//namespace Tarsvin.Runner
//{
//	using System;
//	using System.Collections.Generic;
//	using System.IO;
//	using System.Linq;
//	using System.Text.RegularExpressions;

//	public class RunnerHelper
//	{
//		public static DllInfo GetDllInfo(SolutionProject project, string actualPath)
//		{
//			DllInfo info = new DllInfo();
//			string solutionPath = Path.GetDirectoryName(actualPath) + @"\";
//			info.path = solutionPath + Path.GetDirectoryName(project.RelativePath) + @"\bin\debug\" + project.RelativePath.Split('\\').Last<string>().Replace(".csproj", ".dll");
//			return info;
//		}

//		public static List<DllInfo> GetDllList(Options options, string actualPath, string projectNamePrefix, Solution sln)
//		{
//			List<DllInfo> dllList = new List<DllInfo>();

//			foreach (SolutionProject project in sln.Projects)
//			{
//				if (!IsValidProject(projectNamePrefix, options, project.ProjectName))
//				{
//					continue;
//				}

//				DllInfo info = GetDllInfo(project, actualPath);
//				dllList.Add(info);
//			}

//			return dllList;
//		}

//		public static bool IsValidProject(string projectNamePrefix, Options options, string projectName)
//		{
//			if (!string.IsNullOrWhiteSpace(projectNamePrefix))
//			{
//				string pattern = string.Format("{0}.*", projectNamePrefix);

//				if (!Regex.IsMatch(projectName, pattern))
//				{
//					return false;
//				}
//			}

//			if (options.Projects != null && options.Projects.Length > 0 && !options.Projects.Any(s => s.Equals(projectName, StringComparison.OrdinalIgnoreCase)))
//			{
//				return false;
//			}

//			string projectNameSuffix = options.ProjectSuffix;

//			if (string.IsNullOrWhiteSpace(projectNameSuffix))
//			{
//				return true;
//			}

//			if (!projectName.ToLower().EndsWith(projectNameSuffix.ToLower()))
//			{
//				return false;
//			}

//			return true;
//		}
//	}
//}