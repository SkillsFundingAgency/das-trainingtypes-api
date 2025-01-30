using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.Qualification;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteQualificationsByReferenceId;

public class DeleteQualificationsByReferenceIdCommandHandler(IQualificationRepository qualificationRepository, IApplicationRepository applicationRepository) : IRequestHandler<DeleteQualificationsByReferenceIdCommand, Unit>
{
    public async Task<Unit> Handle(DeleteQualificationsByReferenceIdCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.GetById(request.ApplicationId);

        if (application == null || application.CandidateId != request.CandidateId)
        {
            throw new InvalidOperationException($"Application {request.ApplicationId} not found");
        }

        if (application.QualificationsStatus is (short)SectionStatus.PreviousAnswer)
        {
            application.QualificationsStatus = (short)SectionStatus.InProgress;
            await applicationRepository.Update(application);
        }

        await qualificationRepository.DeleteCandidateApplicationQualificationsByReferenceId(request.CandidateId,
            request.ApplicationId, request.QualificationReferenceId);

        return new Unit();
    }
}