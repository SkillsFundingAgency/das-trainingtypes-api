using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetLearnerAge;
using SFA.DAS.TrainingTypes.Domain.Factories;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Domain.Features;
using SFA.DAS.TrainingTypes.Domain.TrainingTypes;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Queries.GetLearnerAge;

public class WhenIGetLearnerAge
{
    [Test, MoqAutoData]
    public async Task Then_The_Learner_Age_Is_Returned(
        [Frozen] Mock<ITrainingTypeFactory> trainingTypeFactory,
        GetLearnerAgeQueryHandler handler,
        GetLearnerAgeQuery query)
    {
        // Arrange
        var trainingType = new Apprenticeship();
        trainingTypeFactory
            .Setup(x => x.Get(trainingType.ShortCode))
            .Returns(trainingType);

        query.TrainingTypeShortCode = trainingType.ShortCode;

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.MinimumAge.Should().Be(trainingType.LearnerAge.MinimumAge);
        result.MaximumAge.Should().Be(trainingType.LearnerAge.MaximumAge);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_It_Is_Propagated(
        [Frozen] Mock<ITrainingTypeFactory> trainingTypeFactory,
        GetLearnerAgeQueryHandler handler,
        GetLearnerAgeQuery query,
        string trainingTypeShortCode)
    {
        // Arrange
        trainingTypeFactory
            .Setup(x => x.Get(trainingTypeShortCode))
            .Throws(new Exception("Test exception"));

        query.TrainingTypeShortCode = trainingTypeShortCode;

        // Act & Assert
        await handler.Invoking(x => x.Handle(query, CancellationToken.None))
            .Should().ThrowAsync<Exception>()
            .WithMessage("Test exception");
    }
} 