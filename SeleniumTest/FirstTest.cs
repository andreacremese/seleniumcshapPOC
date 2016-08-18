using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumTest {

    public class FirstTest {
        private const string baseUrl = "https://run.plnkr.co/plunks/JmAWOqlJQ3JecBY8IJpD/";
        private const string driversLocation = "../..";

        [Fact]
        public void goesToPage1() {
            using (IWebDriver driver = new InternetExplorerDriver(driversLocation)) {
                // arrange
                
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(baseUrl);

                IWebElement button = driver.FindElement(By.Id("1"));
                // act
                button.Click();
                // assert
                Assert.NotEqual(driver.Url.IndexOf("page1.html"), -1);
                Assert.NotEmpty(driver.FindElements(By.XPath("//*[contains(text(), 'page 1')]")));
                Assert.Empty(driver.FindElements(By.XPath("//*[contains(text(), 'page 2')]")));
            }
        }

        [Fact]
        public void goesToPage2() {
            using (IWebDriver driver = new InternetExplorerDriver(driversLocation)) {
                // arrange
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(baseUrl);

                IWebElement button = driver.FindElement(By.Id("2"));
                // act
                button.Click();
                // assert
                Assert.NotEqual(driver.Url.IndexOf("page2.html"), -1);
                Assert.Empty(driver.FindElements(By.XPath("//*[contains(text(), 'page 1')]")));
                Assert.NotEmpty(driver.FindElements(By.XPath("//*[contains(text(), 'page 2')]")));
            }
        }

        [Fact]
        public void allowsFormSumitting(){
            using (IWebDriver driver = new InternetExplorerDriver(driversLocation)) {
                // arrange
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(baseUrl);
                var inputs = driver.FindElements(By.TagName("input"));
                Assert.False(inputs.Count < 4);
                var firstName = inputs[0];
                var checkbox = inputs[2];
                var submit = inputs[3];
                // Act
                firstName.SendKeys("andrea");
                checkbox.Click();
                submit.Click();
                // Assert
                // wait up to 5 seconds in order to make sure the page loads
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0,0, 5));
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[contains(text(), 'well done andrea')]")));
            }
        }

        [Fact]
        public void requiresacceptConditions() {
            using (IWebDriver driver = new InternetExplorerDriver(driversLocation)) {
                // arrange
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(baseUrl);
                var inputs = driver.FindElements(By.TagName("input"));
                Assert.False(inputs.Count < 4);
                var firstName = inputs[0];
                var checkbox = inputs[2];
                var submit = inputs[3];
                // Act
                firstName.SendKeys("andrea");
                submit.Click();
                // Assert
                // wait up to 5 seconds in order to make sure the page loads
                String text = (new WebDriverWait(driver, TimeSpan.FromSeconds(5))).Until(d => d.SwitchTo().Alert().Text);
                Assert.True(text.Contains("You must agree to the terms first."));
            }
        }
    }
}
