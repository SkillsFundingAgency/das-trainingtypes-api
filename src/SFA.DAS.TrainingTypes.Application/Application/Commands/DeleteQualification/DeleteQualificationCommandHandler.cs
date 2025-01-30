using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.Qualification;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteQualification;

public class DeleteQualificationCommandHandler(IQualificationRepository qualificationRepository, IApplicationRepository applicationRepository) : IRequestHandler<DeleteQualificationCommand, Unit>
{
    public async Task<Unit> Handle(DeleteQualificationCommand request, CancellationToken cancellationToken)
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

        await qualificationRepository.DeleteCandidateApplicationQualificationById(request.CandidateId,
            request.ApplicationId, request.Id);

        return new Unit();
    }
}