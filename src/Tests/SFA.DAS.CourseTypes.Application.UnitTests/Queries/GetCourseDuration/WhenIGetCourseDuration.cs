using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CourseTypes.Application.Application.Queries.GetCourseDuration;
using SFA.DAS.CourseTypes.Domain.CourseTypes;
using SFA.DAS.CourseTypes.Domain.Factories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.CourseTypes.Application.UnitTests.Queries.GetCourseDuration;

public class WhenIGetCourseTypeDuration
{
    [Test, MoqAutoData]
    public async Task Then_The_CourseType_Duration_Is_Returned(
        [Frozen] Mock<ICourseTypeFactory> courseTypeFactory,
        GetCourseDurationQueryHandler handler,
        GetCourseDurationQuery query)
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
        result.MinimumDurationMonths.Should().Be(courseType.CourseDuration.MinimumDurationMonths);
        result.MaximumDurationMonths.Should().Be(courseType.CourseDuration.MaximumDurationMonths);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_It_Is_Propagated(
        [Frozen] Mock<ICourseTypeFactory> courseTypeFactory,
        GetCourseDurationQueryHandler handler,
        GetCourseDurationQuery query,
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