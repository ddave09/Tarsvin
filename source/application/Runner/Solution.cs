using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    public class Solution
    {
        //internal class SolutionParser
        //Name: Microsoft.Build.Construction.SolutionParser
        //Assembly: Microsoft.Build, Version=4.0.0.0

        static readonly Type parser;
        static readonly PropertyInfo reader;
        static readonly MethodInfo parserMethodInfo;
        static readonly PropertyInfo parserPropertyInfo;

        static Solution()
        {
            parser = Type.GetType("Microsoft.Build.Construction.SolutionParser, Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false, false);
            if (parser != null)
            {
                reader = parser.GetProperty("SolutionReader", BindingFlags.NonPublic | BindingFlags.Instance);
                parserPropertyInfo = parser.GetProperty("Projects", BindingFlags.NonPublic | BindingFlags.Instance);
                parserMethodInfo = parser.GetMethod("ParseSolution", BindingFlags.NonPublic | BindingFlags.Instance);
            }
        }

        public List<SolutionProject> Projects { get; private set; }

        public Solution(string solutionFileName)
        {
            if (parser == null)
            {
                throw new InvalidOperationException("Can not find type 'Microsoft.Build.Construction.SolutionParser' are you missing a assembly reference to 'Microsoft.Build.dll'?");
            }
            var solutionParser = parser.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).First().Invoke(null);
            using (var streamReader = new StreamReader(solutionFileName))
            {
                reader.SetValue(solutionParser, streamReader, null);
                parserMethodInfo.Invoke(solutionParser, null);
            }
            var projects = new List<SolutionProject>();
            var array = (Array)parserPropertyInfo.GetValue(solutionParser, null);
            for (int i = 0; i < array.Length; i++)
            {
                projects.Add(new SolutionProject(array.GetValue(i)));
            }
            this.Projects = projects;
        }
    }

    [DebuggerDisplay("{ProjectName}, {RelativePath}, {ProjectGuid}")]
    public class SolutionProject
    {
        static readonly Type solutionProject;
        static readonly PropertyInfo projectName;
        static readonly PropertyInfo relativePath;
        static readonly PropertyInfo projectGuid;
        static readonly PropertyInfo projectType;

        static SolutionProject()
        {
            solutionProject = Type.GetType("Microsoft.Build.Construction.ProjectInSolution, Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false, false);
            if (solutionProject != null)
            {
                projectName = solutionProject.GetProperty("ProjectName", BindingFlags.NonPublic | BindingFlags.Instance);
                relativePath = solutionProject.GetProperty("RelativePath", BindingFlags.NonPublic | BindingFlags.Instance);
                projectGuid = solutionProject.GetProperty("ProjectGuid", BindingFlags.NonPublic | BindingFlags.Instance);
                projectType = solutionProject.GetProperty("ProjectType", BindingFlags.NonPublic | BindingFlags.Instance);
            }
        }

        public string ProjectName { get; private set; }
        public string RelativePath { get; private set; }
        public string ProjectGuid { get; private set; }
        public string ProjectType { get; private set; }

        public SolutionProject(object solutionProject)
        {
            this.ProjectName = projectName.GetValue(solutionProject, null) as string;
            this.RelativePath = relativePath.GetValue(solutionProject, null) as string;
            this.ProjectGuid = projectGuid.GetValue(solutionProject, null) as string;
            this.ProjectType = projectType.GetValue(solutionProject, null).ToString();
        }
    }

}
