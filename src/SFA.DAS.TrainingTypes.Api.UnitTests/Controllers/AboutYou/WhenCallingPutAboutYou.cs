using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiRequests;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Commands.PutAboutYou;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.AboutYou;
public class WhenCallingPutAboutYou
{
    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_Created_Then_Created_Result_Returned(
        Guid id,
        Guid candidateId,
        PutAboutYouItemRequest upsertAboutYouRequest,
        UpsertAboutYouCommandResult upsertAboutYouCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AboutYouController controller)
    {
        upsertAboutYouCommandResult.IsCreated = true;
        mediator.Setup(x => x.Send(It.Is<UpsertAboutYouCommand>(c =>
            c.AboutYou.Sex!.Equals(upsertAboutYouRequest.Sex)
            && c.AboutYou.EthnicGroup!.Equals(upsertAboutYouRequest.EthnicGroup)
            && c.AboutYou.EthnicSubGroup!.Equals(upsertAboutYouRequest.EthnicSubGroup)
            && c.AboutYou.IsGenderIdentifySameSexAtBirth!.Equals(upsertAboutYouRequest.IsGenderIdentifySameSexAtBirth)
            && c.AboutYou.OtherEthnicSubGroupAnswer!.Equals(upsertAboutYouRequest.OtherEthnicSubGroupAnswer)
            ), CancellationToken.None))
            .ReturnsAsync(upsertAboutYouCommandResult);

        var actual = await controller.PutAboutYouItem(candidateId, upsertAboutYouRequest);
        var result = actual as CreatedResult;
        var actualResult = result.Value as Domain.Candidate.AboutYou;

        actual.Should().BeOfType<CreatedResult>();
        actualResult.Should().BeEquivalentTo(upsertAboutYouCommandResult.AboutYou);
    }

    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_NotCreated_Then_Ok_Result_Returned(
        Guid id,
        Guid candidateId,
        PutAboutYouItemRequest upsertAboutYouRequest,
        UpsertAboutYouCommandResult upsertAboutYouCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AboutYouController controller)
    {
        upsertAboutYouCommandResult.IsCreated = false;
        mediator.Setup(x => x.Send(It.Is<UpsertAboutYouCommand>(c =>
                c.AboutYou.Sex!.Equals(upsertAboutYouRequest.Sex)
                && c.AboutYou.EthnicGroup!.Equals(upsertAboutYouRequest.EthnicGroup)
                && c.AboutYou.EthnicSubGroup!.Equals(upsertAboutYouRequest.EthnicSubGroup)
                && c.AboutYou.IsGenderIdentifySameSexAtBirth!.Equals(upsertAboutYouRequest.IsGenderIdentifySameSexAtBirth)
                && c.AboutYou.OtherEthnicSubGroupAnswer!.Equals(upsertAboutYouRequest.OtherEthnicSubGroupAnswer)
            ), CancellationToken.None))
            .ReturnsAsync(upsertAboutYouCommandResult);

        var actual = await controller.PutAboutYouItem(candidateId, upsertAboutYouRequest);
        var result = actual as OkObjectResult;
        var actualResult = result.Value as Domain.Candidate.AboutYou;

        actual.Should().BeOfType<OkObjectResult>();
        actualResult.Should().BeEquivalentTo(upsertAboutYouCommandResult.AboutYou);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
        PutAboutYouItemRequest request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] AboutYouController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<UpsertAboutYouCommand>(),
            CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.PutAboutYouItem(Guid.NewGuid(), request);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
