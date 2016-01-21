namespace Tarsvin.FPBRTest.Specs.Features
{
using System;
using Tarsvin.StepBinder;

[Tarsvin.CustomAttributes.FixtureAttr("FPBRTest")]
[Tarsvin.CustomAttributes.FixtureAttr("Search2")]
public class FPBRTestSearch2
{
	public FPBRTestSearch2()
	{
		FunctionBinder.CallBeforeFeature("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch2");
	}

	[Tarsvin.CustomAttributes.FixtureEndAttr()]
	public void FeatureTearDown()
	{
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch2");
	}

	[Tarsvin.CustomAttributes.CaseAttr("FPBRTest")]
	[Tarsvin.CustomAttributes.CaseAttr("Search2")]
	[Tarsvin.CustomAttributes.CaseAttr("Smoke")]
	public void SearchForTestPipe()
	{
		Object obj= FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch2", "1. Search for TestPipe", "FPBRTest.Search2.Smoke");
		FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch2", "GivenIAmOnTheSearchPage", obj);
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch2", obj);
	}

	[Tarsvin.CustomAttributes.CaseAttr("FPBRTest")]
	[Tarsvin.CustomAttributes.CaseAttr("Search2")]
	[Tarsvin.CustomAttributes.CaseAttr("Smoke")]
	public void SearchForSomething()
	{
		Object obj= FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch2", "2. Search for Something", "FPBRTest.Search2.Smoke");
		FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch2", "GivenIAmOnTheSearchPage", obj);
		FunctionBinder.Then("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch2", "ThenFail", obj);
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch2", obj);
	}
}
}