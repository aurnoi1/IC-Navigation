using IC.Navigation.Extensions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Extensions.Interfaces
{
    public interface ISearchParam
    {
        /// <summary>
        /// Locator to find the WindowsElement.
        /// </summary>
        WDLocators Locator { get; set; }

        /// <summary>
        /// Value of the parameter.
        /// </summary>
        string Value { get; set; } 
    }
}
