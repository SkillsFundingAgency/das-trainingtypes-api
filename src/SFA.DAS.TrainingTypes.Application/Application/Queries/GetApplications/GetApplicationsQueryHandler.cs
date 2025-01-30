using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplications;

public class GetApplicationsQueryHandler(IApplicationRepository applicationRepository) : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
{
    public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var result = await applicationRepository.GetByCandidateId(request.CandidateId, (short?)request.Status);

        return new GetApplicationsQueryResult
        {
            Applications = result.Select(x => (Domain.Application.Application)x).ToList()
        };
    }
}