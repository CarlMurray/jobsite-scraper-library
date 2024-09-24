using OpenQA.Selenium;
using WebScraper.Factories;
using WebScraper.Interfaces;

namespace WebScraper.Classes
{
    public abstract class JobScraper : IJobScraper
    {
        public WebDriver Driver { get; }
        public DriverOptions Options { get; }
        public abstract string JobDetailViewUri { get; }
        public abstract string JobIdCssSelector { get; }
        public abstract string JobIdDomAttributeName { get; }
        public List<string> JobIds { get; set; } = new List<string>();
        public List<Job> JobResults { get; set; } = new List<Job>();
        public abstract string RootDomain { get; }

        /// <summary>
        /// Creates a scraper with the default WebDriver options specified in <see cref="WebDriverFactory.CreateDefaultChromeWebDriver()"/>.
        /// </summary>
        public JobScraper()
        {
            (Driver, Options) = WebDriverFactory.CreateDefaultChromeWebDriver();
        }

        /// <summary>
        /// Creates a scraper with user-specified WebDriver options.
        /// </summary>
        /// <param name="arguments">A list of strings representing chrome arguments. 
        /// See <a href="https://peter.sh/experiments/chromium-command-line-switches/">here</a> for full list of available arguments.</param>
        /// <param name="implicitWaitTime">The amount of time Selenium should keep trying to find the specified web element before throwing an exception. 
        /// See <a href="https://www.selenium.dev/documentation/webdriver/waits/">here</a> for reference.</param>
        public JobScraper(List<string> arguments, double implicitWaitTime = 5)
        {
            (Driver, Options) = WebDriverFactory.CreateCustomChromeWebDriver(arguments, implicitWaitTime);
        }

        /// <summary>
        /// Sign in to the job site by adding an authentication cookie from an existing logged in session. You can find this cookie using Chrome DevTools. It is names "PPID" for Indeed and "li_at" for LinkedIn. Copy its value and pass it into this method.
        /// </summary>
        /// <param name="cookie">An authentication cookie to log in to job site.</param>
        public void AuthenticateWithCookie(string cookieName, string cookieValue)
        {
            Driver.Manage().Cookies.AddCookie(new Cookie(cookieName, cookieValue));
        }


        /// <summary>
        /// Initializes the job scraper by setting the driver URL to the root domain, authenticating with the provided cookie, and refreshing the page.
        /// </summary>
        /// <param name="cookieName">Name of cookie. Indeed: "PPID"; LinkedIn: "li_at".</param>
        /// <param name="cookieValue">Value of the cookie</param>
        public void Initialise(string cookieName, string cookieValue)
        {
            Driver.Url = RootDomain;
            AuthenticateWithCookie(cookieName, cookieValue);
            Driver.Navigate().Refresh();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckIfNextPageExists();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public abstract void NavigateToNextPage();

        /// <summary>
        /// Scrapes text content for all job posts for the job IDs stored in the JobIds property. See <see cref="ScrapeJob(string)"/>.
        /// </summary>
        /// <param name="antiBotDelay">The amount of time to wait between scraping each job post. This is to avoid being blocked by the website. 7.5 seconds by default.</param>
        public virtual void ScrapeAllJobs(int antiBotDelay = 7500)
        {
            foreach (var jobId in JobIds)
            {
                ScrapeJob(jobId);
                Thread.Sleep(antiBotDelay);
            }
        }

        public abstract Job ScrapeJob(string jobId);

        /// <summary>
        /// Scrapes the job ID numbers from all job post cards on a single search result page. 
        /// Adds the Job IDs to the <see cref="LinkedInJobScraper.JobIds"/> property.
        /// Note: This only scrapes the Job ID numbers and not the actual job content. 
        /// This method should be used to build a list of Job IDs which can then be scraped using <see cref="LinkedInJobScraper.ScrapeAllJobs()"/> or <see cref="LinkedInJobScraper.ScrapeSpecifiedJobs(List{string})"/>.
        /// </summary>
        /// <returns>A list of job IDs as strings</returns>
        public virtual List<string> ScrapeJobIdsOnCurrentPage()
        {
            var jobCardList = Driver.FindElements(By.CssSelector(JobIdCssSelector));
            List<string> jobIds = new List<string>();
            foreach (IWebElement jobElement in jobCardList)
            {
                string jobId = jobElement.GetDomAttribute(JobIdDomAttributeName);
                jobIds.Add(jobId);
            }
            JobIds.Concat(jobIds);
            return jobIds;
        }


        /// <summary>
        /// Scrapes text content for a list of specified job IDs and adds to the JobResults property. See <see cref="ScrapeJob(string)"/>.
        /// </summary>
        /// <param name="jobIds">A list of the Job IDs to scrape the text content for.</param>
        /// <param name="antiBotDelay">The amount of time to wait between scraping each job post. This is to avoid being blocked by the website. Recommended: LinkedIn=7500, Indeed=30000</param>
        public virtual void ScrapeSpecifiedJobs(List<string> jobIds, int antiBotDelay)
        {
            foreach (var jobId in jobIds)
            {
                ScrapeJob(jobId);
                Thread.Sleep(antiBotDelay);
            }
        }

        /// <summary>
        /// Searches for jobs by navigating to the search results page using URL parameters.
        /// </summary>
        /// <param name="jobTitle">Job title keywords</param>
        /// <param name="location">Job location</param>
        public abstract void SearchForJobsViaUrlParams(string jobTitle, string location);
    }
}
