using IC.Tests.App.Poms.Appium.POMs;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface INavigables
    {
        PomBlue PomBlue { get; }
        PomMenu PomMenu { get; }
        PomRed PomRed { get; }
        PomYellow PomYellow { get; }
    }
}