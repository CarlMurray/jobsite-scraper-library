using WebScraper.Classes;
using WebScraper.Interfaces;
using WebScraper.Utils;

namespace WebScraper.Examples;

class Program
{
    void ScrapeLinkedIn()
    {
        IJobScraper jobScraper = new LinkedInJobScraper();
        jobScraper.Initialise("li_at", "YOUR_COOKIE_VALUE"); // ADD YOUR COOKIE
        jobScraper.SearchForJobsViaUrlParams("Software Engineer", "Dublin");
        jobScraper.ScrapeSpecifiedJobs(new List<string>() { "4034347232", "4034348540", "4034337985" }, 7500);
        JobsToFileUtil.SaveJobResultsToJsonFile(jobScraper.JobResults, "./linkedin-jobs.json");
    }

    void ScrapeIndeed()
    {
        IJobScraper jobScraper = new IndeedJobScraper();
        jobScraper.Initialise("PPID", "YOUR_COOKIE_VALUE"); // ADD YOUR COOKIE
        Thread.Sleep(2000); // Prevents bot detection
        jobScraper.SearchForJobsViaUrlParams("Software Engineer", "Dublin");
        List<string> jobIds = jobScraper.ScrapeJobIdsOnCurrentPage();
        foreach (var jobId in jobIds)
        {
            Console.WriteLine(jobId);
            Job job = jobScraper.ScrapeJob(jobId);
            JobsToFileUtil.WriteJobToFile(job, "./job-test.json");

            // IMPORTANT
            // If scraping multiple jobs, add a delay to avoid being blocked by the website.
            // Alternatively, use ScrapeAllJobs() or ScrapeSpecifiedJobs() methods which include a delay.
            Thread.Sleep(30000);
        }

        // A less verbose way of achieving the above
        // ------------------------------------------------
        //jobScraper.ScrapeSpecifiedJobs(jobIds, 30000);
        //foreach (var job in jobScraper.JobResults)
        //{
        //    JobsToFileUtil.WriteJobToFile(job, "./jobs.json");
        //}


        // This line writes the JobResults property to a file at once, rather than writing each job individually, as above.
        JobsToFileUtil.SaveJobResultsToJsonFile(jobScraper.JobResults, "./multiple-jobs-test.json");
    }
}