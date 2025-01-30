using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Api.ApiRequests
{
    public class WorkHistoryRequest
    {
        public WorkHistoryType WorkHistoryType { get; set; }
        public string EmployerName { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
