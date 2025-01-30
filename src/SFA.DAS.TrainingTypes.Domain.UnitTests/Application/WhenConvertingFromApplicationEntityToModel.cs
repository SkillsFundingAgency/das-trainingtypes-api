using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Domain.UnitTests.Application;

public class WhenConvertingFromApplicationEntityToModel
{
    [Test, RecursiveMoqAutoData]
    public void Then_The_Fields_Are_Mapped(ApplicationEntity source)
    {
        source.JobsStatus = 4;
        source.QualificationsStatus = 2;
        source.WorkExperienceStatus = 1;
        source.TrainingCoursesStatus = 0;
        source.DisabilityConfidenceStatus = 1;
        source.Status = 2;
        source.SkillsAndStrengthStatus = 3;
        source.InterviewAdjustmentsStatus = 0;
        source.AdditionalQuestion2Status = 0;
        source.AdditionalQuestion1Status = 0;
        source.InterestsStatus = 0;

        var actual = (Domain.Application.Application)source;

        actual.Id.Should().Be(source.Id);
        actual.VacancyReference.Should().Be(source.VacancyReference);
        actual.CandidateId.Should().Be(source.CandidateId);
        actual.DisabilityStatus.Should().Be(source.DisabilityStatus);
        actual.JobsStatus.Should().Be(SectionStatus.NotRequired);
        actual.QualificationsStatus.Should().Be(SectionStatus.Incomplete);
        actual.WorkExperienceStatus.Should().Be(SectionStatus.InProgress);
        actual.TrainingCoursesStatus.Should().Be(SectionStatus.NotStarted);
        actual.DisabilityConfidenceStatus.Should().Be(SectionStatus.InProgress);
        actual.DisabilityConfidenceStatus.Should().Be(SectionStatus.InProgress);
        actual.Status.Should().Be(ApplicationStatus.Withdrawn);
        actual.SkillsAndStrengthStatus.Should().Be(SectionStatus.Completed);
        actual.InterviewAdjustmentsStatus.Should().Be(SectionStatus.NotStarted);
        actual.AdditionalQuestion2Status.Should().Be(SectionStatus.NotStarted);
        actual.AdditionalQuestion1Status.Should().Be(SectionStatus.NotStarted);
        actual.InterestsStatus.Should().Be(SectionStatus.NotStarted);
        actual.WhatIsYourInterest.Should().Be(source.WhatIsYourInterest);
        actual.ApplyUnderDisabilityConfidentScheme.Should().Be(source.ApplyUnderDisabilityConfidentScheme);
        actual.CreatedDate.Should().Be(source.CreatedDate);
        actual.SubmittedDate.Should().Be(source.SubmittedDate);
        actual.ResponseDate.Should().Be(source.ResponseDate);
        actual.ResponseNotes.Should().Be(source.ResponseNotes);
        actual.WithdrawnDate.Should().Be(source.WithdrawnDate);
    }

    [TestCase(SectionStatus.NotStarted, SectionStatus.NotStarted, SectionStatus.NotStarted)]
    [TestCase(SectionStatus.InProgress, SectionStatus.NotStarted, SectionStatus.InProgress)]
    [TestCase(SectionStatus.NotRequired, SectionStatus.NotStarted, SectionStatus.InProgress)]
    [TestCase(SectionStatus.NotStarted, SectionStatus.InProgress, SectionStatus.InProgress)]
    [TestCase(SectionStatus.NotStarted, SectionStatus.NotRequired, SectionStatus.InProgress)]
    [TestCase(SectionStatus.NotRequired, SectionStatus.NotRequired, SectionStatus.Completed)]
    [TestCase(SectionStatus.Completed, SectionStatus.NotRequired, SectionStatus.Completed)]
    [TestCase(SectionStatus.NotRequired, SectionStatus.Completed, SectionStatus.Completed)]
    public void Then_The_Section_States_Are_Calculated_For_Education_History(SectionStatus qualificationStatus, SectionStatus trainingCourseStatus, SectionStatus expectedStatus)
    {
        var source = new ApplicationEntity
        {
            QualificationsStatus = (short)qualificationStatus,
            TrainingCoursesStatus = (short)trainingCourseStatus,
            AdditionalQuestionEntities = new List<AdditionalQuestionEntity>()
        };

        var actual = (Domain.Application.Application)source;

        actual.EducationHistorySectionStatus.Should().Be(expectedStatus);
    }

    [TestCase(SectionStatus.NotStarted, SectionStatus.NotStarted, SectionStatus.NotStarted, SectionStatus.NotStarted, SectionStatus.NotStarted)]
    [TestCase(SectionStatus.NotRequired, SectionStatus.Completed, SectionStatus.NotRequired, SectionStatus.NotRequired, SectionStatus.Completed)]
    [TestCase(SectionStatus.Completed, SectionStatus.Completed, SectionStatus.Completed, SectionStatus.Completed, SectionStatus.Completed)]
    public void Then_The_Section_States_Are_Calculated_For_Application_Questions(SectionStatus skillsAndStrengthsStatus, SectionStatus interestsStatus, SectionStatus additionalQuestion1Status, SectionStatus additionalQuestion2Status, SectionStatus expectedStatus)
    {
        var source = new ApplicationEntity
        {
            SkillsAndStrengthStatus = (short)skillsAndStrengthsStatus,
            InterestsStatus = (short)interestsStatus,
            AdditionalQuestion1Status = (short)additionalQuestion1Status,
            AdditionalQuestion2Status = (short)additionalQuestion2Status,
        };

        var actual = (Domain.Application.Application)source;

        actual.ApplicationQuestionsSectionStatus.Should().Be(expectedStatus);
    }

    [Test, RecursiveMoqAutoData]
    public void Then_If_All_Sections_Completed_Then_ApplicationAllSectionStatus_Is_Complete(ApplicationEntity source)
    {
        source.JobsStatus = 4;
        source.QualificationsStatus = 3;
        source.WorkExperienceStatus = 5;
        source.TrainingCoursesStatus = 3;
        source.DisabilityConfidenceStatus = 4;
        source.Status = 3;
        source.SkillsAndStrengthStatus = 4;
        source.InterviewAdjustmentsStatus = 3;
        source.AdditionalQuestion2Status = 4;
        source.AdditionalQuestion1Status = 4;
        source.InterestsStatus = 3;

        var actual = (Domain.Application.Application)source;

        actual.ApplicationAllSectionStatus.Should().Be(SectionStatus.Completed);
    }

    [Test, RecursiveMoqAutoData]
    public void Then_If_Application_Status_Returned_As_Expected(ApplicationStatus status)
    {
        var source = new ApplicationEntity
        {
            Status = (short)status
        };

        var actual = (Domain.Application.Application)source;

        actual.Status.Should().Be(status);
    }
}