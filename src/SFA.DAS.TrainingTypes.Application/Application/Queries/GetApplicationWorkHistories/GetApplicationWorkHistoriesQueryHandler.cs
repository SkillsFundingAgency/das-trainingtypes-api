using MediatR;
using SFA.DAS.CandidateAccount.Data.WorkExperience;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationWorkHistories;

public record GetApplicationWorkHistoriesQueryHandler(IWorkHistoryRepository WorkHistoryRepository) : IRequestHandler<GetApplicationWorkHistoriesQuery, GetApplicationWorkHistoriesQueryResult>
{
    public async Task<GetApplicationWorkHistoriesQueryResult> Handle(GetApplicationWorkHistoriesQuery request, CancellationToken cancellationToken)
    {
        return await WorkHistoryRepository.GetAll(request.ApplicationId, request.CandidateId, request.WorkHistoryType, cancellationToken);
    }
}