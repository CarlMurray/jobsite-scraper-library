using OpenQA.Selenium;
using WebScraper.Interfaces;
using WebScraper.Utils;
using static WebScraper.Classes.Job;

namespace WebScraper.Classes
{
    /// <summary>
    /// Class used to handle scraping of job data from LinkedIn
    /// </summary>
    public class LinkedInJobScraper : JobScraper, IJobScraper
    {
        public override string JobIdCssSelector { get; }
        public override string JobIdDomAttributeName { get; }
        public override string JobDetailViewUri { get; }
        public override string RootDomain { get; }

        public LinkedInJobScraper() : base()
        {
            JobIdCssSelector = "[data-occludable-job-id]";
            JobIdDomAttributeName = "data-occludable-job-id";
            JobDetailViewUri = "/jobs/view/";
            RootDomain = "https://linkedin.com";
        }

        /// <summary>
        /// Scrapes text content from the job post for the specified jobId
        /// </summary>
        /// <param name="jobId">The unique job ID as specified in the URL for the job post</param>
        public override Job ScrapeJob(string jobId)
        {
            // Navigates to the job post page and scrapes the text content
            Driver.Navigate().GoToUrl($"{RootDomain}{JobDetailViewUri}{jobId}");
            var jobContent = Driver.FindElement(By.TagName("main")).Text;
            string jobTitle = Driver.FindElement(By.CssSelector("main h1")).Text;
            string jobDescription = JavaScriptUtil.GetTextContentFromScript("#job-details", Driver);
            var jobDetailsMetaData = JavaScriptUtil.GetTextContentFromScript("li.job-details-jobs-unified-top-card__job-insight.job-details-jobs-unified-top-card__job-insight--highlight", Driver);
            (WorkArrangementEnum? workArrangement, EmploymentTypeEnum? employmentType, ExperienceLevelEnum? experienceLevel) = JobContentTextParserUtil.ParseLinkedInJobDetailsMetaData(jobDetailsMetaData);
            var job = new Job(jobTitle, jobDescription, jobId, workArrangement, experienceLevel, employmentType);

            // Adds the scraped job data to the JobResults property.
            JobResults.Add(job);

            return job;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void SearchForJobsViaUrlParams(string jobTitle, string location)
        {
            Driver.Navigate().GoToUrl($"{RootDomain}/jobs/search/?keywords={jobTitle}&location={location}");
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override bool CheckIfNextPageExists()
        {

            IWebElement? currentPageButton;
            IWebElement? nextPageButton;
            try
            {
                currentPageButton = Driver.FindElement(By.CssSelector("[data-test-pagination-page-btn].active.selected"));
            }
            catch
            {
                currentPageButton = null;
            }
            string currentPageNumber = JavaScriptUtil.GetTextContentFromScript("[data-test-pagination-page-btn].active.selected", Driver);
            try
            {
                nextPageButton = Driver.FindElement(By.CssSelector($"[data-test-pagination-page-btn='{currentPageNumber + 1}']"));
            }
            catch
            {
                nextPageButton = null;
            }

            if (nextPageButton is not null)
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
            IWebElement? currentPageButton;
            IWebElement? nextPageButton;
            currentPageButton = Driver.FindElement(By.CssSelector("[data-test-pagination-page-btn].active.selected"));
            string currentPageNumber = JavaScriptUtil.GetTextContentFromScript("[data-test-pagination-page-btn].active.selected", Driver);

            nextPageButton = Driver.FindElement(By.CssSelector($"[data-test-pagination-page-btn='{currentPageNumber + 1}']"));
            nextPageButton.Click();
        }

        /// <summary>
        /// Logs in by mimicing human behaviour by typing inputs, pausing etc.
        /// </summary>
        /// <param name="username">Email address</param>
        /// <param name="password">Password</param>
        public void SignInToLinkedInLikeAHuman(string username, string password)
        {
            Driver.Url = "https://linkedin.com/login";
            var emailInput = Driver.FindElement(By.Id("username"));
            var passwordInput = Driver.FindElement(By.Id("password"));
            Thread.Sleep(1000);
            MimicHumanUtil.EnterTextInputLikeHuman(username, emailInput);
            MimicHumanUtil.EnterTextInputLikeHuman(password, passwordInput);
            var signInCta = Driver.FindElement(By.CssSelector("[data-litms-control-urn=\"login-submit\"]"));
            signInCta.Click();
        }

        /// <summary>
        /// Clicks the "Jobs" button/link in the navigation to navigate to the main LinkedIn Jobs search page.
        /// </summary>
        public void ClickJobsNavButtonLikeAHuman()
        {
            Driver.FindElement(By.CssSelector("[href=\"https://www.linkedin.com/jobs/?\"]")).Click();
        }

        /// <summary>
        /// Mimics human behaviour to search for a specified query (job role).
        /// </summary>
        /// <param name="searchQuery">Job role you want to search for e.g. 'software developer', 'accountant' etc.</param>
        public void SearchForJobsLikeAHuman(string searchQuery, string location)
        {
            // Find and clear job search text inputs for keywords and location
            var keywordsInput = Driver.FindElement(By.CssSelector(".jobs-search-box__inner input"));
            var locationInput = Driver.FindElement(By.CssSelector("input[autocomplete=\"address-level2\"]"));
            keywordsInput.Clear();
            locationInput.Clear();

            // Enter search terms mimicing human behaviour
            MimicHumanUtil.EnterTextInputLikeHuman(searchQuery, keywordsInput);
            MimicHumanUtil.EnterTextInputLikeHuman(location, locationInput);
            locationInput.SendKeys(Keys.Enter);
        }
    }
}
