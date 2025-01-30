using AutoFixture.NUnit3;
using Azure.Core;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.Qualification;
using SFA.DAS.CandidateAccount.Data.ReferenceData;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertQualification;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Qualifications;

public class WhenHandlingUpdateQualificationCommand
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Qualification_Created(
        UpsertQualificationCommand command,
        QualificationEntity qualificationEntity,
        ApplicationEntity applicationEntity,
        QualificationReferenceEntity qualificationReferenceEntity,
        [Frozen] Mock<IQualificationReferenceRepository> qualificationReferenceRepository,
        [Frozen] Mock<IQualificationRepository> qualificationRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertQualificationCommandHandler handler)
    {
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.TrainingCoursesStatus = (short)SectionStatus.InProgress;

        qualificationReferenceRepository.Setup(x => x.GetById(command.QualificationReferenceId))
            .ReturnsAsync(qualificationReferenceEntity);
        qualificationRepository.Setup(x =>
                x.Upsert(
                    It.Is<Qualification>(c =>
                        c.Id == command.Qualification.Id &&
                        c.QualificationReference.Id == qualificationReferenceEntity.Id),
                    command.CandidateId, command.ApplicationId))
            .ReturnsAsync(new Tuple<QualificationEntity, bool>(qualificationEntity, true));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(applicationEntity);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Qualification.Id.Should().Be(qualificationEntity.Id);
        actual.IsCreated.Should().BeTrue();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_Qualification_Exists_It_Is_Updated(
        UpsertQualificationCommand command,
        QualificationEntity qualificationEntity,
        ApplicationEntity applicationEntity,
        QualificationReferenceEntity qualificationReferenceEntity,
        [Frozen] Mock<IQualificationReferenceRepository> qualificationReferenceRepository,
        [Frozen] Mock<IQualificationRepository> qualificationRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertQualificationCommandHandler handler)
    {
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.TrainingCoursesStatus = (short)SectionStatus.InProgress;

        qualificationReferenceRepository.Setup(x => x.GetById(command.QualificationReferenceId))
            .ReturnsAsync(qualificationReferenceEntity);
        qualificationRepository.Setup(x =>
                x.Upsert(
                    It.Is<Qualification>(c =>
                        c.Id == command.Qualification.Id &&
                        c.QualificationReference.Id == qualificationReferenceEntity.Id), command.CandidateId,
                    command.ApplicationId))
            .ReturnsAsync(new Tuple<QualificationEntity, bool>(qualificationEntity, false));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(applicationEntity);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Qualification.Id.Should().Be(qualificationEntity.Id);
        actual.IsCreated.Should().BeFalse();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_QualificationReference_Doe_Not_Exists_ErrorReturned(
        UpsertQualificationCommand command,
        QualificationEntity qualificationEntity,
        ApplicationEntity applicationEntity,
        [Frozen] Mock<IQualificationRepository> qualificationRepository,
        [Frozen] Mock<IQualificationReferenceRepository> qualificationReferenceRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertQualificationCommandHandler handler)
    {
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.TrainingCoursesStatus = (short)SectionStatus.InProgress;

        qualificationReferenceRepository.Setup(x => x.GetById(command.QualificationReferenceId))
            .ReturnsAsync((QualificationReferenceEntity?)null);
        qualificationRepository.Setup(x => x.Upsert(command.Qualification, command.CandidateId, command.ApplicationId))
            .ReturnsAsync(new Tuple<QualificationEntity, bool>(qualificationEntity, false));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(applicationEntity);

        Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_Candidate_And_Application_Do_Not_Match_ErrorReturned(
        UpsertQualificationCommand command,
        QualificationEntity qualificationEntity,
        ApplicationEntity applicationEntity,
        [Frozen] Mock<IQualificationRepository> qualificationRepository,
        [Frozen] Mock<IQualificationReferenceRepository> qualificationReferenceRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertQualificationCommandHandler handler)
    {
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.TrainingCoursesStatus = (short)SectionStatus.InProgress;

        qualificationRepository.Setup(x => x.Upsert(command.Qualification, command.CandidateId, command.ApplicationId))
            .ReturnsAsync(new Tuple<QualificationEntity, bool>(qualificationEntity, false));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync((ApplicationEntity?)null);

        Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_SectionStatus_Is_NotStarted_Then_It_Is_Updated_To_InProgress(
        UpsertQualificationCommand command,
        QualificationEntity qualificationEntity,
        ApplicationEntity applicationEntity,
        [Frozen] Mock<IQualificationRepository> qualificationRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertQualificationCommandHandler handler)
    {
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.Id = command.ApplicationId;
        applicationEntity.QualificationsStatus = (short)SectionStatus.NotStarted;

        qualificationRepository.Setup(x => x.Upsert(command.Qualification, command.CandidateId, command.ApplicationId))
            .ReturnsAsync(new Tuple<QualificationEntity, bool>(qualificationEntity, false));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(applicationEntity);

        applicationRepository.Setup(x => x.Update(It.IsAny<ApplicationEntity>())).ReturnsAsync(applicationEntity);

        await handler.Handle(command, CancellationToken.None);

        applicationRepository.Verify(x => x.Update(It.Is<ApplicationEntity>(a => a.QualificationsStatus == (short)SectionStatus.InProgress)));
    }
}