using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiRequests;
using SFA.DAS.TrainingTypes.Api.Controllers;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertTrainingCourse;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.Controllers.TrainingCourses;
public class WhenCallingPutTrainingCourseItem
{
    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_Created_Then_Created_Result_Returned(
        Guid id,
        Guid candidateId,
        Guid applicationId,
        PutTrainingCourseItemRequest upsertTrainingCourseRequest,
        UpsertTrainingCourseCommandResponse upsertTrainingCourseCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        upsertTrainingCourseCommandResult.IsCreated = true;
        mediator.Setup(x => x.Send(It.Is<UpsertTrainingCourseCommand>(c =>
                c.TrainingCourse.Id == id
                && c.TrainingCourse.Title.Equals(upsertTrainingCourseRequest.CourseName)
                && c.TrainingCourse.ToYear.Equals((int)upsertTrainingCourseRequest.YearAchieved)
            ), CancellationToken.None))
            .ReturnsAsync(upsertTrainingCourseCommandResult);

        var actual = await controller.PutTrainingCourseItem(candidateId, applicationId, id, upsertTrainingCourseRequest);
        var result = actual as CreatedResult;
        var actualResult = result.Value as Domain.Application.TrainingCourse;

        actual.Should().BeOfType<CreatedResult>();
        actualResult.Should().BeEquivalentTo(upsertTrainingCourseCommandResult.TrainingCourse);
    }

    [Test, MoqAutoData]
    public async Task Then_If_MediatorCall_Returns_NotCreated_Then_Ok_Result_Returned(
        Guid id,
        Guid candidateId,
        Guid applicationId,
        PutTrainingCourseItemRequest upsertTrainingCourseRequest,
        UpsertTrainingCourseCommandResponse upsertTrainingCourseCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        upsertTrainingCourseCommandResult.IsCreated = false;
        mediator.Setup(x => x.Send(It.Is<UpsertTrainingCourseCommand>(c =>
                c.TrainingCourse.Id == id
                && c.TrainingCourse.Title.Equals(upsertTrainingCourseRequest.CourseName)
                && c.TrainingCourse.ToYear.Equals((int)upsertTrainingCourseRequest.YearAchieved)
            ), CancellationToken.None))
            .ReturnsAsync(upsertTrainingCourseCommandResult);

        var actual = await controller.PutTrainingCourseItem(candidateId, applicationId, id, upsertTrainingCourseRequest);
        var result = actual as OkObjectResult;
        var actualResult = result.Value as Domain.Application.TrainingCourse;

        actual.Should().BeOfType<OkObjectResult>();
        actualResult.Should().BeEquivalentTo(upsertTrainingCourseCommandResult.TrainingCourse);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Error_Then_InternalServerError_Response_Returned(
        PutTrainingCourseItemRequest trainingCourseRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<UpsertTrainingCourseCommand>(),
            CancellationToken.None)).ThrowsAsync(new Exception("Error"));

        var actual = await controller.PutTrainingCourseItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), trainingCourseRequest);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
