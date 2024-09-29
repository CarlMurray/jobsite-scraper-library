using OpenQA.Selenium.Chrome;

namespace WebScraper.Factories
{
    /// <summary>
    /// Factory class for creating WebDriver instances with specified options.
    /// </summary>
    static class WebDriverFactory
    {
        /// <summary>
        /// Creates a ChromeWebDriver with a realistic user agent and a 5 second implicit wait. Use <see cref="WebDriverFactory.CreateCustomChromeWebDriver(List{string}, double)"/> to specify your own arguments.
        /// </summary>
        /// <returns>A <see cref="ChromeDriver"/> which is ready to use for scraping.</returns>
        public static (ChromeDriver, ChromeOptions) CreateDefaultChromeWebDriver()
        {

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-search-engine-choice-screen");
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36");
            var driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            return (driver, options);
        }

        /// <summary>
        /// Returns a ChromeWebDriver with user-specified arguments and delay for implicit wait.
        /// </summary>
        /// <param name="arguments">A list of strings representing chrome arguments. See <a href="https://peter.sh/experiments/chromium-command-line-switches/">here</a> for full list of available arguments.</param>
        /// <param name="implicitWaitTime">The amount of time Selenium should keep trying to find the specified web element before throwing an exception. See <a href="https://www.selenium.dev/documentation/webdriver/waits/">here</a> for reference.</param>
        /// <returns>A <see cref="ChromeDriver"/> with user-specified parameters.</returns>
        public static (ChromeDriver, ChromeOptions) CreateCustomChromeWebDriver(List<string> arguments, double implicitWaitTime = 5)
        {
            ChromeDriver driver = new ChromeDriver();
            ChromeOptions options = new ChromeOptions();
            options.AddArguments(arguments);
            options.AddArgument("--disable-search-engine-choice-screen");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(implicitWaitTime);
            return (driver, options);
        }
    }
}
