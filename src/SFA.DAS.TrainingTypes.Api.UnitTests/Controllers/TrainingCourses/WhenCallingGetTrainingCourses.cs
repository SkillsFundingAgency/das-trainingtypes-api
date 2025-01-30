using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingCourses;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.TrainingCourses;
public class WhenCallingGetTrainingCourses
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_To_Mediator_And_Ok_Returned(
        Guid applicationId,
        Guid candidateId,
        GetTrainingCoursesQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetTrainingCoursesQuery>(
                c =>
                    c.ApplicationId.Equals(applicationId) &&
                    c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var actual = await controller.GetTrainingCourses(candidateId, applicationId) as OkObjectResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual?.Value.Should().BeEquivalentTo((GetTrainingCoursesApiResponse)response);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_Exception_Returned_From_Mediator_Then_InternalServerError_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetTrainingCoursesQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.GetTrainingCourses(candidateId, applicationId) as StatusCodeResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Response_Is_Null_Then_NotFound_Returned(
        Guid applicationId,
        Guid candidateId,
        GetTrainingCoursesQueryResult response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetTrainingCoursesQuery>(
                c =>
                    c.ApplicationId.Equals(applicationId) &&
                    c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var actual = await controller.GetTrainingCourses(candidateId, applicationId);

        actual.Should().BeOfType<NotFoundResult>();
    }
}
