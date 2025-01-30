using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Commands.DeleteSavedVacancy
{
    public record DeleteSavedVacancyCommand(Guid CandidateId, string VacancyReference) : IRequest<Unit>;
}
