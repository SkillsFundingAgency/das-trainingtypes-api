using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CourseTypes.Application.Application.Queries.GetTrainingDuration;
using MediatR;
using SFA.DAS.CourseTypes.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.CourseTypes.Api.UnitTests.Controllers.FeaturesControllerTests;

public class WhenIGetCourseTypeDuration
{
    [Test, MoqAutoData]
    public async Task Then_The_CourseType_Duration_Is_Returned(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] FeaturesController controller,
        string courseTypeShortCode)
    {
        // Arrange
        var expectedResult = new GetTrainingDurationResult
        {
            MinimumDurationMonths = 8,
            MaximumDurationMonths = 48
        };
        mediator
            .Setup(x => x.Send(
                It.Is<GetTrainingDurationQuery>(q => q.CourseTypeShortCode == courseTypeShortCode), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await controller.GetTrainingDuration(courseTypeShortCode);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedResult);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Is_Returned(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] FeaturesController controller,
        string courseTypeShortCode)
    {
        // Arrange
        mediator.Setup(x => x.Send(
                It.Is<GetTrainingDurationQuery>(q => q.CourseTypeShortCode == courseTypeShortCode), 
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await controller.GetTrainingDuration(courseTypeShortCode);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var statusCodeResult = result as StatusCodeResult;
        statusCodeResult?.StatusCode.Should().Be(500);
    }
} 