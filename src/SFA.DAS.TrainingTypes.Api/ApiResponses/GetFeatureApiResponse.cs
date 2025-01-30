using SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationWorkHistories;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Api.ApiResponses
{
    public class GetFeatureApiResponse
    {
        public IEnumerable<WorkHistoryItem> WorkHistories { get; init; } = [];

        public static implicit operator GetFeatureApiResponse(GetApplicationWorkHistoriesQueryResult source)
        {
            return new GetFeatureApiResponse
            {
                WorkHistories = source.WorkHistories.Select(entity => (WorkHistoryItem)entity)
            };
        }


    }
    public class WorkHistoryItem
    {
        public Guid Id { get; set; }
        public WorkHistoryType WorkHistoryType { get; set; }
        public string? Employer { get; set; }
        public string? JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string? Description { get; set; }

        public static implicit operator WorkHistoryItem(WorkHistory source)
        {
            return new WorkHistoryItem
            {
                Id = source.Id,
                WorkHistoryType = source.WorkHistoryType,
                Employer = source.Employer,
                JobTitle = source.JobTitle,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                ApplicationId = source.ApplicationId,
                Description = source.Description,
            };
        }
    }
}
