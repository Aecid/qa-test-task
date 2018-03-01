using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PageObjects.Applications;
using PageObjects.WebDriver;
using PageObjects;
using FluentAssertions;
using System.Collections.Generic;
using PageObjects.Helpers;
using System.Text.RegularExpressions;
using System.Linq;

namespace QATestTask
{
    [TestClass]
    public class Tests
    {
        private App appToTest = new App();
        LandingPage page;
        List<string> template;
        private static string correctText = 
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Alias enim harum minima praesentium quaerat quod.";
        private static string correctTextWithAdditionalWord = correctText + " WORD";
        private static string incorrectText = "1 " + correctText;

        private static string expectedInvalidWordForPartiallyCorrectText = "consectetur";
        private static string partiallyCorrectText = correctText.Replace(
            expectedInvalidWordForPartiallyCorrectText,
            expectedInvalidWordForPartiallyCorrectText.Remove(expectedInvalidWordForPartiallyCorrectText.Length / 2, 1));

        [TestInitialize]
        public void Init()
        {
            appToTest.Start();
            page = appToTest.LandingPage;
            template = page.GetWordsData();
        }

        [TestMethod]
        public void GivenValidation_WhenAllWordsAreCorrect_ThenShouldDisplaySuccessMessage()
        {
            page.SetTextAndBlur(correctText);

            page.IsSuccessMessageShown().Should().BeTrue();
        }

        [TestMethod]
        public void GivenValidation_WhenAllWordsAreCorrect_ThenAllWordsShouldBeValid()
        {
            page.SetTextAndBlur(correctText);

            page.GetInvalidWords().Count.Should().Be(0);

            page.GetValidWords().Count.Should().Be(
                correctText.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void GivenValidation_WhenAllWordsAreCorrectButAdditionalWhitespacesArePresent_ThenAllWordsShouldBeValid()
        {
            var whitespacedText = "   " + correctText.Replace(" ", "   ") + "   ";

            page.SetTextAndBlur(whitespacedText);

            page.GetInvalidWords().Count.Should().Be(0);

            page.GetValidWords().Count.Should().Be(
                correctText.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length);

            page.IsSuccessMessageShown().Should().BeTrue();
        }

        [TestMethod]
        public void GivenValidation_WhenAllWordsAreIncorrect_ThenAllWordsShouldBeInvalid()
        {
            page.SetTextAndBlur(incorrectText);

            page.GetValidWords().Count.Should().Be(0);

            page.GetInvalidWords().Count.Should().Be(template.Count);

            page.IsSuccessMessageShown().Should().BeFalse();
        }

        [TestMethod]
        public void GivenValidation_WhenOneWordIsIncorrect_ThenItShouldBeHighlighted()
        {
            page.SetTextAndBlur(partiallyCorrectText);
            var words = page.GetInvalidWords();
            words.Count.Should().Be(1);
            words.Should().Contain(expectedInvalidWordForPartiallyCorrectText);

            page.IsSuccessMessageShown().Should().BeFalse();
        }

        [TestMethod]
        public void GivenValidation_WhenFirstCharacterAbsent_ThenFirstWordShouldBeInvalid()
        {
            page.SetTextAndBlur(correctText.Remove(0, 1));
            page.SendBackspace();
            var words = page.GetInvalidWords();
            words.Count.Should().Be(1);
            words.Should().Contain(template[0]);

            page.IsSuccessMessageShown().Should().BeFalse();
        }

        [TestMethod]
        public void GivenValidation_WhenLastPeriodAbsent_ThenLastWordShouldBeInvalid()
        {
            page.SetTextAndBlur(correctText);
            page.SendBackspace();
            page.SendSpace();
            var words = page.GetInvalidWords();
            words.Count.Should().Be(1);
            words.Should().Contain(template.Last());

            page.IsSuccessMessageShown().Should().BeFalse();
        }

        [TestMethod]
        public void GivenValidation_WhenAdditionalWordIsSent_ThenSuccessMessageShouldNotBeShown()
        {
            page.SetTextAndBlur(correctTextWithAdditionalWord);
            page.IsSuccessMessageShown().Should().BeFalse();
        }

        [TestMethod]
        public void GivenValidation_WhenBackspaceIsSent_ThenLastWordShouldBeUnvalidated()
        {
            page.SetTextAndBlur(correctText);
            page.SendBackspace();
            var words = page.GetUnvalidatedWords();
            words.Count.Should().Be(1);
            words.Should().Contain(correctText.Split(null).Last());
        }

        [TestCleanup]
        public void CleanUp()
        {
            appToTest.Stop();
        }
    }
}
