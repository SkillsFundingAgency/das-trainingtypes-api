using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiRequests;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.CandidatePreferences.Commands.PutCandidatePreferences;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.NotificationPreferences;
public class WhenPuttingNotificationPreferences
{
    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_Result_Then_Ok_Result_Returned(
        Guid candidateId,
        PutCandidatePreferencesRequest addressRequest,
        PutCandidatePreferencesCommandResult commandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] NotificationPreferenceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<PutCandidatePreferencesCommand>(c =>
                c.CandidatePreferences.Count.Equals(addressRequest.CandidatePreferences.Count)
            ), CancellationToken.None))
            .ReturnsAsync(commandResult);

        var actual = await controller.Put(candidateId, addressRequest);

        actual.Should().BeOfType<OkObjectResult>();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
        Guid candidateId,
        PutCandidatePreferencesRequest addressRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] NotificationPreferenceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<PutCandidatePreferencesCommand>(c =>
                c.CandidatePreferences.Count.Equals(addressRequest.CandidatePreferences.Count)
            ), CancellationToken.None))
            .ThrowsAsync(new Exception("Error"));

        var actual = await controller.Put(candidateId, addressRequest);

        var result = actual as StatusCodeResult;
        result?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
