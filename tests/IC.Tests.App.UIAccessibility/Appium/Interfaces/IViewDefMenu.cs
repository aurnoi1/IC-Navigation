using OpenQA.Selenium.Appium.Windows;

namespace IC.Tests.App.UIAccessibility.Appium.Interfaces
{
    public interface IViewDefMenu
    {
        WindowsElement UIBtnNotImplemented { get; }
        WindowsElement UIBtnOpenBlueView { get; }
        WindowsElement UIBtnOpenRedView { get; }
        WindowsElement UIBtnOpenYellowView { get; }
        WindowsElement UITitle { get; }
        WindowsElement UITxtBoxImportantMessage { get; }
    }
}