using static WebScraper.Classes.Job;

namespace WebScraper.Utils
{
    static class JobContentTextParserUtil
    {
        public static (WorkArrangementEnum?, EmploymentTypeEnum?, ExperienceLevelEnum?) ParseLinkedInJobDetailsMetaData(string jobDetailsMetaData)
        {
            (WorkArrangementEnum? workArrangement, EmploymentTypeEnum? employmentType, ExperienceLevelEnum? experienceLevel) metaData = (null, null, null);

            // Work arrangement
            if (jobDetailsMetaData.Contains("Remote"))
            {
                metaData.workArrangement = WorkArrangementEnum.Remote;
            }
            else if (jobDetailsMetaData.Contains("Hybrid"))
            {
                metaData.workArrangement = WorkArrangementEnum.Hybrid;
            }
            else if (jobDetailsMetaData.Contains("On-site"))
            {
                metaData.workArrangement = WorkArrangementEnum.Onsite;
            }

            // Employment type
            if (jobDetailsMetaData.Contains("Temporary"))
            {
                metaData.employmentType = EmploymentTypeEnum.Temporary;
            }
            else if (jobDetailsMetaData.Contains("Contract"))
            {
                metaData.employmentType = EmploymentTypeEnum.Contract;
            }
            else if (jobDetailsMetaData.Contains("Full-time"))
            {
                metaData.employmentType = EmploymentTypeEnum.Fulltime;
            }
            else if (jobDetailsMetaData.Contains("Part-time"))
            {
                metaData.employmentType = EmploymentTypeEnum.Parttime;
            }

            // Experience level
            if (jobDetailsMetaData.Contains("Internship"))
            {
                metaData.experienceLevel = ExperienceLevelEnum.Internship;
            }
            else if (jobDetailsMetaData.Contains("Entry level"))
            {
                metaData.experienceLevel = ExperienceLevelEnum.EntryLevel;
            }
            else if (jobDetailsMetaData.Contains("Mid-Senior level"))
            {
                metaData.experienceLevel = ExperienceLevelEnum.MidSeniorLevel;
            }

            return metaData;
        }

        public static (WorkArrangementEnum?, EmploymentTypeEnum?) ParseIndeedJobDetailsMetaData(string jobDetailsMetaData)
        {
            (WorkArrangementEnum? workArrangement, EmploymentTypeEnum? employmentType) metaData = (null, null);

            // Work arrangement
            if (jobDetailsMetaData.Contains("Remote"))
            {
                metaData.workArrangement = WorkArrangementEnum.Remote;
            }
            else if (jobDetailsMetaData.Contains("Hybrid"))
            {
                metaData.workArrangement = WorkArrangementEnum.Hybrid;
            }
            else
            {
                metaData.workArrangement = WorkArrangementEnum.Onsite;
            }

            // Employment type
            if (jobDetailsMetaData.Contains("Full-time"))
            {
                metaData.employmentType = EmploymentTypeEnum.Fulltime;
            }
            else if (jobDetailsMetaData.Contains("Part-time"))
            {
                metaData.employmentType = EmploymentTypeEnum.Parttime;
            }


            return metaData;
        }
    }
}
