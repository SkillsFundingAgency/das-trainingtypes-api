using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.TrainingCourse;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertTrainingCourse;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.TrainingCourses;
public class WhenHandlingUpdateTrainingCourseCommand
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_TrainingCourse_Created(
        UpsertTrainingCourseCommand command,
        TrainingCourseEntity trainingCourseEntity,
        ApplicationEntity applicationEntity,
        [Frozen] Mock<ITrainingCourseRepository> trainingCourseRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertTrainingCourseCommandHandler handler)
    {
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.TrainingCoursesStatus = (short)SectionStatus.InProgress;

        trainingCourseRepository.Setup(x =>
            x.UpsertTrainingCourse(command.TrainingCourse, command.CandidateId)).ReturnsAsync(new Tuple<TrainingCourseEntity, bool>(trainingCourseEntity, true));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(applicationEntity);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.TrainingCourse.Id.Should().Be(trainingCourseEntity.Id);
        actual.IsCreated.Should().BeTrue();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_TrainingCourse_Exists_It_Is_Updated(
        UpsertTrainingCourseCommand command,
        TrainingCourseEntity trainingCourseEntity,
        ApplicationEntity applicationEntity,
        [Frozen] Mock<ITrainingCourseRepository> trainingCourseRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertTrainingCourseCommandHandler handler)
    {
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.TrainingCoursesStatus = (short)SectionStatus.InProgress;

        trainingCourseRepository.Setup(x => x.UpsertTrainingCourse(command.TrainingCourse, command.CandidateId))
            .ReturnsAsync(new Tuple<TrainingCourseEntity, bool>(trainingCourseEntity, false));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(applicationEntity);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.TrainingCourse.Id.Should().Be(trainingCourseEntity.Id);
        actual.IsCreated.Should().BeFalse();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_SectionStatus_Is_NotStarted_Then_It_Is_Updated_To_InProgress(
        UpsertTrainingCourseCommand command,
        TrainingCourseEntity trainingCourseEntity,
        ApplicationEntity applicationEntity,
        [Frozen] Mock<ITrainingCourseRepository> trainingCourseRepository,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        UpsertTrainingCourseCommandHandler handler)
    {
        applicationEntity.CandidateId = command.CandidateId;
        applicationEntity.TrainingCoursesStatus = (short)SectionStatus.NotStarted;

        trainingCourseRepository.Setup(x => x.UpsertTrainingCourse(command.TrainingCourse, command.CandidateId))
            .ReturnsAsync(new Tuple<TrainingCourseEntity, bool>(trainingCourseEntity, false));

        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(applicationEntity);

        applicationRepository.Setup(x => x.Update(It.IsAny<ApplicationEntity>())).ReturnsAsync(applicationEntity);

        await handler.Handle(command, CancellationToken.None);

        applicationRepository.Verify(x => x.Update(It.Is<ApplicationEntity>(a => a.TrainingCoursesStatus == (short)SectionStatus.InProgress)));
    }
}
