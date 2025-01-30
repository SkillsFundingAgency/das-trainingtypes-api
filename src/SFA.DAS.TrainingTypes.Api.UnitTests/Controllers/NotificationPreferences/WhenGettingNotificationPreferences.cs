using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.CandidatePreferences.Queries.GetCandidatePreferences;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.NotificationPreferences;
public class WhenGettingNotificationPreferences
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned(
        Guid candidateId,
        GetCandidatePreferencesQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] NotificationPreferenceController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidatePreferencesQuery>(
                c =>
                    c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var actual = await controller.Get(candidateId) as OkObjectResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Returned_From_Mediator_Then_InternalServerError_Is_Returned(
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] NotificationPreferenceController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetCandidatePreferencesQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.Get(candidateId) as StatusCodeResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
