namespace Tarsvin.FPBRTest.Features
{
using System;
using StepBinder;

[CustomAttributes.FixtureAttr("CustomerSite")]
[CustomAttributes.FixtureAttr("Logout")]
public class CustomerSiteLogout
{
	public CustomerSiteLogout()
	{
		FunctionBinder.CallBeforeFeature("Tarsvin.FPBRTest.CustomerSite", "", "CustomerSiteLogout");
	}

	[CustomAttributes.FixtureEndAttr()]
	public void FeatureTearDown()
	{
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.CustomerSite", "", "CustomerSiteLogout");
	}

	public void FeatureBackground(Object obj)
	{
		FunctionBinder.Given("Tarsvin.FPBRTest.CustomerSite", "", "CustomerSiteLogout", "GivenUserIsLoggedInToCustomerSite", obj);
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("Logout")]
	[CustomAttributes.CaseAttr("Smoke")]
	public void UserClicksOnLogoutLink()
	{
		Object obj= FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.CustomerSite", "", "CustomerSiteLogout", "1. User clicks on logout link", "CustomerSite.Logout.Smoke");
		this.FeatureBackground(obj);
		FunctionBinder.When("Tarsvin.FPBRTest.CustomerSite", "", "CustomerSiteLogout", "WhenUserClicksOnLogoutLinkUserGoesToLoginPage", obj);
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.CustomerSite", "", "CustomerSiteLogout", obj);
	}
}
}