using IC.Navigation.Interfaces;

namespace IC.Tests.App.UIAccessibility.Appium.Interfaces
{
    public interface INavigables
    {
        IViewBlue ViewBlue { get; }
        IViewMenu ViewMenu { get; }
        IViewRed ViewRed { get; }
        IViewYellow ViewYellow { get; }
    }
}