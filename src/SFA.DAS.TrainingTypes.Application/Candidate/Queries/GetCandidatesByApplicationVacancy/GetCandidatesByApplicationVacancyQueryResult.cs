using SFA.DAS.CandidateAccount.Domain.Candidate;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetCandidatesByApplicationVacancy;

public class GetCandidatesByApplicationVacancyQueryResult
{
    public List<ApplicationEntity> Candidates { get; set; }
}