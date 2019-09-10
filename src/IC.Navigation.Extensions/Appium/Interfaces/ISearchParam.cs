namespace IC.Navigation.Extensions.Appium.Interfaces
{
    public interface ISearchParam
    {
        /// <summary>
        /// Locator to find the WindowsElement.
        /// </summary>
        string Locator { get; set; }

        /// <summary>
        /// Value of the parameter.
        /// </summary>
        string Value { get; set; }
    }
}