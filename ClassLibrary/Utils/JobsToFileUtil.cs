using System.Text.Json;
using WebScraper.Classes;

namespace WebScraper.Utils
{
    public static class JobsToFileUtil
    {
        /// <summary>
        /// Saves the Job object content to a JSON file.
        /// </summary>
        /// <param name="job">A Job object.</param>
        /// <param name="filename">Name of file to write to. File will be created if non-existant.</param>
        public static void WriteJobToFile(Job job, string filename)
        {
            File.AppendAllText(filename, JsonSerializer.Serialize(job));
        }

        /// <summary>
        /// Saves the Job IDs to a JSON file. Useful for saving progress in case of a crash.
        /// </summary>
        public static void SaveJobIdsToJsonFile(List<string> jobIds, string filename)
        {
            var json = JsonSerializer.Serialize(jobIds);
            File.WriteAllText(filename, json);
        }

        /// <summary>
        /// Saves the JobResults property value to a JSON file
        /// </summary>
        public static void SaveJobResultsToJsonFile(List<Job> jobResults, string filename)
        {
            File.WriteAllText(filename, JsonSerializer.Serialize(jobResults));
        }
    }
}
