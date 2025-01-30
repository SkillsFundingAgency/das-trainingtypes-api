using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetCandidatesByApplicationVacancy;

public class GetCandidatesByApplicationVacancyQueryHandler(IApplicationRepository applicationRepository) : IRequestHandler<GetCandidatesByApplicationVacancyQuery, GetCandidatesByApplicationVacancyQueryResult>
{
    public async Task<GetCandidatesByApplicationVacancyQueryResult> Handle(GetCandidatesByApplicationVacancyQuery request, CancellationToken cancellationToken)
    {
        var applications = await applicationRepository.GetApplicationsByVacancyReference(request.VacancyReference,
            request.StatusId, request.PreferenceId, request.CanEmailOnly);

        return new GetCandidatesByApplicationVacancyQueryResult
        {
            Candidates = applications.ToList()
        };
    }
}