using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetAboutYouItem;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.AboutYou;
public class WhenCallingGetAboutYouItem
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned(
        Guid candidateId,
        GetAboutYouItemQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AboutYouController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetAboutYouItemQuery>(
                c => c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var actual = await controller.Get(candidateId) as OkObjectResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual?.Value.Should().BeEquivalentTo((GetAboutYouItemApiResponse)response);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_NotFound_Returned_When_Result_Is_Null(
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AboutYouController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetAboutYouItemQuery>(
                c =>
                    c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAboutYouItemQueryResult());

        var actual = await controller.Get(candidateId) as NotFoundResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Returned_From_Mediator_Then_InternalServerError_Is_Returned(
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AboutYouController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetAboutYouItemQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.Get(candidateId) as StatusCodeResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
