using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.CandidateAccount.Application.Application.Queries.GetQualification;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteQualification;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Qualifications;

public class WhenDeletingQualificationById
{
    [Test, MoqAutoData]
    public async Task Then_The_Qualification_Is_Deleted_By_Id_And_No_Content_Returned(
        Guid id,
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<DeleteQualificationCommand>(c =>
                    c.CandidateId == candidateId && c.ApplicationId == applicationId && c.Id == id), CancellationToken.None))
            .ReturnsAsync(new Unit());

        var actual = await controller.Delete(candidateId, applicationId, id) as NoContentResult;

        Assert.That(actual, Is.Not.Null);
        actual!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Deleting_Qualification_Then_InternalServerError_Response_Returned(
        Guid id,
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationController controller)
    {
        mediator.Setup(x =>
                x.Send(It.Is<DeleteQualificationCommand>(c =>
                    c.CandidateId == candidateId && c.ApplicationId == applicationId && c.Id == id), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.Delete(candidateId, applicationId, id) as StatusCodeResult;

        Assert.That(actual, Is.Not.Null);
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}