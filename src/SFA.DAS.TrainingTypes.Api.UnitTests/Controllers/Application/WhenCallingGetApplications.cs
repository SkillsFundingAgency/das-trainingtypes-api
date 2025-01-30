using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplications;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Application;

public class WhenCallingGetApplications
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned(
        Guid candidateId,
        ApplicationStatus status,
        GetApplicationsQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.Is<GetApplicationsQuery>(
                c =>
                    c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        //Act
        var actual = await controller.GetApplications(candidateId, status) as OkObjectResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        actual.Value.Should().BeEquivalentTo(response);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Returned_From_Mediator_Then_InternalServerErrorr_Is_Returned(
        Guid candidateId,
        ApplicationStatus status,
        GetApplicationsQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.IsAny<GetApplicationsQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        //Act
        var actual = await controller.GetApplications(candidateId, status) as StatusCodeResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}