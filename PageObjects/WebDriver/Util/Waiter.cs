using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;

namespace PageObjects.WebDriver
{
    /// <summary>
    /// That class covers and extends ExpectedConditions functionality.
    /// It provides support methods for IWebElement and IList extension methods.
    /// It's my oooold class. :D
    /// </summary>
    public static class Waiter
    {
        static Waiter()
        {
            var timeOut = "5"; //string to get from config file if needed
            var shortTimeOut = "1"; //string to get from config file if needed
            DefaultTimeOut = TimeSpan.FromSeconds(int.Parse(timeOut));
            DefaultShortTimeOut = TimeSpan.FromSeconds(int.Parse(shortTimeOut));
        }

        /// <summary>
        /// Default timeOut for waitings
        /// </summary>
        public static TimeSpan DefaultTimeOut;

        /// <summary>
        /// Default short timeOut for waitings
        /// </summary>
        public static TimeSpan DefaultShortTimeOut;

        /// <summary>
        /// Default WebDriverWait waiting.
        /// </summary>
        /// <returns>WebDriverWait object with default TimeOut</returns>
        public static WebDriverWait Wait()
        {
            return new WebDriverWait(Browser.Driver, DefaultTimeOut);
        }

        /// <summary>
        /// Custom TimeOut WebDriverWait.
        /// </summary>
        /// <param name="timeOut">TimeOut for WebDriverWait</param>
        /// <returns></returns>
        public static WebDriverWait Wait(TimeSpan timeOut)
        {
            return new WebDriverWait(Browser.Driver, timeOut);
        }

        /// <summary>
        /// Simple waits for TimeSpan.
        /// Please avoid that unless absolutely needed.
        /// </summary>
        /// <param name="timeOut">Time to wait</param>
        public static void WaitFor(TimeSpan timeOut)
        {
            try
            {
                Waiter.Wait(timeOut).Until<bool>(
                (driver) =>
                {
                    return false;
                });
            }
            catch (WebDriverTimeoutException)
            {
            }
        }

        /// <summary>
        /// Support function method for WaitUntilVisible() extension method
        /// </summary>
        /// <param name="element"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static IWebElement UntilVisible(IWebElement element, TimeSpan timeOut)
        {
            Func<IWebDriver, IWebElement> elementIsVisible = 
                (driver) =>
            {
                try
                {
                    return element.Displayed ? element : null;
                }
                catch (Exception)
                {
                    return null;
                }
            };
            return new WebDriverWait(Browser.Driver, timeOut).Until(elementIsVisible);
        }

        /// <summary>
        /// Support function method for WaitUntilInvisible() extension method
        /// </summary>
        /// <param name="element"></param>
        /// <param name="timeOut"></param>
        public static void UntilInvisible(IWebElement element, TimeSpan timeOut)
        {
                new WebDriverWait(Browser.Driver, timeOut).Until<bool>(
                    (driver) =>
                    {
                        try
                        {
                            return !element.Displayed;
                        }
                        catch(Exception)
                        {
                            return true;
                        }
                    });
        }


        /// <summary>
        /// Waits for element to have any text.
        /// </summary>
        /// <param name="element">element to check</param>
        /// <param name="timeOut">time to wait</param>
        public static IWebElement UntilElementHasAnyText(IWebElement element, TimeSpan timeOut)
        {
            new WebDriverWait(Browser.Driver, timeOut).Until<bool>(
                (driver) =>
                {
                    try
                    {
                        return element.Text != "";
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
            return element;
        }

        /// <summary>
        /// Wait for element to contain some text
        /// </summary>
        /// <param name="element">Element to check</param>
        /// <param name="textToSearch">Text to search</param>
        /// <param name="timeOut">timeOut</param>
        /// <returns></returns>
        public static IWebElement UntilElementHasText(IWebElement element, string textToSearch, TimeSpan timeOut)
        {
            new WebDriverWait(Browser.Driver, timeOut).Until<bool>(
                (driver) =>
                {
                    return element.Text.Contains(textToSearch);
                });

            return element;
        }

        public static IWebElement UntilElementAttributeContainsText(IWebElement element, string attribute, string textToSearch, TimeSpan timeOut)
        {
            new WebDriverWait(Browser.Driver, timeOut).Until<bool>(
                (driver) =>
                {
                    try
                    {
                        return element.GetAttribute(attribute).Contains(textToSearch);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });

            return element;
        }

        public static IWebElement UntilElementAttributeNotContainsText(IWebElement element, string attribute, string textToSearch, TimeSpan timeOut)
        {
            new WebDriverWait(Browser.Driver, timeOut).Until<bool>(
                (driver) =>
                {
                    try
                    {
                        return !element.GetAttribute(attribute).Contains(textToSearch);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });

            return element;
        }

        /// <summary>
        /// Support function method for WaitUntilAllVisible() extension method
        /// </summary>
        /// <param name="list"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static IList<IWebElement> UntilAllElementsAreVisible(IList<IWebElement> list, TimeSpan timeOut)
        {
            Func<IWebDriver, IList<IWebElement>> ifAllElementsAreVisible =
                (driver) =>
                {
                    foreach (IWebElement element in list)
                    {
                        try
                        {
                            if (!element.Displayed)
                            {
                                return null;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    return list.Count() > 0 ? list : null;
                };

            return new WebDriverWait(Browser.Driver, timeOut).Until(ifAllElementsAreVisible);
        }

        /// <summary>
        /// Support function method for WaitUntilClickable() extension method
        /// </summary>
        /// <param name="element"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static IWebElement UntilClickable(IWebElement element, TimeSpan timeOut)
        {
            Func<IWebDriver, IWebElement> elementIsClickable =
                (driver) =>
                {
                    try
                    {
                        return (element.Displayed && element.Enabled) ? element : null;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                };

            return new WebDriverWait(Browser.Driver, timeOut).Until(elementIsClickable);
        }

        public static IWebElement WaitForVisibility(By locator)
        {
            return Waiter.Wait().Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public static IWebElement WaitForVisibility(By locator, TimeSpan timeOut)
        {
            return Waiter.Wait(timeOut).Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public static IWebElement WaitForExistence(By locator)
        {
            return Waiter.Wait().Until(ExpectedConditions.ElementExists(locator));
        }

        public static IWebElement WaitForExistence(By locator, TimeSpan timeOut)
        {
            return Waiter.Wait(timeOut).Until(ExpectedConditions.ElementExists(locator));
        }

        public static void WaitForInvisibility(By locator)
        {
           Waiter.Wait().Until<bool>(
                    (driver) =>
                    {
                        try
                        {
                            return !Waiter.WaitForVisibility(locator).Displayed;
                        }
                        catch (Exception)
                        {
                            return true;
                        }
                    });
        }

        public static void WaitForInvisibility(By locator, TimeSpan timeOut)
        {
            Waiter.Wait().Until<bool>(
                     (driver) =>
                     {
                         try
                         {
                             return !Waiter.WaitForVisibility(locator, timeOut).Displayed;
                         }
                         catch (Exception)
                         {
                             return true;
                         }
                     });
        }

        public static void WaitForFrameAndSwitchToIt(string frameLocator)
        {
            Waiter.Wait().Until(
        (driver) =>
        {
            try
            {
                return driver.SwitchTo().Frame(frameLocator);
            }
            catch (NoSuchFrameException e)
            {
                return null;
            }
        });
        }

        public static void WaitForProjectLoad()
        {
            WaitForFrameAndSwitchToIt("docPreview");
            Browser.Driver.SwitchTo().DefaultContent();
        }

        public static IList<IWebElement> WaitForListVisibility(By listLocator, TimeSpan timeOut)
        {
            Func<IWebDriver, IList<IWebElement>> ifAllElementsAreVisible =
                (driver) =>
                {
                    var list = Browser.Driver.FindElements(listLocator);

                    foreach (IWebElement element in list)
                    {
                        try
                        {
                            if (!element.Displayed)
                            {
                                return null;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    return list.Count() > 0 ? list : null;
                };

            return new WebDriverWait(Browser.Driver, timeOut).Until(ifAllElementsAreVisible);
        }

        public static IList<IWebElement> WaitForListVisibility(By listLocator)
        {
            Func<IWebDriver, IList<IWebElement>> ifAllElementsAreVisible =
                (driver) =>
                {
                    var list = Browser.Driver.FindElements(listLocator);

                    foreach (IWebElement element in list)
                    {
                        try
                        {
                            if (!element.Displayed)
                            {
                                return null;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    return list.Count() > 0 ? list : null;
                };

            return new WebDriverWait(Browser.Driver, DefaultTimeOut).Until(ifAllElementsAreVisible);
        }
    }
}