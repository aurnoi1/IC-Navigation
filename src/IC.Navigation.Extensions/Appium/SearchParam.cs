using IC.Navigation.Extensions.Appium.Interfaces;

namespace IC.Navigation.Extensions.Appium
{
    public class SearchParam : ISearchParam
    {
        public SearchParam(string locator, string value)
        {
            Locator = locator;
            Value = value;
        }

        /// <summary>
        /// Locator to find the WindowsElement.
        /// </summary>
        public string Locator { get; set; }

        /// <summary>
        /// Value of the parameter.
        /// </summary>
        public string Value { get; set; }
    }
}