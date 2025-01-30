using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiRequests;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.UserAccount.Address;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.Address;
public class WhenPostingAddress
{
    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_Result_Then_Ok_Result_Returned(
        Guid candidateId,
        AddressRequest addressRequest,
        CreateUserAddressCommandResult commandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AddressController controller)
    {
        mediator.Setup(x => x.Send(It.Is<CreateUserAddressCommand>(c =>
                c.Email.Equals(addressRequest.Email)
            ), CancellationToken.None))
            .ReturnsAsync(commandResult);

        var actual = await controller.Put(candidateId, addressRequest);

        actual.Should().BeOfType<OkObjectResult>();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
        Guid candidateId,
        AddressRequest addressRequest,
        CreateUserAddressCommandResult commandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AddressController controller)
    {
        mediator.Setup(x => x.Send(It.Is<CreateUserAddressCommand>(c =>
                c.Email.Equals(addressRequest.Email)
            ), CancellationToken.None))
            .ThrowsAsync(new Exception("Error"));

        var actual = await controller.Put(candidateId, addressRequest);

        var result = actual as StatusCodeResult;
        result?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
