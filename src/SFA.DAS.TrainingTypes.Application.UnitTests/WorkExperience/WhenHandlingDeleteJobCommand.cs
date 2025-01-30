using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.CandidateAccount.Application.Application.Commands.DeleteTrainingCourse;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.WorkExperience;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteWorkHistory;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.WorkExperience
{
    public class WhenHandlingDeleteJobCommand
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Delete_WorkHistory_From_Repository(
            Guid candidateId,
            Guid applicationId,
            Guid workHistoryId,
            [Frozen] Mock<IApplicationRepository> applicationRepository,
            [Frozen] Mock<IWorkHistoryRepository> mockRepository,
            DeleteWorkHistoryCommandHandler handler)
        {
            // Arrange
            var command = new DeleteWorkHistoryCommand
            {
                CandidateId = candidateId,
                ApplicationId = applicationId,
                JobId = workHistoryId
            };

            var application = new ApplicationEntity { CandidateId = command.CandidateId, JobsStatus = (short)SectionStatus.NotStarted };
            applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
                .ReturnsAsync(application);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockRepository.Verify(
                x => x.Delete(It.Is<Guid>(a => a.Equals(applicationId)),
                              It.Is<Guid>(w => w.Equals(workHistoryId)),
                              It.Is<Guid>(c => c.Equals(candidateId))),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task If_SectionStatus_Is_PreviousAnswer_Then_SectionStatus_Set_To_InProgress(
            DeleteWorkHistoryCommand command,
            [Frozen] Mock<IApplicationRepository> applicationRepository,
            DeleteWorkHistoryCommandHandler handler)
        {
            var application = new ApplicationEntity { CandidateId = command.CandidateId, JobsStatus = (short)SectionStatus.PreviousAnswer };
            applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
                .ReturnsAsync(application);

            applicationRepository.Setup(x => x.Update(It.IsAny<ApplicationEntity>())).ReturnsAsync(application);

            await handler.Handle(command, CancellationToken.None);

            applicationRepository.Verify(x => x.Update(It.Is<ApplicationEntity>(a => a.JobsStatus == (short)SectionStatus.InProgress)));
        }
    }
}
