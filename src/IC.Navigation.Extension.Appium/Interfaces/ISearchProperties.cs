using IC.Navigation.Extension.Appium.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interfaces;
using System;
using System.Threading;

namespace IC.Navigation.Extension.Appium.Interfaces
{
    public interface ISearchProperties<W> where W : IWebElement
    {
        /// <summary>
        /// The default CancellationToken to interrupt a search of the WebElement.
        /// </summary>
        CancellationToken DefaultCancellationToken { get; }

        /// <summary>
        /// The index of the WebElement in the collection of matching WebElements.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Locator to find the WebElement.
        /// </summary>
        string Locator { get; set; }

        /// <summary>
        /// Value of the Locator.
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// The WebDriver used to find the WebElement.
        /// </summary>
        IFindsByFluentSelector<W> WebDriver { get; }

        /// <summary>
        /// Search the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// The <see cref="DefaultCancellationToken"/> is mandatory when using this method.
        /// <returns>The matching WebElement.</returns>
        /// <exception cref="OperationCanceledException">Thrown when any CancellationToken is cancelled.</exception>
        /// <exception cref="UninitializedDefaultCancellationTokenException">Thrown when no CancellationToken is initialized.</exception>
        W Find();

        /// <summary>
        /// Search the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens used to stop waiting for the control to be found.
        /// They will be linked to the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The matching WebElement.</returns>
        /// <exception cref="OperationCanceledException">Thrown when any CancellationToken is cancelled.</exception>
        W Find(params CancellationToken[] cancellationTokens);

        /// <summary>
        /// Search the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The matching WebElement.</returns>
        /// <exception cref="TimeoutException">Thrown when any timeout is reached before WebElement is found.</exception>
        W Find(TimeSpan timeout);

        /// <summary>
        /// Get the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// <see cref="DefaultCancellationToken"/> will be used if defined.
        /// </summary>
        /// <returns>The matching WebElement, otherwise <c>null</c>.</returns>
        W Get();

        /// <summary>
        /// Get the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="timeout">The maximum amount of time to wait for the control to be found.
        /// This timeout will run in concurence of the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The matching WebElement, otherwise <c>null</c>.</returns>
        W Get(params CancellationToken[] cancellationTokens);

        /// <summary>
        /// Get the WebElement of type <typeparamref name="W"/> matching the SearchProperties.
        /// </summary>
        /// <param name="cancellationTokens">The CancellationTokens used to stop waiting for the control to be found.
        /// They will be linked to the <see cref="DefaultCancellationToken"/> if defined.</param>
        /// <returns>The matching WebElement, otherwise <c>null</c>.</returns>
        W Get(TimeSpan timeout);
    }
}