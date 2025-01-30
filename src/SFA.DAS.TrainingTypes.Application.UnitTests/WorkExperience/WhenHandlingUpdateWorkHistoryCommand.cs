using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.WorkExperience;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpdateWorkHistory;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.WorkExperience;

[TestFixture]
public class WhenHandlingUpdateWorkHistoryCommand
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_WorkHistory_Created(
        UpsertWorkHistoryCommand command,
        WorkHistoryEntity workHistoryEntity,
        ApplicationEntity applicationEntity,
        [Frozen] Mock<IWorkHistoryRepository> workHistoryRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertWorkHistoryCommandHandler handler)
    {
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.JobsStatus = (short)SectionStatus.InProgress;

        workHistoryRepository.Setup(x =>
            x.UpsertWorkHistory(command.WorkHistory, command.CandidateId)).ReturnsAsync(new Tuple<WorkHistoryEntity, bool>(workHistoryEntity, true));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(applicationEntity);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.WorkHistory.Id.Should().Be(workHistoryEntity.Id);
        actual.IsCreated.Should().BeTrue();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_WorkHistory_Exists_It_Is_Updated(
        UpsertWorkHistoryCommand command,
        WorkHistoryEntity workHistoryEntity,
        ApplicationEntity applicationEntity,
        [Frozen] Mock<IWorkHistoryRepository> workHistoryRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertWorkHistoryCommandHandler handler)
    {
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.JobsStatus = (short)SectionStatus.InProgress;

        workHistoryRepository.Setup(x => x.UpsertWorkHistory(command.WorkHistory, command.CandidateId))
            .ReturnsAsync(new Tuple<WorkHistoryEntity, bool>(workHistoryEntity, false));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(applicationEntity);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.WorkHistory.Id.Should().Be(workHistoryEntity.Id);
        actual.IsCreated.Should().BeFalse();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_SectionStatus_Is_Not_Started_Then_Is_Updated_To_InProgress(
        UpsertWorkHistoryCommand command,
        WorkHistoryEntity workHistoryEntity,
        ApplicationEntity applicationEntity,
        [Frozen] Mock<IWorkHistoryRepository> workHistoryRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertWorkHistoryCommandHandler handler)
    {
        command.WorkHistory.WorkHistoryType = WorkHistoryType.Job;
        workHistoryEntity.WorkHistoryType = (byte)WorkHistoryType.Job;
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.JobsStatus = (short)SectionStatus.NotStarted;

        workHistoryRepository.Setup(x => x.UpsertWorkHistory(command.WorkHistory, command.CandidateId))
            .ReturnsAsync(new Tuple<WorkHistoryEntity, bool>(workHistoryEntity, false));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(applicationEntity);

        applicationRepository.Setup(x => x.Update(It.IsAny<ApplicationEntity>())).ReturnsAsync(applicationEntity);

        await handler.Handle(command, CancellationToken.None);

        applicationRepository.Verify(x => x.Update(It.Is<ApplicationEntity>(a => a.JobsStatus == (short)SectionStatus.InProgress)));
    }
}