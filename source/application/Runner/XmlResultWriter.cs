namespace Tarsvin.Runner
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Reflection;
	using System.Xml;

	public class XmlResultWriter
	{
		private XmlTextWriter xmlWriter;
		private TextWriter writer;
		private MemoryStream memoryStream;
		private string projectPath;

		#region Constructors
		public XmlResultWriter(string fileName, string projectPath)
		{
			this.projectPath = projectPath;
			xmlWriter = new XmlTextWriter(new StreamWriter(fileName, false, System.Text.Encoding.UTF8));
			InitializeXmlFile();
		}

		public void Terminate()
		{
			TerminateXmlFile();
		}

		public XmlResultWriter(TextWriter writer)
		{
			this.memoryStream = new MemoryStream();
			this.writer = writer;
			this.xmlWriter = new XmlTextWriter(new StreamWriter(memoryStream, System.Text.Encoding.UTF8));
		}
		#endregion

		private void InitializeXmlFile()
		{
			xmlWriter.WriteStartDocument(false);

			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.WriteComment("This file represents the results of running a test suite");

			xmlWriter.WriteStartElement("test-results");

			xmlWriter.WriteAttributeString("name", this.projectPath);
			xmlWriter.WriteAttributeString("total", GlobalTestStates.TestsRun.ToString());
			xmlWriter.WriteAttributeString("errors", 
				(GlobalTestStates.Error - (GlobalTestStates.Error - GlobalTestStates.ReError)).ToString());
			xmlWriter.WriteAttributeString("failures",
				(GlobalTestStates.FailureCount - (GlobalTestStates.FailureCount - GlobalTestStates.ReFailureCount)).ToString());
			xmlWriter.WriteAttributeString("not-run", GlobalTestStates.NotRun.ToString());
			xmlWriter.WriteAttributeString("inconclusive", GlobalTestStates.Inconclusive.ToString());
			xmlWriter.WriteAttributeString("ignored", GlobalTestStates.Ignored.ToString());
			xmlWriter.WriteAttributeString("skipped", GlobalTestStates.Skipped.ToString());
			xmlWriter.WriteAttributeString("invalid", GlobalTestStates.Invalid.ToString());

			DateTime now = DateTime.Now;
			xmlWriter.WriteAttributeString("date", XmlConvert.ToString(now, "yyyy-MM-dd"));
			xmlWriter.WriteAttributeString("time", XmlConvert.ToString(now, "HH:mm:ss"));
			WriteEnvironment();
			WriteCultureInfo();
		}

		private void WriteCultureInfo()
		{
			xmlWriter.WriteStartElement("culture-info");
			xmlWriter.WriteAttributeString("current-culture",
										   CultureInfo.CurrentCulture.ToString());
			xmlWriter.WriteAttributeString("current-uiculture",
										   CultureInfo.CurrentUICulture.ToString());
			xmlWriter.WriteEndElement();
		}

		private void WriteEnvironment()
		{
			xmlWriter.WriteStartElement("environment");
			xmlWriter.WriteAttributeString("Tarsvin-version",
										   Assembly.GetExecutingAssembly().GetName().Version.ToString());
			xmlWriter.WriteAttributeString("clr-version",
										   Environment.Version.ToString());
			xmlWriter.WriteAttributeString("os-version",
										   Environment.OSVersion.ToString());
			xmlWriter.WriteAttributeString("platform",
				Environment.OSVersion.Platform.ToString());
			xmlWriter.WriteAttributeString("cwd",
										   Environment.CurrentDirectory);
			xmlWriter.WriteAttributeString("machine-name",
										   Environment.MachineName);
			xmlWriter.WriteAttributeString("user",
										   Environment.UserName);
			xmlWriter.WriteAttributeString("user-domain",
										   Environment.UserDomainName);
			xmlWriter.WriteEndElement();
		}

		public void SaveTestResult(IndividualTestState result)
		{
			WriteResultElement(result);
		}

		private void WriteResultElement(IndividualTestState result)
		{
			StartTestElement(result);

			WriteCategoriesElement(result);

			switch (result.Result)
			{
				case true:
					break;
				case false:
					WriteFailureElement(result);
					break;
			}
			xmlWriter.WriteEndElement(); // test element
		}

		private void TerminateXmlFile()
		{
			try
			{
				xmlWriter.WriteEndElement(); // test-results
				xmlWriter.WriteEndDocument();
				xmlWriter.Flush();

				if (memoryStream != null && writer != null)
				{
					memoryStream.Position = 0;
					using (StreamReader rdr = new StreamReader(memoryStream))
					{
						writer.Write(rdr.ReadToEnd());
					}
				}

				xmlWriter.Close();
			}
			finally
			{

			}
		}

		public void StartSuiteElement(IndividualFeatureTestState itfs)
		{
			string result = string.Empty;
			string[] nameSpace = itfs.FeatureName.Split('.');
			xmlWriter.WriteStartElement("test-suite");
			xmlWriter.WriteAttributeString("type", "Assemlbly");
			xmlWriter.WriteAttributeString("name", this.projectPath);
			xmlWriter.WriteAttributeString("executed", "True");
			if (itfs.Success)
			{
				result = "Success";
				xmlWriter.WriteAttributeString("result", result);
			}
			else
			{
				result = "Failure";
				xmlWriter.WriteAttributeString("result", result);
			}
			xmlWriter.WriteAttributeString("success", itfs.Success.ToString());
			xmlWriter.WriteAttributeString("time", itfs.FeatureExecutionTime.TotalSeconds.ToString());
			xmlWriter.WriteAttributeString("asserts", "1");
			xmlWriter.WriteStartElement("results");
			foreach (string str in nameSpace)
			{
				xmlWriter.WriteStartElement("test-suite");
				xmlWriter.WriteAttributeString("type", "Namespace");
				xmlWriter.WriteAttributeString("name", str);
				xmlWriter.WriteAttributeString("executed", "True");
				xmlWriter.WriteAttributeString("result", result);
				xmlWriter.WriteAttributeString("success", itfs.Success.ToString());
				xmlWriter.WriteAttributeString("time", itfs.FeatureExecutionTime.TotalSeconds.ToString());
				xmlWriter.WriteAttributeString("asserts", "1");
				xmlWriter.WriteStartElement("results");
			}
		}

		public void EndSuiteElement(IndividualFeatureTestState itfs)
		{
			List<string> nameSpace = new List<string>(itfs.FeatureName.Split('.'));
			int twice = nameSpace.Count << 1;

			while (twice > 0)
			{
				xmlWriter.WriteEndElement();
				twice--;
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}

		private void StartTestElement(IndividualTestState result)
		{
			xmlWriter.WriteStartElement("test-case");
			xmlWriter.WriteAttributeString("name", string.Format("{0}.{1}", result.NameSpace, result.TestName));
			xmlWriter.WriteAttributeString("description", result.TestName);
			xmlWriter.WriteAttributeString("executed", "True");
			if (result.Result)
			{
				xmlWriter.WriteAttributeString("result", "Success");
			}
			else
			{
				xmlWriter.WriteAttributeString("result", "Failure");
			}

			xmlWriter.WriteAttributeString("success", result.Result.ToString());
			xmlWriter.WriteAttributeString("time", result.ExecTime.TotalSeconds.ToString());
			xmlWriter.WriteAttributeString("asserts", "1");
		}

		private void WriteCategoriesElement(IndividualTestState result)
		{
			if (result.Attributes != null && result.Attributes.Count > 0)
			{
				xmlWriter.WriteStartElement("categories");
				foreach (string attribute in result.Attributes)
				{
					xmlWriter.WriteStartElement("category");
					xmlWriter.WriteAttributeString("name", attribute.Trim('"'));
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
			}
		}

		private void WriteFailureElement(IndividualTestState result)
		{
			xmlWriter.WriteStartElement("failure");

			xmlWriter.WriteStartElement("message");
			xmlWriter.WriteCData(result.ReasonOfFailure);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("stack-trace");
			xmlWriter.WriteCData(result.ExceptionStackTrace);
			xmlWriter.WriteEndElement();

			xmlWriter.WriteEndElement();
		}
	}
}
