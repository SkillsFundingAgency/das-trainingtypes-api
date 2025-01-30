using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Commands.PatchApplication;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Application;

public class WhenCallingPatchApplication
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned(
        Guid id,
        Guid candidateId,
        PatchApplicationCommandResponse response,
        JsonPatchDocument<PatchApplication> request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.Is<PatchApplicationCommand>(
                c =>
                c.Id.Equals(id)
                && c.CandidateId.Equals(candidateId)
                && c.Patch.Equals(request)
                ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        //Act
        var actual = await controller.PatchApplication(id, candidateId, request) as OkObjectResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        actual.Value.Should().BeEquivalentTo(response.Application);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Null_Returned_From_Mediator_Then_NotFound_Is_Returned(
        Guid id,
        Guid candidateId,
        JsonPatchDocument<PatchApplication> request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<PatchApplicationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PatchApplicationCommandResponse
            {
                Application = null
            });

        //Act
        var actual = await controller.PatchApplication(id, candidateId, request) as StatusCodeResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Error_Then_An_InternalServer_Error_Is_Returned(
        Guid id,
        Guid candidateId,
        JsonPatchDocument<PatchApplication> request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<PatchApplicationCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        //Act
        var actual = await controller.PatchApplication(id, candidateId, request) as StatusCodeResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task Then_If_ValidationError_Then_BadRequest_Response_Returned(
        Guid id,
        Guid candidateId,
        JsonPatchDocument<PatchApplication> request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<PatchApplicationCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException("Error"));

        //Act
        var actual = await controller.PatchApplication(id, candidateId, request) as StatusCodeResult;

        //Assert
        var result = actual as BadRequestResult;
        result?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}