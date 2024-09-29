using OpenQA.Selenium;
using WebScraper.Enums;
using WebScraper.Interfaces;
using WebScraper.Utils;

namespace WebScraper.Classes
{
    /// <summary>
    /// Class used to handle scraping of job data from Indeed
    /// </summary>
    public class IndeedJobScraper : JobScraper, IJobScraper
    {

        public override string JobIdCssSelector { get; }
        public override string JobIdDomAttributeName { get; }
        public override string JobDetailViewUri { get; }
        public override string RootDomain => GetRootDomain();
        public Country Country { get; set; }

        public IndeedJobScraper() : base()
        {
            JobIdCssSelector = "li a[data-jk]";
            JobIdDomAttributeName = "data-jk";
            JobDetailViewUri = "/viewjob?jk=";
            Country = Country.Ireland; // Default
        }

        /// <summary>
        /// Allows the user to specify the country for the Indeed job site.
        /// </summary>
        /// <param name="country"></param>
        public IndeedJobScraper(Country country) : this()
        {
            Country = country;
        }

        /// <summary>
        /// Scrapes the job details for the given job ID.
        /// </summary>
        /// <param name="jobId">The ID of the job to scrape.</param>
        public override Job ScrapeJob(string jobId)
        {
            Driver.Navigate().GoToUrl(RootDomain + JobDetailViewUri + jobId);
            string jobTitle = JavaScriptUtil.GetTextContentFromScript("h1", Driver);
            string jobDescription = JavaScriptUtil.GetTextContentFromScript("#jobDescriptionText", Driver);
            Job job = new Job(jobTitle, jobDescription, jobId);

            // Adds the scraped job data to the JobResults property.
            JobResults.Add(job);
            return job;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="antiBotDelay">Default to 30 seconds to avoid Indeed bot detection.</param>
        public override void ScrapeAllJobs(int antiBotDelay = 30000)
        {
            base.ScrapeAllJobs(antiBotDelay);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="jobIds">List of Job IDs to scrape the jobs for.</param>
        /// <param name="antiBotDelay">Default to 30 seconds to avoid Indeed bot detection.</param>
        public override void ScrapeSpecifiedJobs(List<string> jobIds, int antiBotDelay = 30000)
        {
            base.ScrapeSpecifiedJobs(jobIds, antiBotDelay);
        }

        /// <summary>
        /// Gets the root domain for the Indeed job site based on the country, if provided in the constructor.
        /// </summary>
        /// <returns>The root domain for the Indeed job site.</returns>
        public string GetRootDomain()
        {
            string rootDomain;
            switch (Country)
            {
                case Country.UnitedStates:
                    rootDomain = "https://www.indeed.com";
                    break;
                case Country.UnitedKingdom:
                    rootDomain = "https://www.indeed.co.uk";
                    break;
                case Country.Canada:
                    rootDomain = "https://www.indeed.ca";
                    break;
                case Country.Australia:
                    rootDomain = "https://www.indeed.com.au";
                    break;
                case Country.India:
                    rootDomain = "https://www.indeed.co.in";
                    break;
                case Country.France:
                    rootDomain = "https://www.indeed.fr";
                    break;
                case Country.Germany:
                    rootDomain = "https://www.indeed.de";
                    break;
                case Country.Netherlands:
                    rootDomain = "https://www.indeed.nl";
                    break;
                case Country.Japan:
                    rootDomain = "https://www.indeed.jp";
                    break;
                case Country.Ireland:
                    rootDomain = "https://www.indeed.ie";
                    break;
                case Country.Brazil:
                    rootDomain = "https://www.indeed.com.br";
                    break;
                case Country.Mexico:
                    rootDomain = "https://www.indeed.mx";
                    break;
                case Country.Italy:
                    rootDomain = "https://www.indeed.it";
                    break;
                case Country.Spain:
                    rootDomain = "https://www.indeed.es";
                    break;
                case Country.Singapore:
                    rootDomain = "https://www.indeed.sg";
                    break;
                case Country.Switzerland:
                    rootDomain = "https://www.indeed.ch";
                    break;
                case Country.UAE:
                    rootDomain = "https://www.indeed.ae";
                    break;
                default:
                    rootDomain = "https://www.indeed.ie"; // Defaulting to Ireland if no match
                    break;
            }
            return rootDomain;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override bool CheckIfNextPageExists()
        {
            if (Driver.FindElement(By.CssSelector("a[data-testid=\"pagination-page-next\"]")) is not null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void NavigateToNextPage()
        {
            try
            {
                Driver.FindElement(By.CssSelector("a[data-testid=\"pagination-page-next\"]")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override void SearchForJobsViaUrlParams(string jobTitle, string location)
        {
            Driver.Navigate().GoToUrl($"{RootDomain}/jobs?q={jobTitle}&l={location}");
        }
    }
}