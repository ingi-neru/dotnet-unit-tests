﻿using System;
using System. Diagnostics;
using System. Reflection;
using System. Text;
using FluentAssertions;
using OpenQA. Selenium;
using OpenQA. Selenium. Chrome;
using OpenQA. Selenium. Support. UI;
using SeleniumExtras. WaitHelpers;

namespace DatesAndStuff. Web. Tests
{
    [TestFixture]
    public class PersonPageTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private const string BaseURL = "http://localhost:5091";
        private bool acceptNextAlert = true;

        private Process? _blazorProcess;

        [OneTimeSetUp]
        public void StartBlazorServer()
        {
            var webProjectPath = Path. GetFullPath(Path. Combine(
                Assembly. GetExecutingAssembly(). Location ,
                "../../../../../../../src/DatesAndStuff.Web/DatesAndStuff.Web.csproj"
                ));

            var webProjFolderPath = Path. GetDirectoryName(webProjectPath);

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet" ,
                //Arguments = $"run --project \"{webProjectPath}\"",
                Arguments = "dotnet run --no-build" ,
                WorkingDirectory = webProjFolderPath ,
                RedirectStandardOutput = true ,
                RedirectStandardError = true ,
                UseShellExecute = false
            };

            _blazorProcess = Process. Start(startInfo);

            // Wait for the app to become available
            var client = new HttpClient();
            var timeout = TimeSpan. FromSeconds(30);
            var start = DateTime. Now;

            while (DateTime. Now - start < timeout)
            {
                try
                {
                    var result = client. GetAsync(BaseURL). Result;
                    if (result. IsSuccessStatusCode)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Thread. Sleep(1000);
                }
            }
        }

        [OneTimeTearDown]
        public void StopBlazorServer()
        {
            if (_blazorProcess != null && !_blazorProcess. HasExited)
            {
                _blazorProcess. Kill(true);
                _blazorProcess. Dispose();
            }
        }

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver. Quit();
                driver. Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
       

        }
        public static IEnumerable<double> SalaryIncreasePercentages => new double [ ]
{
            0,
            5,
            10,
            15,
            20,
            -9.9999,
            -10,
            100
};

        [Test, TestCaseSource(nameof(SalaryIncreasePercentages))]
        public void Person_SalaryIncrease_ShouldIncrease(double increasePercent)
        {
            // Arrange
            var wait = new WebDriverWait(driver , TimeSpan. FromSeconds(5));
            driver. Navigate(). GoToUrl(BaseURL);
            driver. FindElement(By. XPath("//*[@data-test='PersonPageNavigation']")). Click();
            Thread. Sleep(100);
            var salaryLabel = wait. Until(ExpectedConditions. ElementExists(By. XPath("//*[@data-test='DisplayedSalary']")));
            var baseSalary = double. Parse(salaryLabel. Text);

            
            var input = wait. Until(ExpectedConditions. ElementExists(By. XPath("//*[@data-test='SalaryIncreasePercentageInput']")));
            input. Clear();
            input. SendKeys(increasePercent.ToString());


            // Act
            var submitButton = wait. Until(ExpectedConditions. ElementExists(By. XPath("//*[@data-test='SalaryIncreaseSubmitButton']")));
            submitButton. Click();
            Thread. Sleep(100);

            // Assert
            var salaryAfterSubmission = double. Parse(salaryLabel. Text);
            if (increasePercent > -10)
            {
                double expectedSalary = baseSalary * (1 + increasePercent / 100);
                salaryAfterSubmission. Should(). BeApproximately(expectedSalary , 0.001);
            }
            else
            {
                // Ha -10-nél kisebb az érték, akkor nem változhat a fizetés
                salaryAfterSubmission. Should(). Be(baseSalary);
            }
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver. FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver. SwitchTo(). Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver. SwitchTo(). Alert();
                string alertText = alert. Text;
                if (acceptNextAlert)
                {
                    alert. Accept();
                }
                else
                {
                    alert. Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}