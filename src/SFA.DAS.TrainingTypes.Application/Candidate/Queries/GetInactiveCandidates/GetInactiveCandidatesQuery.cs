using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetInactiveCandidates
{
    public record GetInactiveCandidatesQuery(
        DateTime CutOffDateTime,
        int PageNumber,
        int PageSize) : IRequest<GetInactiveCandidatesQueryResult>;
}