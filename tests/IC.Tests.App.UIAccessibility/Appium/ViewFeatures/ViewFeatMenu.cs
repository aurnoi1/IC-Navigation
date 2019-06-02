using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewDefinitions;

namespace IC.Tests.App.UIAccessibility.Appium.ViewFeatures
{
    public abstract class ViewFeatMenu : ViewDefMenu, IViewFeatMenu
    {
        public ViewFeatMenu(in IMySession session) : base(session) => this.session = session;

        private IMySession session;

        /// <summary>
        /// Enter a text in the UITxtBoxImportantMessage.
        /// </summary>
        /// <param name="text">The text to enter.</param>
        public void EnterText(string text)
        {
            UITxtBoxImportantMessage.SendKeys(text);
        }
    }
}