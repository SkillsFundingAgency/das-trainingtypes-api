using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplication;

public class GetApplicationQueryHandler(IApplicationRepository applicationRepository) : IRequestHandler<GetApplicationQuery, GetApplicationQueryResult>
{
    public async Task<GetApplicationQueryResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
    {
        var applicationEntity = await applicationRepository.GetById(request.ApplicationId, request.IncludeDetail);

        if (applicationEntity == null)
        {
            return new GetApplicationQueryResult();
        }

        if (applicationEntity.CandidateId != request.CandidateId)
        {
            return null;
        }

        var application = request.IncludeDetail ? (ApplicationDetail)applicationEntity : (Domain.Application.Application)applicationEntity;

        return new GetApplicationQueryResult
        {
            Application = application
        };
    }
}