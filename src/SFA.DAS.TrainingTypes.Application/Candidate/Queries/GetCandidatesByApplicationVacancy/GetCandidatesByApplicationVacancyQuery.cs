using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetCandidatesByApplicationVacancy;

public class GetCandidatesByApplicationVacancyQuery : IRequest<GetCandidatesByApplicationVacancyQueryResult>
{
    public required string VacancyReference { get; set; }
    public short? StatusId { get; set; }
    public Guid? PreferenceId { get; set; }
    public bool CanEmailOnly { get; set; }
}