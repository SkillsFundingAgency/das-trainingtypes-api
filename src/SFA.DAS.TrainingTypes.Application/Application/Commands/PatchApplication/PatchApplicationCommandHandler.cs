using System.ComponentModel.DataAnnotations;
using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.TrainingTypes.Domain.Application;
using ValidationResult = SFA.DAS.TrainingTypes.Domain.RequestHandlers.ValidationResult;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.PatchApplication;

public class PatchApplicationCommandHandler(IApplicationRepository applicationRepository) : IRequestHandler<PatchApplicationCommand, PatchApplicationCommandResponse>
{
    public async Task<PatchApplicationCommandResponse> Handle(PatchApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await applicationRepository.GetById(request.Id);

        if (application == null)
        {
            return new PatchApplicationCommandResponse();
        }

        if (application.CandidateId != request.CandidateId)
        {
            var validationResult = new ValidationResult();
            validationResult.AddError(nameof(application.CandidateId), "Application does not belong to candidate");
            throw new ValidationException(validationResult.DataAnnotationResult, null, null);
        }

        var patchedDoc = (Domain.Application.PatchApplication)application;

        request.Patch.ApplyTo(patchedDoc);

        application.Status = (short)patchedDoc.Status;
        application.TrainingCoursesStatus = (short)patchedDoc.TrainingCoursesStatus;
        application.QualificationsStatus = (short)patchedDoc.QualificationsStatus;
        application.JobsStatus = (short)patchedDoc.JobsStatus;
        application.DisabilityConfidenceStatus = (short)patchedDoc.DisabilityConfidenceStatus;
        application.SkillsAndStrengthStatus = (short)patchedDoc.SkillsAndStrengthStatus;
        application.InterviewAdjustmentsStatus = (short)patchedDoc.InterviewAdjustmentsStatus;
        application.AdditionalQuestion1Status = (short)patchedDoc.AdditionalQuestion1Status;
        application.AdditionalQuestion2Status = (short)patchedDoc.AdditionalQuestion2Status;
        application.InterestsStatus = (short)patchedDoc.InterestsStatus;
        application.WorkExperienceStatus = (short)patchedDoc.WorkExperienceStatus;
        application.WhatIsYourInterest = patchedDoc.WhatIsYourInterest;
        application.UpdatedDate = DateTime.UtcNow;
        application.ResponseNotes = patchedDoc.ResponseNotes;
        application.Support = patchedDoc.Support;
        application.Strengths = patchedDoc.Strengths;

        switch (patchedDoc.Status)
        {
            case ApplicationStatus.Successful or ApplicationStatus.UnSuccessful:
                application.ResponseDate = DateTime.UtcNow;
                break;
            case ApplicationStatus.Submitted:
                application.SubmittedDate = DateTime.UtcNow;
                break;
            case ApplicationStatus.Withdrawn:
                application.WithdrawnDate = DateTime.UtcNow;
                break;
        }

        if (application.ApplyUnderDisabilityConfidentScheme != patchedDoc.ApplyUnderDisabilityConfidentScheme)
        {
            application.ApplyUnderDisabilityConfidentScheme = patchedDoc.ApplyUnderDisabilityConfidentScheme;
            if (application.DisabilityConfidenceStatus is (short)SectionStatus.NotStarted or (short)SectionStatus.PreviousAnswer)
            {
                application.DisabilityConfidenceStatus = (short)SectionStatus.InProgress;
            }
        }

        var updatedApplication = await applicationRepository.Update(application);

        return new PatchApplicationCommandResponse
        {
            Application = updatedApplication
        };
    }
}