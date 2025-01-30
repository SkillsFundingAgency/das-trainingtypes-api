using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.CandidateAccount.Application.Application.Commands.DeleteQualification;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteQualificationsByReferenceId;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Qualifications;

public class WhenDeletingQualificationsByQualificationReferenceId
{
    [Test, MoqAutoData]
    public async Task Then_The_Qualifications_Are_Deleted_By_ReferenceId_And_No_Content_Returned(
        Guid qualificationReferenceId,
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<DeleteQualificationsByReferenceIdCommand>(c =>
                    c.CandidateId == candidateId && c.ApplicationId == applicationId && c.QualificationReferenceId == qualificationReferenceId), CancellationToken.None))
            .ReturnsAsync(new Unit());

        var actual = await controller.DeleteByReferenceId(candidateId, applicationId, qualificationReferenceId) as NoContentResult;

        Assert.That(actual, Is.Not.Null);
        actual!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Deleting_Qualifications_Then_InternalServerError_Response_Returned(
        Guid qualificationReferenceId,
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<DeleteQualificationsByReferenceIdCommand>(c =>
                    c.CandidateId == candidateId && c.ApplicationId == applicationId && c.QualificationReferenceId == qualificationReferenceId), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.DeleteByReferenceId(candidateId, applicationId, qualificationReferenceId) as StatusCodeResult;

        Assert.That(actual, Is.Not.Null);
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}