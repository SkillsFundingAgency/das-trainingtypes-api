using MediatR;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetWorkHistoryItem
{
    public class GetWorkHistoryItemQuery : IRequest<GetWorkHistoryItemQueryResult?>
    {
        public Guid ApplicationId { get; init; }
        public Guid CandidateId { get; set; }
        public WorkHistoryType? WorkHistoryType { get; set; }
        public Guid Id { get; set; }
    }
}
