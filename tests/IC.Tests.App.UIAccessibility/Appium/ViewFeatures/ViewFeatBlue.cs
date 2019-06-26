using IC.Tests.App.UIAccessibility.Appium.ViewDefinitions;
using IC.Tests.App.UIAccessibility.Appium.Interfaces;

namespace IC.Tests.App.UIAccessibility.Appium.ViewFeatures
{
    public abstract class ViewFeatBlue : ViewDefBlue, IViewFeatBlue
    {
        public ViewFeatBlue(in IUIAccess session) : base(session) => this.session = session;

        private IUIAccess session;
    }
}