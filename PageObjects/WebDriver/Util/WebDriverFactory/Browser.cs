using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;

namespace PageObjects.WebDriver
{
    public class Browser
    {
        /// <summary>
        /// Class that provides static property Driver for use in all framework.
        /// </summary>
        private static readonly object Padlock = new object();
        private static IWebDriver driverInstance = null;
        private static WebDriverFactory driverFactory = new WebDriverFactory();

        public Browser()
        {
        }

        /// <summary>
        /// Static property that contains singleton WebDriver instance
        /// Change 'BrowserType' for different browser.
        /// This could be moved to appsettings.
        /// </summary>
        public static IWebDriver Driver
        {
            get
            {
                lock (Padlock)
                {
                    driverInstance = driverInstance ?? driverFactory.GetDriver(BrowserType.Chrome);
                }

                return driverInstance;
            }
        }

        /// <summary>
        /// Correctly closes WebDriver instance
        /// </summary>
        public static void Close()
        {
            if (driverInstance != null)
            {
                driverInstance.Quit();
                driverInstance = null;
            }
        }
    }
}
