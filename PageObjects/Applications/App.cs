using OpenQA.Selenium;
using PageObjects.WebDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.Applications
{
    public class App
    {
        IWebDriver driver;

        public App()
        {
            driver = Browser.Driver;
        }

        public void Start()
        {
            driver.Navigate().GoToUrl("https://hexagontech.github.io/interview-qa-task/");
        }

        public void Stop()
        {
            Browser.Close();
        }

        public LandingPage LandingPage
        {
            get
            {
                return new LandingPage(driver);
            }
        }
    }
}
