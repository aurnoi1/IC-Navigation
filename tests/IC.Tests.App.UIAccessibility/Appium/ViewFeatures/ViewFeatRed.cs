using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewDefinitions;

namespace IC.Tests.App.UIAccessibility.Appium.ViewFeatures
{
    public abstract class ViewFeatRed : ViewDefRed
    {
        public ViewFeatRed(in IUIAccess session) : base(session) => this.session = session;

        private IUIAccess session;
    }
}