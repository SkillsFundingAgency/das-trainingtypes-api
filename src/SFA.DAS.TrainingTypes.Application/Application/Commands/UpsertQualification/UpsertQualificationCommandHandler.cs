using MediatR;
using SFA.DAS.CandidateAccount.Application.Application.Commands.UpsertTrainingCourse;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.Qualification;
using SFA.DAS.CandidateAccount.Data.ReferenceData;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertQualification;

public class UpsertQualificationCommandHandler(IQualificationRepository qualificationRepository, IApplicationRepository applicationRepository, IQualificationReferenceRepository qualificationReferenceRepository) : IRequestHandler<UpsertQualificationCommand, UpsertQualificationCommandResponse>
{
    public async Task<UpsertQualificationCommandResponse> Handle(UpsertQualificationCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.GetById(request.ApplicationId);
        if (application == null || application.CandidateId != request.CandidateId)
        {
            throw new InvalidOperationException($"Application {request.ApplicationId} not found");
        }

        var qualificationReference = await qualificationReferenceRepository.GetById(request.QualificationReferenceId);

        if (qualificationReference == null)
        {
            throw new InvalidOperationException($"Qualification Reference {request.QualificationReferenceId} not found");
        }

        request.Qualification.QualificationReference = qualificationReference;

        var result = await qualificationRepository.Upsert(request.Qualification, request.CandidateId, request.ApplicationId);

        if (application.QualificationsStatus is (short)SectionStatus.NotStarted or (short)SectionStatus.PreviousAnswer)
        {
            application.QualificationsStatus = (short)SectionStatus.InProgress;
            await applicationRepository.Update(application);
        }

        return new UpsertQualificationCommandResponse
        {
            Qualification = result.Item1,
            IsCreated = result.Item2
        };
    }
}