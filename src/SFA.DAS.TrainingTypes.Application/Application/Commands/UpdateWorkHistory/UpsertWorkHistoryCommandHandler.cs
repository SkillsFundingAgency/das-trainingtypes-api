using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.WorkExperience;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpdateWorkHistory;

public class UpsertWorkHistoryCommandHandler(IWorkHistoryRepository repository, IApplicationRepository applicationRepository) : IRequestHandler<UpsertWorkHistoryCommand, UpsertWorkHistoryCommandResponse>
{
    public async Task<UpsertWorkHistoryCommandResponse> Handle(UpsertWorkHistoryCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.GetById(request.ApplicationId);
        if (application == null || application.CandidateId != request.CandidateId)
        {
            throw new InvalidOperationException($"Application {request.ApplicationId} not found");
        }

        var result = await repository.UpsertWorkHistory(request.WorkHistory, request.CandidateId);

        switch (request.WorkHistory.WorkHistoryType)
        {
            case WorkHistoryType.Job when application.JobsStatus is (short)SectionStatus.NotStarted or (short)SectionStatus.PreviousAnswer:
                application.JobsStatus = (short)SectionStatus.InProgress;
                await applicationRepository.Update(application);
                break;
            case WorkHistoryType.WorkExperience when application.WorkExperienceStatus is (short)SectionStatus.NotStarted or (short)SectionStatus.PreviousAnswer:
                application.WorkExperienceStatus = (short)SectionStatus.InProgress;
                await applicationRepository.Update(application);
                break;
        }

        return new UpsertWorkHistoryCommandResponse
        {
            WorkHistory = result.Item1,
            IsCreated = result.Item2
        };
    }
}