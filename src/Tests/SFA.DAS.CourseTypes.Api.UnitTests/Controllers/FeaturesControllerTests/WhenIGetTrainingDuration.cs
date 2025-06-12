using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CourseTypes.Api.Controllers;
using SFA.DAS.CourseTypes.Application.Application.Queries.GetCourseDuration;
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
        var expectedResult = new GetCourseDurationResult
        {
            MinimumDurationMonths = 8,
            MaximumDurationMonths = 48
        };
        mediator
            .Setup(x => x.Send(
                It.Is<GetCourseDurationQuery>(q => q.CourseTypeShortCode == courseTypeShortCode), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await controller.GetCourseDuration(courseTypeShortCode);

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
                It.Is<GetCourseDurationQuery>(q => q.CourseTypeShortCode == courseTypeShortCode), 
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await controller.GetCourseDuration(courseTypeShortCode);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var statusCodeResult = result as StatusCodeResult;
        statusCodeResult?.StatusCode.Should().Be(500);
    }
} 