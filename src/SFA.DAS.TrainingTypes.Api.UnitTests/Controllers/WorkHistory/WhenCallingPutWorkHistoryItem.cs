using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiRequests;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpdateWorkHistory;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.WorkHistory;

public class WhenCallingPutWorkHistory
{
    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_Created_Then_Created_Result_Returned(
        Guid id,
        Guid candidateId,
        Guid applicationId,
        PutWorkHistoryItemRequest upsertWorkHistoryRequest,
        UpsertWorkHistoryCommandResponse upsertWorkHistoryCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] WorkHistoryController controller)
    {
        upsertWorkHistoryCommandResult.IsCreated = true;
        mediator.Setup(x => x.Send(It.Is<UpsertWorkHistoryCommand>(c =>
                c.WorkHistory.Id == id
                && c.WorkHistory.ApplicationId.Equals(applicationId)
            ), CancellationToken.None))
            .ReturnsAsync(upsertWorkHistoryCommandResult);

        var actual = await controller.PutWorkHistoryItem(candidateId, applicationId, id, upsertWorkHistoryRequest);
        var result = actual as CreatedResult;
        var actualResult = result.Value as Domain.Application.WorkHistory;

        actual.Should().BeOfType<CreatedResult>();
        actualResult.Should().BeEquivalentTo(upsertWorkHistoryCommandResult.WorkHistory);
    }

    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_NotCreated_Then_Ok_Result_Returned(
        Guid id,
        Guid candidateId,
        Guid applicationId,
        PutWorkHistoryItemRequest upsertWorkHistoryRequest,
        UpsertWorkHistoryCommandResponse upsertWorkHistoryCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] WorkHistoryController controller)
    {
        upsertWorkHistoryCommandResult.IsCreated = false;
        mediator.Setup(x => x.Send(It.Is<UpsertWorkHistoryCommand>(c =>
                c.WorkHistory.Id == id
                && c.WorkHistory.ApplicationId.Equals(applicationId)
            ), CancellationToken.None))
            .ReturnsAsync(upsertWorkHistoryCommandResult);

        var actual = await controller.PutWorkHistoryItem(candidateId, applicationId, id, upsertWorkHistoryRequest);
        var result = actual as OkObjectResult;
        var actualResult = result.Value as Domain.Application.WorkHistory;

        actual.Should().BeOfType<OkObjectResult>();
        actualResult.Should().BeEquivalentTo(upsertWorkHistoryCommandResult.WorkHistory);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
        PutWorkHistoryItemRequest workHistoryRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] WorkHistoryController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<UpsertWorkHistoryCommand>(),
            CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.PutWorkHistoryItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), workHistoryRequest);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}