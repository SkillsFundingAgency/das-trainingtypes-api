using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Api.ApiRequests
{
    public class PutWorkHistoryItemRequest
    {
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public WorkHistoryType WorkHistoryType { get; set; }
    }
}
