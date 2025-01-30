using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingCourseItem;
using System.Net;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.TrainingCourses;
public class WhenCallingGet
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned(
        Guid applicationId,
        Guid candidateId,
        Guid id,
        GetTrainingCourseItemQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetTrainingCourseItemQuery>(
                c =>
                    c.ApplicationId.Equals(applicationId) &&
                    c.CandidateId.Equals(candidateId) &&
                    c.Id.Equals(id)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var actual = await controller.Get(candidateId, applicationId, id) as OkObjectResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual?.Value.Should().BeEquivalentTo((GetTrainingCourseItemApiResponse)response);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_NotFound_Returned_From_Mediator_Then_NotFound_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetTrainingCourseItemQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetTrainingCourseItemQueryResult)null!);

        var actual = await controller.Get(candidateId, applicationId, id) as StatusCodeResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Returned_From_Mediator_Then_InternalServerError_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        Guid id,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetTrainingCourseItemQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.Get(candidateId, applicationId, id) as StatusCodeResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
