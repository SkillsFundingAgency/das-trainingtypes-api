using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetRecognitionOfPriorLearning;
using SFA.DAS.TrainingTypes.Domain.Factories;
using SFA.DAS.TrainingTypes.Domain.Features;
using SFA.DAS.TrainingTypes.Domain.TrainingTypes;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Queries.GetRecognitionOfPriorLearning;

public class WhenIGetRecognitionOfPriorLearning
{
    [Test, MoqAutoData]
    public async Task Then_The_Recognition_Of_Prior_Learning_Is_Returned(
        [Frozen] Mock<ITrainingTypeFactory> trainingTypeFactory,
        GetRecognitionOfPriorLearningQueryHandler handler,
        GetRecognitionOfPriorLearningQuery query)
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
        result.IsRequired.Should().Be(trainingType.RecognitionOfPriorLearning.IsRequired);
        result.OffTheJobTrainingMinimumHours.Should().Be(trainingType.RecognitionOfPriorLearning.OffTheJobTrainingMinimumHours);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_It_Is_Propagated(
        [Frozen] Mock<ITrainingTypeFactory> trainingTypeFactory,
        GetRecognitionOfPriorLearningQueryHandler handler,
        GetRecognitionOfPriorLearningQuery query,
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