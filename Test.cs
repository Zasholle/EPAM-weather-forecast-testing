using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace TestWebProject
{
    [TestClass]
    public class Test
    {
        private IWebDriver _driver;
        private string _baseUrl;
        private string _finalUrl;

        private readonly Configuration _appSettings =
            ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);

        [TestInitialize]
        public void SetupTest()
        {
            var browser = _appSettings.AppSettings.Settings["browser"].Value;

            switch (browser)
            {
                case "chrome":
                    _driver = new ChromeDriver();
                    break;
                case "edge":
                    _driver = new EdgeDriver();
                    break;
                case "firefox":
                    _driver = new FirefoxDriver();
                    break;
                case "opera":
                    _driver = new OperaDriver();
                    break;
                default:
                    _driver = new ChromeDriver();
                    break;
            }

            _baseUrl = "https://meteo.paraplan.net/en/";
            _finalUrl = "https://meteo.paraplan.net/en/forecast/cherepovets/aerological_diagram/";
            _driver.Navigate().GoToUrl(_baseUrl);
            _driver.Manage().Window.Maximize();
        }

        [TestMethod]
        public void SwitchPagesTest()
        {
            var forecast = By.XPath("//*[.='Five-day weather forecast']");
            var diagram = By.XPath("//a[@title='Skew-T log-P diagram']");

            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            
            _driver.FindElement(forecast).Click();
            IsElementVisible(forecast);

            IsElementVisible(diagram);
            _driver.FindElement(diagram).Click();
            Assert.AreEqual(_finalUrl, _driver.Url);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _driver.Close();
            _driver.Quit();
        }

        public void IsElementVisible(By element, int timeout = 10)
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(timeout)).Until(
                ExpectedConditions.ElementIsVisible(element));
        }
    }
}
