using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationByVacancyReference;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Application;

public class WhenCallingGetByReferenceApplication
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned(
        string vacancyReference,
        Guid candidateId,
        GetApplicationByVacancyReferenceQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.Is<GetApplicationByVacancyReferenceQuery>(
                c =>
                    c.VacancyReference.Equals(vacancyReference)
                    && c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        //Act
        var actual = await controller.GetApplicationByVacancyReference(candidateId, vacancyReference) as OkObjectResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual!.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Null_Returned_From_Mediator_Then_NotFound_Is_Returned(
        string vacancyReference,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.Is<GetApplicationByVacancyReferenceQuery>(
                c =>
                    c.VacancyReference.Equals(vacancyReference)
                    && c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetApplicationByVacancyReferenceQueryResult
            {
                Application = null
            });

        //Act
        var actual = await controller.GetApplicationByVacancyReference(candidateId, vacancyReference) as StatusCodeResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task Then_If_ValidationError_Returned_From_Mediator_Then_BadRequest_Is_Returned(
        string vacancyReference,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<GetApplicationByVacancyReferenceQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException());

        //Act
        var actual = await controller.GetApplicationByVacancyReference(candidateId, vacancyReference) as BadRequestObjectResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Returned_From_Mediator_Then_InternalServerError_Is_Returned(
        string vacancyReference,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<GetApplicationByVacancyReferenceQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        //Act
        var actual = await controller.GetApplicationByVacancyReference(candidateId, vacancyReference) as StatusCodeResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}