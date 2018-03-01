using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;

namespace PageObjects.WebDriver
{
    public class WebDriverFactory : IDisposable
    {
        public void Dispose()
        {
            this.driver.Dispose();
        }

        /// <summary>
        /// This is a WebDriver factory that creates driver instance of needed type.
        /// Depends on a setting in App.Config
        /// By default creates IE driver.
        /// </summary>
        private IWebDriver driver;
        private EventFiringWebDriver eventDriver;

        /// private BasicAuthWebDriverWrapper basicAuth;
        /// <summary>
        /// Creates IE driver instance
        /// </summary>
        /// <returns>Instance of IE driver</returns>
        private IWebDriver CreateIEDriver()
        {
             InternetExplorerOptions options = new InternetExplorerOptions
            {
                EnsureCleanSession = true,
                IgnoreZoomLevel = true,
                EnablePersistentHover = true,
                EnableNativeEvents = false,
            };

             // TODO: WebDriver - Insert blank lines after set of declared variables, before 'return', etc.

            InternetExplorerDriverService service = InternetExplorerDriverService.CreateDefaultService();

            service.HideCommandPromptWindow = true;

            driver = new InternetExplorerDriver(service, options);

            return driver;
        }

        private IWebDriver CreateFirefoxDriver()
        {
            FirefoxProfile profile = new FirefoxProfile();

            driver = new FirefoxDriver(profile);
            return driver;
        }

        /// <summary>
        /// Creates Chrome driver instance
        /// </summary>
        /// <returns>Chrome driver instance</returns>
        private IWebDriver CreateChromeDriver()
        {
            var options = new ChromeOptions();
            options.AddArguments(new List<string>() { "headless" }); // Comment this row to disable headless mode for Chrome.
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            driver = new ChromeDriver(service, options);
            return driver;
        }

        /// <summary>
        /// Creates instance of needed driver
        /// </summary>
        /// <param name="browser">"IE" for Internet Explorer driver, "Firefox" for Firefox driver, "Chrome" for Chrome driver</param>
        /// <returns></returns>
        public IWebDriver GetDriver(BrowserType browser)
        {
            IWebDriver driver;
            switch (browser)
            {
                case BrowserType.IE:
                    driver = CreateIEDriver();
                    break;
                case BrowserType.Chrome:
                    driver = CreateChromeDriver();
                    break;
                case BrowserType.Firefox:
                    driver = CreateFirefoxDriver();
                    break;
                default:
                    driver = CreateFirefoxDriver();
                    break;
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Cookies.DeleteAllCookies();
            return driver;
        }
    }
}
