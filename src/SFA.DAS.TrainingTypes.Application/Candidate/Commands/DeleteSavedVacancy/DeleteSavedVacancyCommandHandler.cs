using MediatR;
using SFA.DAS.CandidateAccount.Data.SavedVacancy;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Commands.DeleteSavedVacancy
{
    public record DeleteSavedVacancyCommandHandler(ISavedVacancyRepository Repository) : IRequestHandler<DeleteSavedVacancyCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteSavedVacancyCommand command, CancellationToken cancellationToken)
        {
            var result = await Repository.Get(command.CandidateId, command.VacancyReference);

            if (result != null)
            {
                await Repository.Delete(result);
            }

            return Unit.Value;
        }
    }
}