using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.CandidateAccount.Application.ReferenceData.Queries;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.ReferenceData.Queries.GetAvailableQualifications;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.ReferenceData;

public class WhenCallingGetQualifications
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetAvailableQualificationsQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ReferenceDataController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetAvailableQualificationsQuery>(), CancellationToken.None))
            .ReturnsAsync(result);

        var actual = await controller.GetQualifications() as OkObjectResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        actual.Value.Should().BeEquivalentTo(new { result.QualificationReferences });
    }

    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Exception_Internal_Server_Error_Response_Returned(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ReferenceDataController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetAvailableQualificationsQuery>(), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetQualifications() as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}