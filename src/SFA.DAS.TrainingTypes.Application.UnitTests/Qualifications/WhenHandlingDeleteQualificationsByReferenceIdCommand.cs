using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.CandidateAccount.Application.Application.Commands.DeleteQualification;
using SFA.DAS.CandidateAccount.Data.Application;
using SFA.DAS.CandidateAccount.Data.Qualification;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteQualificationsByReferenceId;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Qualifications;

public class WhenHandlingDeleteQualificationsByReferenceIdCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Qualification_Deleted(
        DeleteQualificationsByReferenceIdCommand command,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        [Frozen] Mock<IQualificationRepository> qualificationRepository,
        DeleteQualificationsByReferenceIdCommandHandler handler)
    {
        var application = new ApplicationEntity { CandidateId = command.CandidateId, QualificationsStatus = (short)SectionStatus.Completed };
        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(application);

        await handler.Handle(command, CancellationToken.None);

        qualificationRepository.Verify(x => x.DeleteCandidateApplicationQualificationsByReferenceId(command.CandidateId, command.ApplicationId, command.QualificationReferenceId), Times.Once);
    }


    [Test, MoqAutoData]
    public async Task If_SectionStatus_Is_PreviousAnswer_Then_SectionStatus_Set_To_InProgress(
        DeleteQualificationsByReferenceIdCommand command,
        [Frozen] Mock<IApplicationRepository> applicationRepository,
        DeleteQualificationsByReferenceIdCommandHandler handler)
    {
        var application = new ApplicationEntity { CandidateId = command.CandidateId, QualificationsStatus = (short)SectionStatus.PreviousAnswer };
        applicationRepository.Setup(x => x.GetById(command.ApplicationId, false))
            .ReturnsAsync(application);

        applicationRepository.Setup(x => x.Update(It.IsAny<ApplicationEntity>())).ReturnsAsync(application);

        await handler.Handle(command, CancellationToken.None);

        applicationRepository.Verify(x => x.Update(It.Is<ApplicationEntity>(a => a.QualificationsStatus == (short)SectionStatus.InProgress)));
    }
}