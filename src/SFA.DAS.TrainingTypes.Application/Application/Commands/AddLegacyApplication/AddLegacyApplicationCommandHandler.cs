using MediatR;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.ReferenceData;
using SFA.DAS.CandidateAccount.Domain.Candidate;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.AddLegacyApplication;

public class AddLegacyApplicationCommandHandler(IApplicationRepository applicationRepository, IQualificationReferenceRepository qualificationReferenceRepository) : IRequestHandler<AddLegacyApplicationCommand, AddLegacyApplicationCommandResponse>
{
    public async Task<AddLegacyApplicationCommandResponse> Handle(AddLegacyApplicationCommand request, CancellationToken cancellationToken)
    {
        var entity = await CreateApplicationEntity(request.LegacyApplication);

        var result = await applicationRepository.Upsert(entity);

        return new AddLegacyApplicationCommandResponse
        {
            Id = result.Item1.Id
        };
    }

    private async Task<ApplicationEntity> CreateApplicationEntity(LegacyApplication legacyApplication)
    {
        var qualificationReferences = (await qualificationReferenceRepository.GetAll()).ToList();

        var additionalQuestions = new List<AdditionalQuestionEntity>();
        if (legacyApplication.AdditionalQuestion1 != null)
        {
            additionalQuestions.Add(new AdditionalQuestionEntity
            {
                QuestionText = legacyApplication.AdditionalQuestion1,
                Answer = legacyApplication.AdditionalQuestion1Answer ?? string.Empty
            });
        }
        if (legacyApplication.AdditionalQuestion2 != null)
        {
            additionalQuestions.Add(new AdditionalQuestionEntity
            {
                QuestionText = legacyApplication.AdditionalQuestion2,
                Answer = legacyApplication.AdditionalQuestion2Answer ?? string.Empty
            });
        }

        return new ApplicationEntity
        {
            Id = legacyApplication.Id,
            CandidateId = legacyApplication.CandidateId,
            VacancyReference = legacyApplication.VacancyReference,
            Status = (short)legacyApplication.Status,
            SubmittedDate = legacyApplication.DateApplied,
            ResponseDate = legacyApplication.Status switch
            {
                ApplicationStatus.Successful => legacyApplication.SuccessfulDateTime,
                ApplicationStatus.UnSuccessful => legacyApplication.UnsuccessfulDateTime,
                _ => null
            },
            ResponseNotes = legacyApplication.Status == ApplicationStatus.UnSuccessful ?
                legacyApplication.UnsuccessfulReason : string.Empty,
            MigrationDate = DateTime.UtcNow,
            Strengths = legacyApplication.SkillsAndStrengths,
            Support = legacyApplication.Support,
            AdditionalQuestionEntities = additionalQuestions,
            SkillsAndStrengthStatus = (short)SectionStatus.Incomplete,
            InterviewAdjustmentsStatus = (short)SectionStatus.Incomplete,
            DisabilityConfidenceStatus = legacyApplication.IsDisabilityConfident
                ? (short)SectionStatus.NotStarted
                : (short)SectionStatus.NotRequired,
            JobsStatus = (short)SectionStatus.Incomplete,
            QualificationsStatus = (short)SectionStatus.Incomplete,
            TrainingCoursesStatus = (short)SectionStatus.Incomplete,
            AdditionalQuestion1Status = additionalQuestions.Count > 0
                ? (short)SectionStatus.Incomplete
                : (short)SectionStatus.NotRequired,
            AdditionalQuestion2Status = additionalQuestions.Count > 1
                ? (short)SectionStatus.Incomplete
                : (short)SectionStatus.NotRequired,
            TrainingCourseEntities = legacyApplication.TrainingCourses.Select(x => new TrainingCourseEntity
            {
                Title = x.Title,
                ToYear = x.ToDate.Year
            }).ToList(),
            WorkHistoryEntities = legacyApplication.WorkExperience.Select(x => new WorkHistoryEntity
            {
                Employer = x.Employer,
                JobTitle = x.JobTitle,
                Description = x.Description,
                StartDate = x.FromDate == DateTime.MinValue ? DateTime.UtcNow : x.FromDate,
                EndDate = x.ToDate == DateTime.MinValue ? null : x.ToDate,
                WorkHistoryType = (byte)WorkHistoryType.Job
            }).ToList(),
            QualificationEntities = legacyApplication.Qualifications
                .Select(x => MapQualification(x, qualificationReferences)).ToList()
        };
    }

    private QualificationEntity MapQualification(LegacyApplication.Qualification source, List<QualificationReferenceEntity> qualificationReferences)
    {
        var result = new QualificationEntity
        {
            QualificationReferenceId = GetQualificationType(qualificationReferences, source.QualificationType),
            Subject = source.Subject,
            Grade = source.Grade
        };

        switch (source.QualificationType)
        {
            case "GCSE":
            case "AS Level":
            case "A Level":
            case "BTEC":
                result.AdditionalInformation = string.Empty;
                result.IsPredicted = source.IsPredicted;
                break;
            default:
                result.Subject = source.QualificationType;
                result.AdditionalInformation = source.Subject;
                break;
        }

        return result;
    }

    private Guid GetQualificationType(List<QualificationReferenceEntity> qualificationReferences, string source)
    {
        switch (source)
        {
            case "GCSE":
                return qualificationReferences.Single(x => string.Equals(x.Name, "GCSE", StringComparison.OrdinalIgnoreCase)).Id;
            case "AS Level":
                return qualificationReferences.Single(x => string.Equals(x.Name, "AS LEVEL", StringComparison.OrdinalIgnoreCase)).Id;
            case "A Level":
                return qualificationReferences.Single(x => string.Equals(x.Name, "A LEVEL", StringComparison.OrdinalIgnoreCase)).Id;
            case "BTEC":
                return qualificationReferences.Single(x => string.Equals(x.Name, "BTEC", StringComparison.OrdinalIgnoreCase)).Id;
            case "NVQ or SVQ Level 1":
            case "NVQ or SVQ Level 2":
            case "NVQ or SVQ Level 3":
            default:
                return qualificationReferences.Single(x => string.Equals(x.Name, "OTHER", StringComparison.OrdinalIgnoreCase)).Id;
        }
    }
}