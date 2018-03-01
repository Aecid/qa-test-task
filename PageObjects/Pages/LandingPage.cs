using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageObjects.WebDriver;
using PageObjects.Helpers;

namespace PageObjects
{
    public class LandingPage : Page
    {
        private IWebElement textContainer
        {
            get
            {
                return Waiter.WaitForExistence(By.XPath("//div[@class='tah-placeholder tah-layer']"));
            }
        }

        private IWebElement textInput
        {
            get
            {
                return Waiter.WaitForExistence(By.XPath("//div[@class='tah-textarea tah-layer']"));
            }
        }

        private IWebElement successMessage
        {
            get
            {
                return Waiter.WaitForExistence(By.ClassName("success-message"));
            }
        }

        private bool ValidationCheck(string classname)
        {
            return classname.Equals("tah-word-valid");
        }

        public LandingPage(IWebDriver driver) :
            base(driver)
        {
        }

        public List<string> GetWordsData()
        {
            var wordsData = new List<string>();

            var text = this.textContainer.Text;

            var words = text.Split(null);

            foreach (var word in words)
            {
                wordsData.Add(word.ToLowerInvariant());
            }

            return wordsData;
        }

        public List<Word> GetValidatedWordsData()
        {
            var validatedWordsData = new List<Word>();

            IList<IWebElement> spans = textContainer.FindElements(By.XPath("./span"));

            foreach (var span in spans)
            {
                validatedWordsData.Add(new Word(span.Text.ToLowerInvariant(), ValidationCheck(span.GetAttribute("class"))));
            }

            return validatedWordsData;
        }

        public List<string> GetUnvalidatedWords()
        {
            var wordsData = GetWordsData();
            var validatedWordsData = GetValidatedWordsData();
            if (wordsData.Count == validatedWordsData.Count)
            {
                return null;
            }

            else
            {
                var unvalidatedWords = new List<string>();
                foreach (var word in wordsData)
                {
                    if (!validatedWordsData.Any(w => w.Text.Equals(word.ToLowerInvariant())))
                    {
                        unvalidatedWords.Add(word.ToLowerInvariant());
                    }
                }

                return unvalidatedWords;
            }
        }

        public List<string> GetValidWords()
        {
            var words = GetValidatedWordsData();
            return words.Where(w => w.Valid == true).Select(i => i.ToString().ToLowerInvariant()).ToList();
        }

        public List<string> GetInvalidWords()
        {
            var words = GetValidatedWordsData();
            return words.Where(w => w.Valid == false).Select(i => i.ToString().ToLowerInvariant()).ToList();
        }

        public void SetText(string text)
        {
            this.textInput.SetText(text.ToLowerInvariant());
        }

        public void SetTextUpperCase(string text)
        {
            this.textInput.SetText(text.ToUpperInvariant());
        }

        public void SendBackspace() //such methods could be replaced with escaped sequences like \b but for better readability - this. :)
        {
            this.textInput.SendKeys(Keys.Backspace);
        }

        public void SendSpace()
        {
            this.textInput.SendKeys(Keys.Space);
        }

        public void SetTextAndBlur(string text)
        {
            this.textInput.SetTextAndTab(text.ToLowerInvariant());
        }

        public bool IsSuccessMessageShown()
        {
            return successMessage.Displayed;
        }
    }
}
