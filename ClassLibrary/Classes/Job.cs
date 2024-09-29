namespace WebScraper.Classes
{
    /// <summary>
    /// Represents an individual job
    /// </summary>
    public class Job
    {
        private string? _workArrangement;
        private string? _experienceLevel;
        private string? _employmentType;

        /// <summary>
        /// The job title in a job post.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// The job description in a job post.
        /// </summary>
        public string JobDescription { get; set; }

        /// <summary>
        /// The job ID used by the job site as a unique identifier for the job post.
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// Describes if role is remote, on-site, hybrid, etc. May be null if not in job post.
        /// </summary>
        public string? WorkArrangement
        {
            get => _workArrangement;

            // Checks if value is valid enum, else null
            set
            {
                if (Enum.TryParse<WorkArrangementEnum>(value, true, out WorkArrangementEnum result))
                {
                    _workArrangement = result.ToString();
                }
                else
                {
                    _workArrangement = null;
                }
            }
        }

        /// <summary>
        /// Describes job experience requirement. May be null if not in job post.
        /// </summary>
        public string? ExperienceLevel
        {
            get => _experienceLevel;

            // Checks if value is valid enum, else null
            set
            {
                if (Enum.TryParse<ExperienceLevelEnum>(value, true, out ExperienceLevelEnum result))
                {
                    _experienceLevel = result.ToString();
                }
                else
                {
                    _experienceLevel = null;
                }
            }
        }

        /// <summary>
        /// Describes job contract type - full-time, part-time, etc.  May be null if not in job post.
        /// </summary>
        public string? EmploymentType
        {
            get => _employmentType;

            // Checks if value is valid enum, else null
            set
            {
                if (Enum.TryParse<EmploymentTypeEnum>(value, true, out EmploymentTypeEnum result))
                {

                    _employmentType = result.ToString();
                }
                else
                {
                    _employmentType = null;
                }
            }
        }

        public Job(string jobTitle, string jobDescription, string jobId, WorkArrangementEnum? workArrangement = null, ExperienceLevelEnum? experienceLevel = null, EmploymentTypeEnum? employmentType = null)
        {
            JobTitle = jobTitle;
            JobDescription = jobDescription;
            JobId = jobId;
            WorkArrangement = (workArrangement != null) ? workArrangement.ToString() : null;
            ExperienceLevel = (experienceLevel != null) ? experienceLevel.ToString() : null;
            EmploymentType = (employmentType != null) ? employmentType.ToString() : null;
        }

        public enum WorkArrangementEnum
        {
            Remote,
            Hybrid,
            Onsite
        }

        public enum ExperienceLevelEnum
        {
            Internship,
            EntryLevel,
            Associate,
            MidSeniorLevel,
            Director,
            Executive
        }

        public enum EmploymentTypeEnum
        {
            Contract,
            Temporary,
            Parttime,
            Fulltime
        }
    }
}
