using IC.Tests.App.UIAccessibility.Appium.POMs;

namespace IC.Tests.App.UIAccessibility.Appium.Interfaces
{
    public interface INavigables
    {
        PomBlue ViewBlue { get; }
        PomMenu ViewMenu { get; }
        PomRed ViewRed { get; }
        PomYellow ViewYellow { get; }
    }
}