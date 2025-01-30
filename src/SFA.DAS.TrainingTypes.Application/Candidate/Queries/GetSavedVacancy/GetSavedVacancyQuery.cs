using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetSavedVacancy
{
    public record GetSavedVacancyQuery(Guid CandidateId, string VacancyReference)
        : IRequest<GetSavedVacancyQueryResult>;
}