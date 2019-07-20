using IC.Navigation.Interfaces;
using IC.Tests.App.UIAccessibility.Appium.ViewNavigables;

namespace IC.Tests.App.UIAccessibility.Appium.Interfaces
{
    public interface INavigables
    {
        ViewBlue ViewBlue { get; }
        ViewMenu ViewMenu { get; }
        ViewRed ViewRed { get; }
        ViewYellow ViewYellow { get; }
    }
}