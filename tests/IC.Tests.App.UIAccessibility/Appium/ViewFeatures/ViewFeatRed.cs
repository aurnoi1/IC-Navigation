using IC.Tests.App.UIAccessibility.Appium.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewDefinitions;

namespace IC.Tests.App.UIAccessibility.Appium.ViewFeatures
{
    public abstract class ViewFeatRed : ViewDefRed
    {
        public ViewFeatRed(in IMySession session) : base(session) => this.session = session;

        private IMySession session;
    }
}