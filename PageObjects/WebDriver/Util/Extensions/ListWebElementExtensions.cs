using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace PageObjects.WebDriver
{
    public static class ListWebElementExtensions
    {
        /// <summary>
        /// Waits until all elements in List become visible
        /// </summary>
        /// <param name="element">IWebElement element to check</param>
        /// <param name="timeOut">TimeSpan to wait</param>
        /// <returns></returns>
        public static IList<IWebElement> WaitUntilAllElementsVisible(this IList<IWebElement> list, TimeSpan timeOut)
        {
            return Waiter.UntilAllElementsAreVisible(list, timeOut);
        }

        /// <summary>
        /// Waits until all elements in List become visible
        /// </summary>
        /// <param name="element">IWebElement element to check</param>
        /// <returns></returns>
        public static IList<IWebElement> WaitUntilAllElementsVisible(this IList<IWebElement> list)
        {
            return Waiter.UntilAllElementsAreVisible(list, Waiter.DefaultTimeOut);
        }
    }
}