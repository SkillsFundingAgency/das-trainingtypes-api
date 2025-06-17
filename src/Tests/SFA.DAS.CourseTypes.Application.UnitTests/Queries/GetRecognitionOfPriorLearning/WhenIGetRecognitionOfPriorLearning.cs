using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CourseTypes.Application.Application.Queries.GetRecognitionOfPriorLearning;
using SFA.DAS.CourseTypes.Domain.CourseTypes;
using SFA.DAS.CourseTypes.Domain.Factories;
using SFA.DAS.CourseTypes.Domain.Features;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.CourseTypes.Application.UnitTests.Queries.GetRecognitionOfPriorLearning;

public class WhenIGetRecognitionOfPriorLearning
{
    [Test, MoqAutoData]
    public async Task Then_The_Recognition_Of_Prior_Learning_Is_Returned(
        [Frozen] Mock<ICourseTypeFactory> courseTypeFactory,
        GetRecognitionOfPriorLearningQueryHandler handler,
        GetRecognitionOfPriorLearningQuery query)
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
        result.IsRequired.Should().Be(courseType.RecognitionOfPriorLearning.IsRequired);
        result.OffTheJobTrainingMinimumHours.Should().Be(courseType.RecognitionOfPriorLearning.OffTheJobTrainingMinimumHours);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_It_Is_Propagated(
        [Frozen] Mock<ICourseTypeFactory> courseTypeFactory,
        GetRecognitionOfPriorLearningQueryHandler handler,
        GetRecognitionOfPriorLearningQuery query,
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