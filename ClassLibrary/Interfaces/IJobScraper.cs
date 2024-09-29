using OpenQA.Selenium;
using WebScraper.Classes;

namespace WebScraper.Interfaces
{
    /// <summary>
    /// Describes the behaviour for a Job Scraper. The implementation is not tied to a specific job site and can be adapted for various sites.
    /// </summary>
    public interface IJobScraper
    {
        /// <summary>
        /// Selenium WebDriver instance
        /// </summary>
        WebDriver Driver { get; }

        /// <summary>
        /// Options for the Selenium WebDriver
        /// </summary>
        DriverOptions Options { get; }

        /// <summary>
        /// Root domain of the job site
        /// </summary>
        string RootDomain { get; }

        /// <summary>
        /// Stores the list of Jobs that have been scraped by the JobScraper
        /// </summary>
        List<Job> JobResults { get; set; }

        /// <summary>
        /// Stores the list of Job IDs that have been scraped by the JobScraper. 
        /// These IDs are used to scrape the text content of the job posts.
        /// </summary>
        List<string> JobIds { get; set; }

        /// <summary>
        /// The URI to view the detail page for a single job.
        /// </summary>
        string JobDetailViewUri { get; }

        /// <summary>
        /// CSS selector used by Selenium to find the element with the job ID.
        /// </summary>
        string JobIdCssSelector { get; }

        /// <summary>
        /// Name of the DOM attribute that contains the job ID.
        /// </summary>
        string JobIdDomAttributeName { get; }

        /// <summary>
        /// Allows the user to authenticate with a cookie. 
        /// This is useful for sites that require a login to view job posts.
        /// </summary>
        /// <param name="cookieName">Name of cookie. Indeed: "PPID"; LinkedIn: "li_at".</param>
        /// <param name="cookieValue">Value of the cookie</param>
        void AuthenticateWithCookie(string cookieName, string cookieValue);

        /// <summary>
        /// Checks if there is another page of search results to view.
        /// </summary>
        /// <returns>True if another page exists</returns>
        bool CheckIfNextPageExists();

        /// <summary>
        /// Navigates the WebDriver to the next page of search results.
        /// </summary>
        void NavigateToNextPage();

        /// <summary>
        /// Initialises the scraper.
        /// </summary>
        /// <param name="cookie">Auth cookie</param>
        void Initialise(string cookieName, string cookieValue);

        /// <summary>
        /// Scrapes the job ID numbers from all job post cards on a single search result page.
        /// </summary>
        /// <returns>List of Job IDs</returns>
        List<string> ScrapeJobIdsOnCurrentPage();

        /// <summary>
        /// Scrapes the text content for a single job post.
        /// </summary>
        /// <param name="jobId">ID of the job.</param>
        /// <returns><see cref="Job"/></returns>
        Job ScrapeJob(string jobId);

        /// <summary>
        /// Scrapes the text content for all job posts for the job IDs stored in the JobIds property.
        /// </summary>
        /// <param name="antiBotDelay">Delay between scraping each job post to prevent bot detection.</param>
        void ScrapeAllJobs(int antiBotDelay);

        /// <summary>
        /// Scrapes the text content for a specified list of jobs.
        /// </summary>
        /// <param name="jobIds"></param>
        /// <param name="antiBotDelay">Delay between scraping each job post to prevent bot detection.</param>
        void ScrapeSpecifiedJobs(List<string> jobIds, int antiBotDelay);

        /// <summary>
        /// Searches for jobs using URL parameters.
        /// </summary>
        /// <param name="jobTitle">Job title search keywords</param>
        /// <param name="location">Location to search for jobs</param>
        void SearchForJobsViaUrlParams(string jobTitle, string location);
    }
}
