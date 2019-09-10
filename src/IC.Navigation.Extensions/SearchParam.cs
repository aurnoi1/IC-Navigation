using IC.Navigation.Extensions.Enums;
using IC.Navigation.Extensions.Interfaces;

namespace IC.Navigation.Extensions
{
    public class SearchParam : ISearchParam
    {
        public SearchParam(WDLocators locator, string value)
        {
            Locator = locator;
            Value = value;
        }

        /// <summary>
        /// Name of the parameter.
        /// </summary>
        public WDLocators Locator { get; set; }

        /// <summary>
        /// Value of the parameter.
        /// </summary>
        public string Value { get; set; }
    }
}