using OpenQA.Selenium.Appium.Windows;

namespace IC.Tests.App.UIAccessibility.Appium.Interfaces
{
    public interface IViewDefBlue
    {
        WindowsElement UIBtnBack { get; }
        WindowsElement UIBtnOpenYellowView { get; }
        WindowsElement UITitle { get; }
    }
}