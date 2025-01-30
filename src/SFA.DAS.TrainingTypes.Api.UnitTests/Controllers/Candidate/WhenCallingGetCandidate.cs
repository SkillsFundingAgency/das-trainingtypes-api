using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetCandidate;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Candidate;

public class WhenCallingGetCandidate
{
    [Test, MoqAutoData]
    public async Task Then_Mediator_Query_Is_Called_And_Candidate_Returned(
        string id,
        GetCandidateQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] CandidateController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.Is<GetCandidateQuery>(c => c.Id.Equals(id)), CancellationToken.None))
            .ReturnsAsync(queryResult);

        //Act
        var actual = await controller.GetCandidate(id) as OkObjectResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        actual.Value.Should().BeEquivalentTo(queryResult.Candidate);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Null_From_Mediator_Response_Not_Found_Returned(
        string id,
        GetCandidateQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] CandidateController controller)
    {
        //Arrange
        queryResult.Candidate = null;
        mediator.Setup(x => x.Send(It.Is<GetCandidateQuery>(c => c.Id.Equals(id)), CancellationToken.None))
            .ReturnsAsync(queryResult);

        //Act
        var actual = await controller.GetCandidate(id) as NotFoundResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Internal_Server_Error_Returned(
        string id,
        GetCandidateQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] CandidateController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.Is<GetCandidateQuery>(c => c.Id.Equals(id)), CancellationToken.None))
            .ThrowsAsync(new Exception());

        //Act
        var actual = await controller.GetCandidate(id) as StatusCodeResult;

        //Assert
        Assert.That(actual, Is.Not.Null);
        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}