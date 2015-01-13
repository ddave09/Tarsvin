namespace FPBRTest
{
    using System;
using StepBinder;

[CustomAttributes.FixtureAttr("CustomerSite")]
[CustomAttributes.FixtureAttr("CustomerSiteLogin")]
public class CustomerSiteLogin
{
	public CustomerSiteLogin()
	{
		FunctionBinder.CallBeforeFeature("CustomerSite", "", "CustomerSiteLogin");
	}

	[CustomAttributes.FixtureEndAttr()]
	public void FeatureTearDown()
	{
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin");
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("CustomerSiteLogin")]
	public void LoginWithEmptyPassword()
	{
		Object obj= FunctionBinder.CallBeforeScenario("CustomerSite", "", "CustomerSiteLogin", "4. Login with empty password", "CustomerSite.CustomerSiteLogin");
		FunctionBinder.Given("CustomerSite", "", "CustomerSiteLogin", "GivenIAmOnTheLoginPage", obj);
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterInvalidUsername", obj);
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterEmptyPassword", obj);
		FunctionBinder.When("CustomerSite", "", "CustomerSiteLogin", "WhenILogin", obj);
		FunctionBinder.Then("CustomerSite", "", "CustomerSiteLogin", "ThenIShouldSeeADeniedErrorMessage", obj);
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", obj);
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("CustomerSiteLogin")]
	public void LoginWithEmptyUsername()
	{
		Object obj= FunctionBinder.CallBeforeScenario("CustomerSite", "", "CustomerSiteLogin", "5. Login with empty username", "CustomerSite.CustomerSiteLogin");
		FunctionBinder.Given("CustomerSite", "", "CustomerSiteLogin", "GivenIAmOnTheLoginPage", obj);
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterEmptyUsername", obj);
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterInvalidPassword", obj);
		FunctionBinder.When("CustomerSite", "", "CustomerSiteLogin", "WhenILogin", obj);
		FunctionBinder.Then("CustomerSite", "", "CustomerSiteLogin", "ThenIShouldSeeADeniedErrorMessage", obj);
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", obj);
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("CustomerSiteLogin")]
	public void LoginWithEmptyUsernameAndInvalidPassword()
	{
		Object obj= FunctionBinder.CallBeforeScenario("CustomerSite", "", "CustomerSiteLogin", "6. Login with empty username and invalid password", "CustomerSite.CustomerSiteLogin");
		FunctionBinder.Given("CustomerSite", "", "CustomerSiteLogin", "GivenIAmOnTheLoginPage", obj);
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterEmptyUsername", obj);
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterInvalidPassword", obj);
		FunctionBinder.When("CustomerSite", "", "CustomerSiteLogin", "WhenILogin", obj);
		FunctionBinder.Then("CustomerSite", "", "CustomerSiteLogin", "ThenIShouldSeeADeniedErrorMessage", obj);
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", obj);
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("CustomerSiteLogin")]
	public void LoginWithEmptyPasswordAndInvalidPassword()
	{
		Object obj= FunctionBinder.CallBeforeScenario("CustomerSite", "", "CustomerSiteLogin", "7. Login with empty password and invalid password", "CustomerSite.CustomerSiteLogin");
		FunctionBinder.Given("CustomerSite", "", "CustomerSiteLogin", "GivenIAmOnTheLoginPage", obj);
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterEmptyUsername", obj);
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterInvalidPassword", obj);
		FunctionBinder.When("CustomerSite", "", "CustomerSiteLogin", "WhenILogin", obj);
		FunctionBinder.Then("CustomerSite", "", "CustomerSiteLogin", "ThenIShouldSeeADeniedErrorMessage", obj);
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", obj);
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("CustomerSiteLogin")]
	public void LoginWithTooManyInvalidAttempts()
	{
		Object obj= FunctionBinder.CallBeforeScenario("CustomerSite", "", "CustomerSiteLogin", "8. Login with too many invalid attempts", "CustomerSite.CustomerSiteLogin");
		FunctionBinder.Given("CustomerSite", "", "CustomerSiteLogin", "GivenIAmOnTheLoginPage", obj);
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIAttemptToLoginWithTooManyInvalidAttempts", obj);
		FunctionBinder.Then("CustomerSite", "", "CustomerSiteLogin", "ThenIShouldBeLockedOut", obj);
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", obj);
	}
}
}