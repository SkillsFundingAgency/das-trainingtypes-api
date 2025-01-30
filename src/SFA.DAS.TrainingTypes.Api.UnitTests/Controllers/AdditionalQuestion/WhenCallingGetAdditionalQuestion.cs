using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetAdditionalQuestion;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.AdditionalQuestion;

[TestFixture]
public class WhenCallingGetAdditionalQuestion
{
    [Test, MoqAutoData]
    public async Task Then_The_Response_Is_Returned_As_Expected(
        Guid applicationId,
        Guid candidateId,
        Guid id,
        GetAdditionalQuestionItemQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AdditionalQuestionController controller)
    {
        //Arrange
        mediator.Setup(x => x.Send(It.Is<GetAdditionalQuestionItemQuery>(
                c =>
                    c.ApplicationId.Equals(applicationId) &&
                    c.CandidateId.Equals(candidateId) &&
                    c.Id.Equals(id)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        //Act
        var actual = await controller.Get(candidateId, applicationId, id) as OkObjectResult;

        //Assert
        using var scope = new AssertionScope();
        actual.Should().NotBeNull();
        actual?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        actual?.Value.Should().BeEquivalentTo((GetAdditionalQuestionItemApiResponse)response);
    }

    [Test, MoqAutoData]
    public async Task And_Response_Is_Null_Then_Return_NotFound(
        Guid applicationId,
        Guid candidateId,
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AdditionalQuestionController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetAdditionalQuestionItemQuery>(
                c =>
                    c.ApplicationId.Equals(applicationId) &&
                    c.CandidateId.Equals(candidateId) &&
                    c.Id.Equals(id)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var actual = await controller.Get(candidateId, applicationId, id);

        actual.Should().BeOfType<NotFoundResult>();
    }
}