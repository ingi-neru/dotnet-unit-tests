using NUnit. Framework;
using OpenQA. Selenium;
using OpenQA. Selenium. Chrome;
using OpenQA. Selenium. Firefox;
using OpenQA. Selenium. Support. UI;
using SeleniumExtras. WaitHelpers;
using System;

namespace DatesAndStuff.Web.Tests
{
    [TestFixture]
    public class WizzairTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new FirefoxDriver();
            driver. Manage(). Window. Maximize();
            wait = new WebDriverWait(driver , TimeSpan. FromSeconds(10));
        }

        [Test]
        public void SearchFlights_MarosvasarhelyToBudapest_NextWeek()
        {
            driver. Navigate(). GoToUrl("https://wizzair.com/en-gb");

            // Cookie elfogadás, ha megjelenik
            try
            {
                var acceptCookiesBtn = wait. Until(ExpectedConditions. ElementToBeClickable(By. Id("onetrust-accept-btn-handler")));
                acceptCookiesBtn. Click();
            }
            catch (WebDriverTimeoutException)
            {
                // Ha nem jelenik meg, nem baj
            }
            Thread. Sleep(1000);

            var oneWayOption = wait. Until(ExpectedConditions. ElementToBeClickable(By. XPath("//*[@data-test='oneway']")));
            oneWayOption. Click();
            // Indulási hely input (class="search-departure-station autodata")
            var departureInput = wait. Until(ExpectedConditions. ElementIsVisible(By. XPath("//*[@data-test='search-departure-station']")));
            departureInput. Clear();
            departureInput. SendKeys("Tirgu Mures");

            // Várjuk az autocomplete listát és válasszuk az első elemet

            Thread. Sleep(1000);

            var departureFirstOption = wait. Until(ExpectedConditions. ElementToBeClickable(By. XPath("(//*[contains(@class,'ds-locations-container__location__name')])[1]")));
            departureFirstOption. Click();

            // Érkezési hely input (class="search-arrival-station")
            var arrivalInput = wait. Until(ExpectedConditions. ElementIsVisible(By. XPath("//*[@data-test='search-arrival-station']")));
            arrivalInput. Clear();
            arrivalInput. SendKeys("Budapest");
            Thread. Sleep(1000);

            var arrivalFirstOption = wait. Until(ExpectedConditions. ElementToBeClickable(By. XPath("(//*[contains(@class,'ds-locations-container__location__name')])[1]")));
            arrivalFirstOption. Click();

            // Indulási dátum a következő hétre

            // Kiválasztjuk a megfelelő napot a naptárból (aria-label tartalmazza az ISO dátumot)
            DateTime today = DateTime.Now;
            int daysUntilNextMonday = ((int)DayOfWeek. Monday - (int)today. DayOfWeek + 7) % 7;
            if (daysUntilNextMonday == 0) daysUntilNextMonday = 7; // ha ma hétfő, akkor a következő hétfő 7 nap múlva

            DateTime nextMonday = today. AddDays(daysUntilNextMonday);
            string formattedDate = nextMonday. ToString("dddd, dd MMMM yyyy" , new System. Globalization. CultureInfo("en-US"));

            var dateElement = wait. Until(ExpectedConditions. ElementIsVisible(By. XPath($"//*[@aria-label='{formattedDate}']")));
            dateElement. Click();
            Thread. Sleep(1000);


            // Keresés gomb (data-test="flight-search")
            var searchButton = wait. Until(ExpectedConditions. ElementToBeClickable(By. XPath("//*[@data-test='flight-search']")));
            searchButton. Click();

            if (daysUntilNextMonday == 0) daysUntilNextMonday = 7; // ha ma hétfő, akkor a következő hétfő 7 nap múlva

            DateTime startDate = today. AddDays(daysUntilNextMonday);
            Thread. Sleep(1000);
            for (int i = 0; i < 7; i++) // hét nap iterálása
            {
                DateTime currentDate = startDate. AddDays(i);
                string isoDate = currentDate. ToString("yyyy-MM-dd");

                string xpath = $"//time[@datetime='{isoDate}T00:00:00']";

                try
                {
                    // Várjuk meg, hogy az elem jelen legyen
                    var timeElement = wait. Until(ExpectedConditions. ElementExists(By. XPath(xpath)));

                    // Ellenőrizzük, hogy az elem kattintható-e (látható és engedélyezett)
                    bool isClickable = false;
                    try
                    {
                        var clickableElement = wait. Until(ExpectedConditions. ElementToBeClickable(By. XPath(xpath)));
                        isClickable = clickableElement != null;
                    }
                    catch (WebDriverTimeoutException)
                    {
                        isClickable = false;
                    }

                    if (isClickable)
                    {
                        Console. WriteLine($"Day {currentDate:dddd, dd MMMM yyyy} is clickable - clicking.");
                        timeElement. Click();

                        // Itt adhatsz plusz logikát, pl. várakozás az eredményekre, új keresés stb.
                    }
                    else
                    {
                        Console. WriteLine($"Day {currentDate:dddd, dd MMMM yyyy} is NOT clickable - skipping.");
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    Console. WriteLine($"Day {currentDate:dddd, dd MMMM yyyy} not found on the page.");
                }
            }

        }

        [TearDownAttribute]
        public void TearDown()
        {
            driver. Quit();
            driver. Dispose();
        }
    }
}
