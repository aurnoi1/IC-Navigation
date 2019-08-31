using IC.Tests.App.Poms.Appium.POMs;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface INavigables
    {
        PomBlue ViewBlue { get; }
        PomMenu ViewMenu { get; }
        PomRed ViewRed { get; }
        PomYellow ViewYellow { get; }
    }
}