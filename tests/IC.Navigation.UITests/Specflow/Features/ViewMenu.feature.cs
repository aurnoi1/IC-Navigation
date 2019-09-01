// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace IC.Navigation.UITests.Specflow.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.Collection("UITests")]
    [Xunit.TraitAttribute("Category", "xunit:collection(UITests)")]
    public partial class ViewMenuFeature : Xunit.IClassFixture<ViewMenuFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "ViewMenu.feature"
#line hidden
        
        public ViewMenuFeature(ViewMenuFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ViewMenu", "\tI want to validate that controls and features are correctly implemented", ProgrammingLanguage.CSharp, new string[] {
                        "xunit:collection(UITests)"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 5
#line 6
    testRunner.Given("The application under test has been started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
    testRunner.And("The \"menu page\" has been opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="The controls should be displayed in \"menu page\"")]
        [Xunit.TraitAttribute("FeatureTitle", "ViewMenu")]
        [Xunit.TraitAttribute("Description", "The controls should be displayed in \"menu page\"")]
        [Xunit.TraitAttribute("Category", "view_menu")]
        public virtual void TheControlsShouldBeDisplayedInMenuPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The controls should be displayed in \"menu page\"", null, new string[] {
                        "view_menu"});
#line 10
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "usage_name"});
            table2.AddRow(new string[] {
                        "title"});
            table2.AddRow(new string[] {
                        "button to open the blue page"});
            table2.AddRow(new string[] {
                        "button to open the red page"});
#line 11
    testRunner.Then("The following controls should be displayed in the current page:", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="The \"Not Implemented\" should be not displayed")]
        [Xunit.TraitAttribute("FeatureTitle", "ViewMenu")]
        [Xunit.TraitAttribute("Description", "The \"Not Implemented\" should be not displayed")]
        [Xunit.TraitAttribute("Category", "view_menu")]
        public virtual void TheNotImplementedShouldBeNotDisplayed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The \"Not Implemented\" should be not displayed", null, new string[] {
                        "view_menu"});
#line 19
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
this.FeatureBackground();
#line 20
   testRunner.Then("The control \"not implemented\" should not be displayed in the current page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ViewMenuFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ViewMenuFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
