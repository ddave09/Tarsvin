namespace Tarsvin.FPBRTest.Specs.Steps
{
	using System;
	using TestPipe.Assertions;
	using TestPipe.Core;
	using TestPipe.Core.Page;
	using TestPipe.Core.Session;
	using Tarsvin.FPBRTest.Specs.Pages;
	using TestPipe.Runner;

	[Tarsvin.CustomAttributes.StepTypeAttr]
	public class FPBRTestSearch1Step
	{
		private static SessionFeature feature;
		private BasePage resultPage;
		private SessionScenario scenario;
		private string searchText;
		private SearchPage sut;

		#region Tarsvin friends
		[Tarsvin.CustomAttributes.BeforeFeature]
		public static void SetupFeature(string[] tags, string title)
		{
			feature = StepHelper.SetupFeature(tags, title);
		}

		[Tarsvin.CustomAttributes.BeforeScenario]
		public void SetupScenario(string[] tags, string fTitle, string sTitle)
		{
			scenario = StepHelper.SetupScenario(tags, fTitle, sTitle);
			this.sut = new SearchPage(this.scenario.Browser, TestSession.Environment);
		}

		[Tarsvin.CustomAttributes.AfterScenario]
		public void TearDownScenario()
		{
			StepHelper.TearDownScenario(scenario);
		}

		[Tarsvin.CustomAttributes.AfterFeature]
		public static void TearDownFeatureS()
		{
			RunnerBase.TeardownSuite();
		}
		#endregion
		
		public void GivenIAmOnTheSearchPage()
		{
			this.sut.Open();
		}
		
		public void GivenBackgroundTest()
		{

		}

		public void ThenResultsShouldBeDisplayed()
		{
			string pageState = string.Format("Page Title: {0}, Browser Title: {1}, Page Url: {2}, Browser Page: {3}", this.resultPage.Title, this.resultPage.Browser.Title, this.resultPage.PageUrl, this.resultPage.Browser.Url);
			bool isOpen = this.resultPage.IsOpen();
			Asserts.IsTrue(isOpen, pageState);
		}

		public void WhenISubmitASearch()
		{
			this.searchText = this.scenario.Data.Q;

			this.sut.Search.TypeText(this.searchText);

			//this.sut.EnterText(this.sut.Search, this.searchText);

			this.sut = new SearchPage(this.scenario.Browser, TestSession.Environment);

			//this.resultPage = this.sut.Submit(this.searchText);
		}

		public void ThenFail()
		{
			scenario.Asserts.Fail("Correct Failure");
		}
	}
}
