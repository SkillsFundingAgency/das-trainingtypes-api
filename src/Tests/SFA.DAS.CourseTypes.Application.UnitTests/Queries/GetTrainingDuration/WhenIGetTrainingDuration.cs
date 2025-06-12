using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CourseTypes.Application.Application.Queries.GetTrainingDuration;
using SFA.DAS.CourseTypes.Domain.Factories;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.CourseTypes.Domain.CourseTypes;

namespace SFA.DAS.CourseTypes.Application.UnitTests.Queries.GetTrainingDuration;

public class WhenIGetCourseTypeDuration
{
    [Test, MoqAutoData]
    public async Task Then_The_CourseType_Duration_Is_Returned(
        [Frozen] Mock<ICourseTypeFactory> courseTypeFactory,
        GetTrainingDurationQueryHandler handler,
        GetTrainingDurationQuery query)
    {
        // Arrange
        var courseType = new Apprenticeship();
        courseTypeFactory
            .Setup(x => x.Get(courseType.ShortCode))
            .Returns(courseType);

        query.CourseTypeShortCode = courseType.ShortCode;

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.MinimumDurationMonths.Should().Be(courseType.TrainingDuration.MinimumDurationMonths);
        result.MaximumDurationMonths.Should().Be(courseType.TrainingDuration.MaximumDurationMonths);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_It_Is_Propagated(
        [Frozen] Mock<ICourseTypeFactory> courseTypeFactory,
        GetTrainingDurationQueryHandler handler,
        GetTrainingDurationQuery query,
        string courseTypeShortCode)
    {
        // Arrange
        courseTypeFactory
            .Setup(x => x.Get(courseTypeShortCode))
            .Throws(new Exception("Test exception"));

        query.CourseTypeShortCode = courseTypeShortCode;

        // Act & Assert
        await handler.Invoking(x => x.Handle(query, CancellationToken.None))
            .Should().ThrowAsync<Exception>()
            .WithMessage("Test exception");
    }
} 