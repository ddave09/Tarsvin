namespace Tarsvin.FPBRTest.Specs.Features
{
	using System;
	using Tarsvin.StepBinder;

	[Tarsvin.CustomAttributes.FixtureAttr("FPBRTest")]
	[Tarsvin.CustomAttributes.FixtureAttr("Search")]
	public class FPBRTestSearch
	{
		public FPBRTestSearch()
		{
			FunctionBinder.CallBeforeFeature("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch");
		}

		[Tarsvin.CustomAttributes.FixtureEndAttr()]
		public void FeatureTearDown()
		{
			FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch");
		}

		[Tarsvin.CustomAttributes.CaseAttr("FPBRTest")]
		[Tarsvin.CustomAttributes.CaseAttr("Search")]
		[Tarsvin.CustomAttributes.CaseAttr("Smoke")]
		public void SearchForTestPipe()
		{
			Object obj = FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "1. Search for TestPipe", "FPBRTest.Search.Smoke");
			try
			{
				FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "GivenIAmOnTheSearchPage", obj);
			}
			finally
			{
				FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", obj);
			}
		}

		[Tarsvin.CustomAttributes.CaseAttr("FPBRTest")]
		[Tarsvin.CustomAttributes.CaseAttr("Search")]
		[Tarsvin.CustomAttributes.CaseAttr("Smoke")]
		public void SearchForSomething()
		{
			Object obj = FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "2. Search for Something", "FPBRTest.Search.Smoke");
			try
			{
				FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "GivenIAmOnTheSearchPage", obj);
				FunctionBinder.Then("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "ThenFail", obj);
			}
			finally
			{
				FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", obj);
			}
		}
	}
}