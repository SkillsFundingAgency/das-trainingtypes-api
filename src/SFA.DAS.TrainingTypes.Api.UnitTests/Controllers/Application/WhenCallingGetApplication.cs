using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplication;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Application;

public class WhenCallingGetApplication
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned(
        Guid id,
        Guid candidateId,
        Domain.Application.Application application,
        GetApplicationQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        response.Application = application;
        mediator.Setup(x => x.Send(It.Is<GetApplicationQuery>(
                c =>
                    c.ApplicationId.Equals(id)
                    && c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        //Act
        var actual = await controller.GetApplication(id, candidateId) as OkObjectResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        actual.Value.Should().BeEquivalentTo((Domain.Application.Application)response.Application, options => options.Excluding(prop => prop!.AdditionalQuestions));
    }

    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned_With_Detail(
        Guid id,
        Guid candidateId,
        Domain.Application.ApplicationDetail application,
        GetApplicationQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        response.Application = application;
        mediator.Setup(x => x.Send(It.Is<GetApplicationQuery>(
                c =>
                    c.ApplicationId.Equals(id)
                    && c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        //Act
        var actual = await controller.GetApplication(id, candidateId, true) as OkObjectResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        actual.Value.Should().BeEquivalentTo((Domain.Application.ApplicationDetail)response.Application, options =>
            options
                .Excluding(prop => prop!.AdditionalQuestions)
                .Excluding(prop => prop!.TrainingCourses)
            );
    }


    [Test, MoqAutoData]
    public async Task Then_If_Null_Returned_From_Mediator_Then_NotFound_Is_Returned(
        Guid id,
        Guid candidateId,
        GetApplicationQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<GetApplicationQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetApplicationQueryResult
            {
                Application = null
            });

        //Act
        var actual = await controller.GetApplication(id, candidateId) as StatusCodeResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task Then_If_ValidationError_Returned_From_Mediator_Then_BadRequest_Is_Returned(
        Guid id,
        Guid candidateId,
        GetApplicationQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<GetApplicationQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException());

        //Act
        var actual = await controller.GetApplication(id, candidateId) as BadRequestObjectResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Returned_From_Mediator_Then_InternalServerErrorr_Is_Returned(
        Guid id,
        Guid candidateId,
        GetApplicationQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<GetApplicationQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        //Act
        var actual = await controller.GetApplication(id, candidateId) as StatusCodeResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}