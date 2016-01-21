namespace Tarsvin.FPBRTest.Specs.Features
{
using System;
using Tarsvin.StepBinder;

[Tarsvin.CustomAttributes.FixtureAttr("FPBRTest")]
[Tarsvin.CustomAttributes.FixtureAttr("Search1")]
public class FPBRTestSearch1
{
	public FPBRTestSearch1()
	{
		FunctionBinder.CallBeforeFeature("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch1");
	}

	[Tarsvin.CustomAttributes.FixtureEndAttr()]
	public void FeatureTearDown()
	{
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch1");
	}

	[Tarsvin.CustomAttributes.CaseAttr("FPBRTest")]
	[Tarsvin.CustomAttributes.CaseAttr("Search1")]
	[Tarsvin.CustomAttributes.CaseAttr("Smoke")]
	public void SearchForTestPipe()
	{
		Object obj= FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch1", "1. Search for TestPipe", "FPBRTest.Search1.Smoke");
		FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch1", "GivenIAmOnTheSearchPage", obj);
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch1", obj);
	}

	[Tarsvin.CustomAttributes.CaseAttr("FPBRTest")]
	[Tarsvin.CustomAttributes.CaseAttr("Search1")]
	[Tarsvin.CustomAttributes.CaseAttr("Smoke")]
	public void SearchForSomething()
	{
		Object obj= FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch1", "2. Search for Something", "FPBRTest.Search1.Smoke");
		FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch1", "GivenIAmOnTheSearchPage", obj);
		FunctionBinder.Then("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch1", "ThenFail", obj);
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch1", obj);
	}
}
}