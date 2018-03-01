using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;

namespace PageObjects.WebDriver
{
    public static class WebElementExtensions
    {
        /// <summary>
        /// Waits until element becomes visible
        /// </summary>
        /// <param name="element">element to check</param>
        /// <param name="timeOut">time to wait</param>
        /// <returns></returns>
        public static IWebElement WaitUntilVisible(this IWebElement element, TimeSpan timeOut)
        {
            return Waiter.UntilVisible(element, timeOut);
        }

        public static IWebElement WaitForAnimation(this IWebElement element)
        {
            Waiter.WaitFor(TimeSpan.FromMilliseconds(1000));
            return element;
        }

        /// <summary>
        /// Method to wait for fade-in appearance
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IWebElement WaitForFadeInAnimation(this IWebElement element)
        {
            string opacity = null;

            while (opacity.Equals("1") != true)
            {
                Waiter.WaitFor(TimeSpan.FromMilliseconds(500));

                opacity = element.GetCssValue("opacity");
            }            
            
            return element;
        }

        /// <summary>
        /// Waits until element becomes visible
        /// </summary>
        /// <param name="element">element to check</param>
        /// <returns></returns>
        public static IWebElement WaitUntilVisible(this IWebElement element)
        {
            return Waiter.UntilVisible(element, Waiter.DefaultTimeOut);
        }

        /// <summary>
        /// Waits until element becomes invisible
        /// </summary>
        /// <param name="element">element to wait for disappear</param>
        /// <param name="timeOut">time to wait</param>
        public static void WaitUntilInvisible(this IWebElement element, TimeSpan timeOut)
        {
            Waiter.UntilInvisible(element, timeOut);
        }

        /// <summary>
        /// Waits until element becomes invisible
        /// </summary>
        /// <param name="element">element to check</param>
        public static void WaitUntilInvisible(this IWebElement element)
        {
            Waiter.UntilInvisible(element, Waiter.DefaultTimeOut);
        }

        /// <summary>
        /// Waits until element becomes visible and enabled
        /// </summary>
        /// <param name="element">element to check</param>
        /// <param name="timeOut">time to wait</param>
        /// <returns></returns>
        public static IWebElement WaitUntilClickable(this IWebElement element, TimeSpan timeOut)
        {
            return Waiter.UntilClickable(element, timeOut);
        }
        

        /// <summary>
        /// This method works with shorter default timeout, but retries the count we input
        /// </summary>
        /// <param name="element"></param>
        /// <param name="maxRetries"></param>
        /// <returns></returns>
        public static IWebElement WaitUntilClickable(this IWebElement element, int maxRetries)
        {
            int i = 0;

            IWebElement foundElement = null;
            
            while (i < maxRetries) 
            {
                foundElement = Waiter.UntilClickable(element, Waiter.DefaultShortTimeOut);
                if (foundElement != null) 
                {
                    break;
                }
                
                i++;
            }
            return foundElement;
        }

        /// <summary>
        /// Waits until element becomes visible and enabled
        /// </summary>
        /// <param name="element">element to check</param>
        /// <returns></returns>
        public static IWebElement WaitUntilClickable(this IWebElement element)
        {
            return Waiter.UntilClickable(element, Waiter.DefaultTimeOut);
        }

        public static IWebElement WaitUntilEnabled(this IWebElement element)
        {
            var stopWatch = Stopwatch.StartNew();
            
            while((!element.Enabled))
            {
                if (stopWatch.Elapsed > Waiter.DefaultTimeOut)
                    throw new Exception(string.Format("Timeout expired in waiting for the element: {0} to enable", element.TagName));
            }

            return element;
        }

        /// <summary>
        /// Waits until element has any text
        /// </summary>
        /// <param name="element">element to check</param>
        /// <returns></returns>
        public static IWebElement WaitUntilElementHasAnyText(this IWebElement element)
        {
            return Waiter.UntilElementHasAnyText(element, Waiter.DefaultTimeOut);
        }

        /// <summary>
        /// Waits until element contains needed text
        /// </summary>
        /// <param name="element">element to check</param>
        /// <param name="textToSearch">text to wait in element</param>
        /// <returns></returns>
        public static IWebElement WaitUntilElementHasText(this IWebElement element, string textToSearch)
        {
            return Waiter.UntilElementHasText(element, textToSearch, Waiter.DefaultTimeOut);
        }

        public static IWebElement WaitUntilElementAttributeHasText(this IWebElement element, string attribute, string textToSearch)
        {
            return Waiter.UntilElementAttributeContainsText(element, attribute, textToSearch, Waiter.DefaultTimeOut);
        }

        public static IWebElement WaitUntilElementAttributeHasNoText(this IWebElement element, string attribute, string textToSearch)
        {
            return Waiter.UntilElementAttributeNotContainsText(element, attribute, textToSearch, Waiter.DefaultTimeOut);
        }

        /// <summary>
        /// Hover mouse above the element
        /// </summary>
        /// <param name="element">element to hove mouse over</param>
        /// <returns></returns>
        public static IWebElement MouseHover(this IWebElement element, int timeOut = 1)
        {
            OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(Browser.Driver);
            action.MoveToElement(element).Build().Perform();
            return element;
        }

        /// <summary>
        /// Double click on element
        /// </summary>
        /// <param name="element">Target</param>
        /// <returns></returns>
        public static IWebElement DoubleClick(this IWebElement element)
        {
            OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(Browser.Driver);
            action.DoubleClick().Build().Perform();
            return element;
        }

        /// <summary>
        /// Hover mouse above first element, then click on second element
        /// </summary>
        /// <param name="elementToHover">first IWebElement element</param>
        /// <param name="elementToClick">second IWebElement element</param>
        /// <returns></returns>
        public static IWebElement MouseHoverThenClick(this IWebElement elementToHover, IWebElement elementToClick)
        {
            OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(Browser.Driver);
            action.MoveToElement(elementToHover);
            action.MoveToElement(elementToClick);
            action.Click(elementToClick);
            action.Build().Perform();
            return elementToHover;
        }

        /// <summary>
        /// Clear input, sendkeys
        /// </summary>
        /// <param name="element">element to send keys</param>
        /// <param name="value">value to input</param>
        /// <param name="clearFirst">clear field before sending keys or not</param>
        public static void SetText(this IWebElement element, string value, bool clearFirst = true)
        {
            if (clearFirst)
            {
                element.Clear();
            }

            element.SendKeys(value);
        }

        /// <summary>
        /// Clear input, sendkeys, press Enter
        /// </summary>
        /// <param name="element">element to send keys</param>
        /// <param name="value">value to input</param>
        /// <param name="clearFirst">clear field before sending keys or not</param>
        public static void SetTextAndEnter(this IWebElement element, string value, bool clearFirst = true)
        {
            if (clearFirst)
            {
                element.Clear();
            }

            element.SendKeys(value);
            element.SendKeys(Keys.Return);
        }

        /// <summary>
        /// Clear input, sendkeys, press Tab to change focus
        /// </summary>
        /// <param name="element">element to send keys</param>
        /// <param name="value">value to input</param>
        /// <param name="clearFirst">clear field before sending keys or not</param>
        public static void SetTextAndTab(this IWebElement element, string value, bool clearFirst = true)
        {
            if (clearFirst)
            {
                element.Clear();
            }

            element.SendKeys(value);
            element.SendKeys(Keys.Tab);
        }

        /// <summary>
        /// Wrapper for IWebElement to SelectElement
        /// </summary>
        /// <param name="element">element to wrap into SelectElement</param>
        /// <returns></returns>
        public static SelectElement AsSelectElement(this IWebElement element)
        {
            return new SelectElement(element);
        }

        /// <summary>
        /// Does element exist on the page
        /// </summary>
        /// <param name="element">Target element</param>
        /// <returns>true if element exists</returns>
        public static bool ElementExists(this IWebElement element)
        {
            try
            {
                return element.WaitUntilVisible(Waiter.DefaultTimeOut).Displayed;
            }

            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Does element exist on the page, using user provided timeout
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="userSetTimeOut">User set timeout</param>
        /// <returns>true if element exists</returns>
        public static bool ElementExists(this IWebElement element, TimeSpan userSetTimeOut)
        {
            try
            {
                return element.WaitUntilVisible(userSetTimeOut).Displayed;
            }

            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Focus outside of the component and click with mouse
        /// </summary>
        /// <param name="element">Target Element</param>
        /// <param name="driver">Driver Component</param>
        /// <returns>true if action succeeded</returns>
        public static bool MoveOutAndClick(this IWebElement element, IWebDriver driver)
        {
            try
            {
                Actions actions = new Actions(driver);

                actions.MoveToElement(element, -1, 0).Click().Perform();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method to find the checked status of a checkbox web element.
        /// </summary>
        /// <param name="element">Target Element</param>
        /// <returns></returns>
        public static bool IsCheckboxChecked(this IWebElement element)
        {
            string script = string.Format("return document.getElementById('{0}').checked",element.GetAttribute("id"));

            IJavaScriptExecutor executer = Browser.Driver as IJavaScriptExecutor;
            var isChecked = (bool)executer.ExecuteScript(script);

            if (isChecked)
            {
                return true;
            }

            return false;
        }
    }
}