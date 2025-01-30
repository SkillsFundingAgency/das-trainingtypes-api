using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.WorkExperience;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteWorkHistory
{
    public class DeleteWorkHistoryCommandHandler(IWorkHistoryRepository workHistoryRepository, IApplicationRepository applicationRepository) : IRequestHandler<DeleteWorkHistoryCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteWorkHistoryCommand command, CancellationToken cancellationToken)
        {
            var application = await applicationRepository.GetById(command.ApplicationId);

            if (application == null || application.CandidateId != command.CandidateId)
            {
                throw new InvalidOperationException($"Application {command.ApplicationId} not found");
            }

            if (application.JobsStatus is (short)SectionStatus.PreviousAnswer)
            {
                application.JobsStatus = (short)SectionStatus.InProgress;
                await applicationRepository.Update(application);
            }

            await workHistoryRepository.Delete(command.ApplicationId, command.JobId, command.CandidateId);

            return Unit.Value;
        }
    }
}
