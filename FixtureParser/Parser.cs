namespace FixtureParser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.Build.Construction;
    using Microsoft.Build.Evaluation;
    using System.Collections;

    public class Cleanup
    {
        public void CleanSolution(SolutionManger sim, FileManger fim, Project p, string fixtureFilePath, string fileName)
        {
            fim.DeleteFile(fixtureFilePath);
            sim.RemoveFileFromProject(p, fileName);
        }
    }

    public class SolutionManger
    {
        public Project GetProject(string pName)
        {
            // test_nunit_test to test
            return new Project(@"C:\_Automation\test\source\application\SunGard.PNE.Test." + pName + @".Specs\SunGard.PNE.Test." + pName + @".Specs.csproj");
        }

        public void AddFileToProject(Project p, string fileName)
        {
            ICollection<ProjectItem> items = p.GetItems("Compile");
            foreach (ProjectItem pi in items)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(pi.EvaluatedInclude, fileName))
                    goto Finish;
            }
            p.AddItem("Compile", fileName);
        Finish:
            p.Save();
        }

        public void RemoveFileFromProject(Project p, string fileName)
        {
            List<ProjectItem> pis = new List<ProjectItem>();
            ICollection<ProjectItem> items = p.GetItems("Compile");
            foreach (ProjectItem pi in items)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(pi.EvaluatedInclude, fileName))
                {
                    pis.Add(pi);
                }
            }
            if (Convert.ToBoolean(pis.Count))
            {
                p.RemoveItems(pis.AsEnumerable());
            }
            p.Save();
        }
    }

    public class FileManger
    {
        public void DeleteFile(string fixtureFilePath)
        {
            File.Delete(Path.GetDirectoryName(fixtureFilePath) + @"\" + fixtureFilePath.Split('\\').Last().Replace('.', '_') + ".cs");
        }

        public void WriteFile(string fixtureFilePath, string data)
        {
            File.WriteAllText(Path.GetDirectoryName(fixtureFilePath) + @"\" + fixtureFilePath.Split('\\').Last().Replace('.', '_') + ".cs", data);
        }

        public string CastFileName(string fName)
        {
            return fName.Replace(".feature", "_feature.cs");
        }

        public IEnumerator TextToEnumerator(string fixtureFilePath)
        {
            return File.ReadAllLines(fixtureFilePath).GetEnumerator();
        }
    }

    public class Parser
    {
        Dictionary<string, string> modifiers = null;

        public Parser()
        {
            modifiers = new Dictionary<string, string>();
            modifiers.Add("public", "public");
            modifiers.Add("class", "class");
            modifiers.Add("void", "void");
            modifiers.Add("virtual", "virtual");
            modifiers.Add("partial", "partial");
            modifiers.Add("static", "static");
            modifiers.Add("sealed", "sealed");
        }

        public void Parse(string fixtureFilePath, string pName, string fName)
        {
            #region Instanciation
            SolutionManger sim = new SolutionManger();
            FileManger fim = new FileManger();
            Cleanup cp = new Cleanup();
            #endregion
            #region PreParsing
            Project p = sim.GetProject(pName);    
            string fileName = fim.CastFileName(fName);
            cp.CleanSolution(sim, fim, p, fixtureFilePath, fileName);      
            #endregion

            Stopwatch sw = Stopwatch.StartNew();

            #region Variable Initialization
            string nameSpaceFinder = null;
            string className = null;
            string lineName = null;
            string name = null;
            bool projectNameFlag = true;
            bool hierarchyNameFlag = true;
            string hierarchyName = null;
            bool isFeatureIgnoreAttr = true;
            bool skipFound = false;
            bool backgroundExists = false;
            List<string> attrs = new List<string>();
            string attributeString = null;
            string writeString = string.Empty;
            string previousToken = string.Empty;
            string currentStep = string.Empty; // (Given|When|Then)
            string previousParentToken = string.Empty;
            string line = string.Empty;
            #endregion

            var enumerator = fim.TextToEnumerator(fixtureFilePath);

            #region Main Loop
            while (enumerator.MoveNext())
            {
                string lineToken = string.Empty;
                line = (string)enumerator.Current;
                if (line == string.Empty)
                    continue;
                string[] tokens = line.Split();
                
                #region ProcessTokens
                foreach (string token in tokens)
                {
                    if (token == string.Empty)
                        continue;

                    if (lineToken != string.Empty)
                        goto merger;

                    if (StringComparer.OrdinalIgnoreCase.Equals(token.First(), '#'))
                    {
                        break;
                    }

                    if (StringComparer.OrdinalIgnoreCase.Equals(token.First(), '@'))
                    {
                        skipFound = false;
                        string sub = token.Substring(1);
                        if (StringComparer.OrdinalIgnoreCase.Equals(token.Substring(1), "Ignore") ||
                            StringComparer.OrdinalIgnoreCase.Equals(token.Substring(1), "Incomplete") ||
                            StringComparer.OrdinalIgnoreCase.Equals(token.Substring(1), "Manual"))
                        {
                            attrs.Clear();
                            skipFound = true;
                            if (isFeatureIgnoreAttr)
                            {
                                writeString = null;
                                isFeatureIgnoreAttr = false;
                                goto FinishProgram;
                            }
                            break;
                        }
                        if (projectNameFlag)
                        {
                            nameSpaceFinder = tokens[0].Substring(1);
                            projectNameFlag = false;
                        }

                        attrs.Add(token.Substring(1));
                        isFeatureIgnoreAttr = false;
                        continue;
                    }

                    if (hierarchyNameFlag)
                    {
                        for (int i = 1; i < attrs.Count - 1; i++)
                        {
                            hierarchyName += ".";
                            hierarchyName += attrs[i];
                        }
                        writeString += "namespace SunGard.PNE." + nameSpaceFinder + ".Specs.Features" + hierarchyName + "\r\n" + "{" + "\r\n";
                        writeString += "using System;\r\n";
                        writeString += "using StepBinder;\r\n\r\n";
                        //writeString += "using SunGard.PNE.Test." + nameSpaceFinder + ".Specs.Steps;\r\n\r\n";
                        hierarchyNameFlag = false;
                    }


                    if (skipFound)
                        break;

                    if (StringComparer.OrdinalIgnoreCase.Equals(token, "given"))
                    {
                        previousToken = "step";
                        lineToken = "step";
                        currentStep = "Given";
                        if (hierarchyName != null)
                            writeString += "\t\t" + "FunctionBinder." + token + "(\"" + nameSpaceFinder + "\", \"" + hierarchyName.Substring(1) + "\", \"" + className + "\", \"" + token;
                        else
                            writeString += "\t\t" + "FunctionBinder." + token + "(\"" + nameSpaceFinder + "\", \"\", \"" + className + "\", \"" + token;
                        continue;
                    }
                    if (StringComparer.OrdinalIgnoreCase.Equals(token, "when"))
                    {
                        previousToken = "step";
                        lineToken = "step";
                        currentStep = "When";
                        if (hierarchyName != null)
                            writeString += "\t\t" + "FunctionBinder." + token + "(\"" + nameSpaceFinder + "\", \"" + hierarchyName.Substring(1) + "\", \"" + className + "\", \"" + token;
                        else
                            writeString += "\t\t" + "FunctionBinder." + token + "(\"" + nameSpaceFinder + "\", \"\", \"" + className + "\", \"" + token;
                        continue;
                    }
                    if (StringComparer.OrdinalIgnoreCase.Equals(token, "then"))
                    {
                        previousToken = "step";
                        lineToken = "step";
                        currentStep = "Then";
                        if (hierarchyName != null)
                            writeString += "\t\t" + "FunctionBinder." + token + "(\"" + nameSpaceFinder + "\", \"" + hierarchyName.Substring(1) + "\", \"" + className + "\", \"" + token;
                        else
                            writeString += "\t\t" + "FunctionBinder." + token + "(\"" + nameSpaceFinder + "\", \"\", \"" + className + "\", \"" + token;
                        continue;
                    }
                    if (StringComparer.OrdinalIgnoreCase.Equals(token, "and"))
                    {
                        previousToken = "step";
                        lineToken = "step";
                        if (hierarchyName != null)
                            writeString += "\t\t" + "FunctionBinder." + token + "(\"" + nameSpaceFinder + "\", \"" + hierarchyName.Substring(1) + "\", \"" + className + "\", \"" + currentStep;
                        else
                            writeString += "\t\t" + "FunctionBinder." + token + "(\"" + nameSpaceFinder + "\", \"\", \"" + className + "\", \"" + currentStep;
                        continue;
                    }
                    if (StringComparer.OrdinalIgnoreCase.Equals(token, "but"))
                    {
                        previousToken = "step";
                        lineToken = "step";
                        if (hierarchyName != null)
                            writeString += "\t\t" + "FunctionBinder." + token + "(\"" + nameSpaceFinder + "\", \"" + hierarchyName.Substring(1) + "\", \"" + className + "\", \"" + currentStep;
                        else
                            writeString += "\t\t" + "FunctionBinder." + token + "(\"" + nameSpaceFinder + "\", \"\", \"" + className + "\", \"" + currentStep;
                        continue;
                    }
                    if (StringComparer.OrdinalIgnoreCase.Equals(token, "scenario:"))
                    {
                        if (StringComparer.OrdinalIgnoreCase.Equals(previousToken, "step") && !StringComparer.OrdinalIgnoreCase.Equals(previousParentToken, "background"))
                        {
                            if (hierarchyName != null)
                                writeString += "\t\t" + "FunctionBinder.CallAfterX(\"" + nameSpaceFinder + "\", \"" + hierarchyName.Substring(1) + "\", \"" + className + "\", obj);" + "\r\n";
                            else
                                writeString += "\t\t" + "FunctionBinder.CallAfterX(\"" + nameSpaceFinder + "\", \"" + "\", \"" + className + "\", obj);" + "\r\n";
                            writeString += "\t" + "}" + "\r\n" + "\r\n";
                        }
                        else if (StringComparer.OrdinalIgnoreCase.Equals(previousToken, "step"))
                        {
                            writeString += "\t" + "}" + "\r\n" + "\r\n";
                        }
                        lineToken = "scenario";
                        previousParentToken = "scenario";
                        foreach (string attr in attrs)
                        {
                            writeString += "\t[CustomAttributes.CaseAttr(\"" + attr + "\")]\r\n";
                            attributeString += attr + ".";
                        }

                        attrs.Clear();
                        writeString += "\t" + modifiers["public"] + " " + modifiers["void"] + " ";
                        continue;
                    }
                    if (StringComparer.OrdinalIgnoreCase.Equals(token, "feature:"))
                    {
                        previousParentToken = "feature";
                        previousToken = "feature";
                        lineToken = "feature";
                        foreach (string attr in attrs)
                            writeString += "[CustomAttributes.FixtureAttr(\"" + attr + "\")]\r\n";
                        attrs.Clear();
                        writeString += modifiers["public"] + " " + modifiers["class"] + " ";
                        continue;
                    }
                    if (StringComparer.OrdinalIgnoreCase.Equals(token, "background:"))
                    {
                        previousParentToken = "background";
                        previousToken = "background";
                        lineToken = "background";
                        backgroundExists = true;
                        attrs.Clear();
                        writeString += "\t" + modifiers["public"] + " " + modifiers["void"] + " " + "FeatureBackground";
                        continue;
                    }
                merger:
                    if (lineToken == string.Empty || lineToken == "background")
                        break;

                    lineName += " " + token;
                    if (Regex.IsMatch(token, @"\d."))
                        continue;
                    name += token[0].ToString().ToUpper() + token.Substring(1);
                    writeString += (token[0].ToString().ToUpper() + token.Substring(1)).Trim(',');
                }
                #endregion

                #region PostTokenProcess
                if (StringComparer.OrdinalIgnoreCase.Equals(lineToken, "feature"))
                {
                    className = name;
                    name = string.Empty;
                    lineName = string.Empty;
                    attributeString = string.Empty;
                    writeString += "\r\n" + "{" + "\r\n";
                    if (hierarchyName != null)
                    {
                        writeString += "\t" + "public " + className + "()" + "\r\n" + "\t" + "{" + "\r\n" + "\t\t" + "FunctionBinder.CallBeforeFeature(\"" + nameSpaceFinder + "\", \"" + hierarchyName.Substring(1) + "\", \"" + className + "\");"/*, \"feature\");"*/ + "\r\n" + "\t" + "}" + "\r\n" + "\r\n";
                        writeString += "\t" + "[CustomAttributes.FixtureEndAttr()]" + "\r\n";
                        writeString += "\t" + "public void " + "FeatureTearDown" + "()" + "\r\n" + "\t" + "{" + "\r\n" + "\t\t" + "FunctionBinder.CallAfterX(\"" + nameSpaceFinder + "\", \"" + hierarchyName.Substring(1) + "\", \"" + className + "\");"/*, \"feature\");"*/ + "\r\n" + "\t" + "}" + "\r\n" + "\r\n";
                    }
                    else
                    {
                        writeString += "\t" + "public " + className + "()" + "\r\n" + "\t" + "{" + "\r\n" + "\t\t" + "FunctionBinder.CallBeforeFeature(\"" + nameSpaceFinder + "\", \"" + "\", \"" + className + "\");"/*, \"feature\");"*/ + "\r\n" + "\t" + "}" + "\r\n" + "\r\n";
                        writeString += "\t" + "[CustomAttributes.FixtureEndAttr()]" + "\r\n";
                        writeString += "\t" + "public void " + "FeatureTearDown" + "()" + "\r\n" + "\t" + "{" + "\r\n" + "\t\t" + "FunctionBinder.CallAfterX(\"" + nameSpaceFinder + "\", \"" + "\", \"" + className + "\");"/*, \"feature\");"*/ + "\r\n" + "\t" + "}" + "\r\n" + "\r\n";
                    }

                }
                else if (StringComparer.OrdinalIgnoreCase.Equals(lineToken, "background"))
                {
                    writeString += "(Object obj)" + "\r\n\t" + "{" + "\r\n";
                    name = string.Empty;
                    lineName = string.Empty;
                    attributeString = string.Empty;
                }

                else if (StringComparer.OrdinalIgnoreCase.Equals(lineToken, "scenario"))
                {

                    writeString += "(" + ")" + "\r\n\t" + "{" + "\r\n" + "\t\t";
                    if (hierarchyName != null)
                        writeString += "Object" + " obj" + "= " + "FunctionBinder.CallBeforeScenario(\"" + nameSpaceFinder + "\", \"" + hierarchyName.Substring(1) + "\", \"" + className + "\", \"" + lineName.Trim() + "\", \"" + attributeString.Trim('.') + "\");"/*, \"scenario\");"*/ + "\r\n";
                    else
                        writeString += "Object" + " obj" + "= " + "FunctionBinder.CallBeforeScenario(\"" + nameSpaceFinder + "\", \"" + "\", \"" + className + "\", \"" + lineName.Trim() + "\", \"" + attributeString.Trim('.') + "\");"/*, \"scenario\");"*/ + "\r\n";

                    name = string.Empty;
                    lineName = string.Empty;
                    attributeString = string.Empty;

                    if (backgroundExists)
                    {
                        writeString += "\t\t";
                        writeString += "this.FeatureBackground(obj);" + "\r\n";
                    }
                }

                else if (StringComparer.OrdinalIgnoreCase.Equals(lineToken, "step"))
                {
                    writeString += "\", obj)" + ";" + "\r\n";
                    name = string.Empty;
                    lineName = string.Empty;
                }
                #endregion
            }

            #region EndLastScenario

            writeString += "\t\t";
            if (hierarchyName != null)
                writeString += "FunctionBinder.CallAfterX(\"" + nameSpaceFinder + "\", \"" + hierarchyName.Substring(1) + "\", \"" + className + "\", obj);" + "\r\n";
            else
                writeString += "FunctionBinder.CallAfterX(\"" + nameSpaceFinder + "\", \"" + "\", \"" + className + "\", obj);" + "\r\n";
            writeString += "\t" + "}" + "\r\n" + "}" + "\r\n" + "}";

            #endregion
            fim.WriteFile(fixtureFilePath, writeString);
            sim.AddFileToProject(p, fileName);
        FinishProgram:
            Console.WriteLine("Elapsed Time: {0}", sw.ElapsedMilliseconds);
            #endregion
        }
     
    }
}
