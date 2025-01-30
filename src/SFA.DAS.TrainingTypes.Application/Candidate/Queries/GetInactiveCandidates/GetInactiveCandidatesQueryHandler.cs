using MediatR;
using SFA.DAS.CandidateAccount.Data.Candidate;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetInactiveCandidates
{
    public class GetInactiveCandidatesQueryHandler(ICandidateRepository repository)
        : IRequestHandler<GetInactiveCandidatesQuery, GetInactiveCandidatesQueryResult>
    {
        public async Task<GetInactiveCandidatesQueryResult> Handle(GetInactiveCandidatesQuery request,
            CancellationToken cancellationToken)
        {
            return await repository.GetCandidatesByActivity(
                request.CutOffDateTime,
                request.PageNumber,
                request.PageSize,
                cancellationToken);
        }
    }
}