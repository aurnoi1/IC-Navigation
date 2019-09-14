using IC.Navigation.Extensions.Appium.WindowsDriver.Interfaces;
using IC.Navigation.Interfaces;

namespace IC.Tests.App.Poms.Appium.Interfaces
{
    public interface IFacade : INavigatorSession, INavigables, IWindowsDriverSession
    {
    }
}