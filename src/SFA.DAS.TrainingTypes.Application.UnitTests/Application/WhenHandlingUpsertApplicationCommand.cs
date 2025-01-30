using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.AdditionalQuestion;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.SavedVacancy;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertApplication;
using SFA.DAS.TrainingTypes.Domain.Application;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Application;

public class WhenHandlingUpsertApplicationCommand
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_Candidate_Retrieved_And_Application_Created(
        UpsertApplicationCommand command,
        ApplicationEntity applicationEntity,
        AdditionalQuestionEntity additionalQuestionEntity,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        [Frozen] Mock<IAdditionalQuestionRepository> additionalQuestionRepository,
        UpsertApplicationCommandHandler handler)
    {
        applicationRepository.Setup(x =>
            x.Upsert(It.Is<ApplicationEntity>(c =>
                c.VacancyReference.Equals(command.VacancyReference)
                && c.CandidateId.Equals(command.CandidateId)
                && c.DisabilityStatus.Equals(command.DisabilityStatus)
                && c.Status.Equals((short)command.Status)
                && c.JobsStatus.Equals((short)command.IsApplicationQuestionsComplete)
                && c.DisabilityConfidenceStatus.Equals((short)command.IsDisabilityConfidenceComplete)
                && c.QualificationsStatus.Equals((short)command.IsEducationHistoryComplete)
                && c.TrainingCoursesStatus.Equals((short)command.IsWorkHistoryComplete)
                && c.WorkExperienceStatus.Equals((short)command.IsInterviewAdjustmentsComplete)
                && c.AdditionalQuestion1Status.Equals((short)command.IsAdditionalQuestion1Complete)
                && c.AdditionalQuestion2Status.Equals((short)command.IsAdditionalQuestion2Complete)
                ))).ReturnsAsync(new Tuple<ApplicationEntity, bool>(applicationEntity, true));

        additionalQuestionRepository.Setup(x =>
            x.UpsertAdditionalQuestion(It.Is<Domain.Application.TrainingType>(c =>
                c.Id.Equals(Guid.NewGuid())
                && c.ApplicationId.Equals(applicationEntity.Id)
                && c.CandidateId.Equals(command.CandidateId)
                && c.QuestionText.Equals(command.AdditionalQuestions.FirstOrDefault())
                && c.Answer.Equals(string.Empty)
                ), command.CandidateId))
            .ReturnsAsync(new Tuple<AdditionalQuestionEntity, bool>(additionalQuestionEntity, true));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Application.Id.Should().Be(applicationEntity.Id);
        actual.IsCreated.Should().BeTrue();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_Candidate_And_Application_Exist_It_Is_Updated(
        UpsertApplicationCommand command,
        ApplicationEntity applicationEntity,
        AdditionalQuestionEntity additionalQuestionEntity,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        [Frozen] Mock<IAdditionalQuestionRepository> additionalQuestionRepository,
        UpsertApplicationCommandHandler handler)
    {
        applicationRepository.Setup(x =>
            x.Upsert(It.Is<ApplicationEntity>(c =>
                c.VacancyReference.Equals(command.VacancyReference)
                && c.CandidateId.Equals(command.CandidateId)
                && c.DisabilityStatus.Equals(command.DisabilityStatus)
                && c.Status.Equals((short)command.Status)
                && c.JobsStatus.Equals((short)command.IsApplicationQuestionsComplete)
                && c.DisabilityConfidenceStatus.Equals((short)command.IsDisabilityConfidenceComplete)
                && c.QualificationsStatus.Equals((short)command.IsEducationHistoryComplete)
                && c.TrainingCoursesStatus.Equals((short)command.IsWorkHistoryComplete)
                && c.WorkExperienceStatus.Equals((short)command.IsInterviewAdjustmentsComplete)
                && c.AdditionalQuestion1Status.Equals((short)command.IsAdditionalQuestion1Complete)
                && c.AdditionalQuestion2Status.Equals((short)command.IsAdditionalQuestion2Complete)
            ))).ReturnsAsync(new Tuple<ApplicationEntity, bool>(applicationEntity, false));

        additionalQuestionRepository.Setup(x =>
                x.UpsertAdditionalQuestion(It.Is<Domain.Application.TrainingType>(c =>
                    c.Id.Equals(Guid.NewGuid())
                    && c.ApplicationId.Equals(applicationEntity.Id)
                    && c.CandidateId.Equals(command.CandidateId)
                    && c.QuestionText.Equals(command.AdditionalQuestions.FirstOrDefault())
                    && c.Answer.Equals(string.Empty)
                ), command.CandidateId))
            .ReturnsAsync(new Tuple<AdditionalQuestionEntity, bool>(additionalQuestionEntity, true));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Application.Id.Should().Be(applicationEntity.Id);
        actual.IsCreated.Should().BeFalse();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_Application_Is_Cloned_If_Previous_Application_Exists(
        UpsertApplicationCommand command,
        ApplicationEntity previousApplication,
        ApplicationEntity cloneResult,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertApplicationCommandHandler handler)
    {
        previousApplication.MigrationDate = null;
        var previousApplications = new List<ApplicationEntity> { previousApplication };

        applicationRepository.Setup(x => x.Exists(command.CandidateId, command.VacancyReference))
            .ReturnsAsync(false);

        applicationRepository.Setup(x => x.GetByCandidateId(command.CandidateId, null))
            .ReturnsAsync(previousApplications);

        applicationRepository.Setup(x =>
            x.Clone(previousApplication.Id, command.VacancyReference, command.IsDisabilityConfidenceComplete == SectionStatus.NotStarted, command.IsAdditionalQuestion1Complete, command.IsAdditionalQuestion2Complete))
            .ReturnsAsync(cloneResult);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Application.Id.Should().Be(cloneResult.Id);
        actual.IsCreated.Should().BeTrue();
    }

    [Test]
    [RecursiveMoqInlineAutoData(ApplicationStatus.Withdrawn)]
    [RecursiveMoqInlineAutoData(ApplicationStatus.Draft)]
    [RecursiveMoqInlineAutoData(ApplicationStatus.Expired)]
    public async Task Then_The_Request_Is_Handled_Application_Is_Not_Cloned_From_A_Previous_Application_If_Withdrawn_Or_Expired_Or_WithDrawn(
        ApplicationStatus status,
        UpsertApplicationCommand command,
        ApplicationEntity previousApplication,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertApplicationCommandHandler handler)
    {
        previousApplication.Status = (short)status;
        var previousApplications = new List<ApplicationEntity> { previousApplication };

        applicationRepository.Setup(x => x.Exists(command.CandidateId, command.VacancyReference))
            .ReturnsAsync(false);

        applicationRepository.Setup(x => x.GetByCandidateId(command.CandidateId, null))
            .ReturnsAsync(previousApplications);

        await handler.Handle(command, CancellationToken.None);

        applicationRepository.Verify(x =>
                x.Clone(It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<SectionStatus?>(),
                    It.IsAny<SectionStatus?>()),
            Times.Never());
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_Application_Is_Not_Cloned_From_A_Previous_Application_If_Migrated(
        UpsertApplicationCommand command,
        ApplicationEntity previousApplication,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertApplicationCommandHandler handler)
    {
        previousApplication.Status = (short)ApplicationStatus.Submitted;
        previousApplication.MigrationDate = DateTime.Today;
        var previousApplications = new List<ApplicationEntity> { previousApplication };

        applicationRepository.Setup(x => x.Exists(command.CandidateId, command.VacancyReference))
            .ReturnsAsync(false);

        applicationRepository.Setup(x => x.GetByCandidateId(command.CandidateId, null))
            .ReturnsAsync(previousApplications);

        await handler.Handle(command, CancellationToken.None);

        applicationRepository.Verify(x =>
                x.Clone(It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<SectionStatus?>(),
                    It.IsAny<SectionStatus?>()),
            Times.Never());
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_Saved_Vacancy_Is_Deleted(
        UpsertApplicationCommand command,
        SavedVacancy savedVacancy,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        [Frozen] Mock<ISavedVacancyRepository> savedVacancyRepository,
        UpsertApplicationCommandHandler handler)
    {
        savedVacancyRepository.Setup(x => x.Get(command.CandidateId, command.VacancyReference))
            .ReturnsAsync(savedVacancy);

        await handler.Handle(command, CancellationToken.None);

        savedVacancyRepository.Verify(x => x.Delete(It.Is<SavedVacancy>(v => v == savedVacancy)), Times.Once);

    }
}