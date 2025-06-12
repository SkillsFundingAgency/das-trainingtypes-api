using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CourseTypes.Api.ApiResponses;
using SFA.DAS.CourseTypes.Api.Controllers;
using SFA.DAS.CourseTypes.Application.Application.Queries.GetLearnerAge;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.CourseTypes.Api.UnitTests.Controllers.FeaturesControllerTests;

public class WhenIGetLearnerAge
{
    [Test, MoqAutoData]
    public async Task Then_The_Learner_Age_Is_Returned(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] FeaturesController controller,
        string courseTypeShortCode)
    {
        // Arrange
        var expectedResponse = new GetLearnerAgeResult
        {
            MinimumAge = 16,
            MaximumAge = 67
        };

        mediator.Setup(x => x.Send(
            It.Is<GetLearnerAgeQuery>(q => q.CourseTypeShortCode == courseTypeShortCode),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var response = await controller.GetLearnerAge(courseTypeShortCode);

        // Assert
        response.Should().NotBeNull();
        var result = response.Should().BeOfType<OkObjectResult>().Subject;
        var model = result.Value.Should().BeOfType<GetLearnerAgeApiResponse>().Subject;
        model.MinimumAge.Should().Be(expectedResponse.MinimumAge);
        model.MaximumAge.Should().Be(expectedResponse.MaximumAge);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Is_Returned(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] FeaturesController controller,
        string courseTypeShortCode)
    {
        // Arrange
        mediator.Setup(x => x.Send(
            It.Is<GetLearnerAgeQuery>(q => q.CourseTypeShortCode == courseTypeShortCode),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new System.Exception("Test exception"));

        // Act
        var response = await controller.GetLearnerAge(courseTypeShortCode);

        // Assert
        response.Should().NotBeNull();
        response.Should().BeOfType<StatusCodeResult>()
            .Which.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
} 