using MediatR;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationWorkHistories;

public record GetApplicationWorkHistoriesQuery : IRequest<GetApplicationWorkHistoriesQueryResult>
{
    public Guid ApplicationId { get; init; }
    public Guid CandidateId { get; set; }
    public WorkHistoryType? WorkHistoryType { get; set; }
}