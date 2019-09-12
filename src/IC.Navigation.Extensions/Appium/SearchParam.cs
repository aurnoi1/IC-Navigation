using IC.Navigation.Extensions.Appium.Interfaces;

namespace IC.Navigation.Extensions.Appium
{
    public class SearchParam : ISearchParam
    {
        public SearchParam()
        {
        }

        public SearchParam(string locator, string value)
        {
            Locator = locator;
            Value = value;
        }

        public string Locator { get; set; }
        public string Value { get; set; }
    }
}