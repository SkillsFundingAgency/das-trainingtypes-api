using MediatR;
using SFA.DAS.CandidateAccount.Data.Candidate;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetCandidate;

public class GetCandidateQueryHandler(ICandidateRepository repository) : IRequestHandler<GetCandidateQuery, GetCandidateQueryResult>
{
    public async Task<GetCandidateQueryResult> Handle(GetCandidateQuery request, CancellationToken cancellationToken)
    {
        var candidate = await repository.GetByGovIdentifier(request.Id);

        if (candidate == null && Guid.TryParse(request.Id, out var id))
        {
            candidate = await repository.GetById(id);
        }

        return new GetCandidateQueryResult
        {
            Candidate = candidate
        };
    }
}