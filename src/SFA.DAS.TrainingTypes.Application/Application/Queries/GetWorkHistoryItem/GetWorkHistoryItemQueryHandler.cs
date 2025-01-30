using MediatR;
using SFA.DAS.CandidateAccount.Data.WorkExperience;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetWorkHistoryItem;

public class GetWorkHistoryItemQueryHandler(IWorkHistoryRepository WorkHistoryRepository) : IRequestHandler<GetWorkHistoryItemQuery, GetWorkHistoryItemQueryResult?>
{
    public async Task<GetWorkHistoryItemQueryResult?> Handle(GetWorkHistoryItemQuery request, CancellationToken cancellationToken)
    {
        var result = await WorkHistoryRepository.Get(request.ApplicationId, request.CandidateId, request.Id, request.WorkHistoryType, cancellationToken);
        return result == null ? null : (GetWorkHistoryItemQueryResult)result;
    }
}