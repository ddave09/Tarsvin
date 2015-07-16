//namespace RunnerHelper.Test
//{
//	using System;
//	using Microsoft.VisualStudio.TestTools.UnitTesting;
//	using Tarsvin.Runner;

//	[TestClass]
//	public class RunnerHelperTest
//	{
//		[TestClass]
//		public class IsValidProject
//		{
//			[TestMethod]
//			public void IsValidProject_Returns_True()
//			{
//				string projectNamePrefix = string.Empty;
//				Options options = new Options();
//				string projectName = "Tarsvin.Some.Specs";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsTrue(actual);
//			}

//			[TestMethod]
//			public void IsValidProject_Returns_True_When_Match_ProjectNamePrefix()
//			{
//				string projectNamePrefix = "Tarsvin";
//				Options options = new Options();
//				string projectName = "Tarsvin.Some.Specs";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsTrue(actual);
//			}

//			[TestMethod]
//			public void IsValidProject_Returns_False_When_Not_Match_ProjectNamePrefix()
//			{
//				string projectNamePrefix = "Yar";
//				Options options = new Options();
//				string projectName = "Tarsvin.Some.Specs";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsFalse(actual);
//			}

//			[TestMethod]
//			public void IsValidProject_Returns_True_When_Match_Options_Projects()
//			{
//				string projectNamePrefix = string.Empty;
//				Options options = new Options();
//				options.Projects = new string[] { "Tarsvin.Some.Specs", "Tarsvin.Some2.Specs", "Tarsvin.Some3.Specs" };
//				string projectName = "Tarsvin.Some.Specs";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsTrue(actual);
//			}

//			[TestMethod]
//			public void IsValidProject_Returns_True_When_Options_Projects_Null()
//			{
//				string projectNamePrefix = string.Empty;
//				Options options = new Options();
//				options.Projects = null;
//				string projectName = "Tarsvin.Some.Specs";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsTrue(actual);
//			}

//			[TestMethod]
//			public void IsValidProject_Returns_True_When_Options_Projects_Empty()
//			{
//				string projectNamePrefix = string.Empty;
//				Options options = new Options();
//				options.Projects = new string[] { };
//				string projectName = "Tarsvin.Some.Specs";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsTrue(actual);
//			}

//			[TestMethod]
//			public void IsValidProject_Returns_True_When_Options_Projects_All_Caps()
//			{
//				string projectNamePrefix = string.Empty;
//				Options options = new Options();
//				options.Projects = new string[] { "TARSVIN.SOME.SPECS" };
//				string projectName = "Tarsvin.Some.Specs";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsTrue(actual);
//			}

//			[TestMethod]
//			public void IsValidProject_Returns_False_When_Not_Match_Options_Projects()
//			{
//				string projectNamePrefix = string.Empty;
//				Options options = new Options();
//				options.Projects = new string[] { "Tarsvin.Some1.Specs", "Tarsvin.Some2.Specs", "Tarsvin.Some3.Specs" };
//				string projectName = "Tarsvin.Some.Specs";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsFalse(actual);
//			}

//			[TestMethod]
//			public void IsValidProject_Returns_True_When_ProjectName_Ends_With_ProjectNameSuffix()
//			{
//				string projectNamePrefix = string.Empty;
//				Options options = new Options();
//				options.ProjectSuffix = "SPECS";
//				string projectName = "Tarsvin.Some.Specs";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsTrue(actual);
//			}

//			[TestMethod]
//			public void IsValidProject_Returns_True_When_ProjectName_Equals_ProjectNameSuffix()
//			{
//				string projectNamePrefix = string.Empty;
//				Options options = new Options();
//				options.ProjectSuffix = "Tarsvin.Some.Specs";
//				string projectName = "Tarsvin.Some.Specs";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsTrue(actual);
//			}

//			[TestMethod]
//			public void IsValidProject_Returns_False_When_ProjectName_Not_Ends_With_Specs()
//			{
//				string projectNamePrefix = string.Empty;
//				Options options = new Options();
//				options.ProjectSuffix = "SPECS";
//				string projectName = "Tarsvin.Some";

//				bool actual = RunnerHelper.IsValidProject(projectNamePrefix, options, projectName);

//				Assert.IsFalse(actual);
//			}
//		}
//	}
//}
