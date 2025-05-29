using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingDuration;
using SFA.DAS.TrainingTypes.Domain.Factories;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Domain.TrainingTypes;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Queries.GetTrainingDuration;

public class WhenIGetTrainingDuration
{
    [Test, MoqAutoData]
    public async Task Then_The_Training_Duration_Is_Returned(
        [Frozen] Mock<ITrainingTypeFactory> trainingTypeFactory,
        GetTrainingDurationQueryHandler handler,
        GetTrainingDurationQuery query)
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
        result.MinimumDurationMonths.Should().Be(trainingType.TrainingDuration.MinimumDurationMonths);
        result.MaximumDurationMonths.Should().Be(trainingType.TrainingDuration.MaximumDurationMonths);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_It_Is_Propagated(
        [Frozen] Mock<ITrainingTypeFactory> trainingTypeFactory,
        GetTrainingDurationQueryHandler handler,
        GetTrainingDurationQuery query,
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