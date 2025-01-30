using AutoFixture.NUnit3;
using Azure;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetAddress;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Address;

public class WhenCallingGetAddress
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned(
        Guid candidateId,
        GetAddressQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AddressController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetAddressQuery>(
                c =>
                    c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var actual = await controller.Get(candidateId) as OkObjectResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual?.Value.Should().BeEquivalentTo((GetAddressApiResponse)response);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_Null_Returned_From_Mediator_Then_NotFound_Is_Returned(
        Guid candidateId,
        GetAddressQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AddressController controller)
    {
        response.Address = null;
        mediator.Setup(x => x.Send(It.IsAny<GetAddressQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var actual = await controller.Get(candidateId) as StatusCodeResult;

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
        [Greedy] AddressController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetAddressQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.Get(candidateId) as StatusCodeResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}