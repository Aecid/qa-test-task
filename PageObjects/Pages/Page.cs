using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjects.Helpers;

namespace PageObjects.WebDriver
{
    public abstract class Page
    {
        protected IWebDriver driver;

        public Page(IWebDriver webdriver)
        {
            this.driver = webdriver;
            PageFactory.InitElements(driver, this);
        }
    }
}
