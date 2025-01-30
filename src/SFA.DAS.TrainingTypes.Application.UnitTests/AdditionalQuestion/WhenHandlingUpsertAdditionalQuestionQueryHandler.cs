using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.AdditionalQuestion;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertAdditionalQuestion;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.AdditionalQuestion;

[TestFixture]
public class WhenHandlingUpsertAdditionalQuestionQueryHandler
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_AdditionalQuestion_Created(
        UpsertAdditionalQuestionCommand command,
        AdditionalQuestionEntity additionalQuestionEntity,
        [Frozen] Mock<IAdditionalQuestionRepository> additionalQuestionRepository,
        UpsertAdditionalQuestionCommandHandler handler)
    {
        additionalQuestionRepository.Setup(x =>
            x.UpsertAdditionalQuestion(command.AdditionalQuestion, command.CandidateId)).ReturnsAsync(new Tuple<AdditionalQuestionEntity, bool>(additionalQuestionEntity, true));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.AdditionalQuestion.Id.Should().Be(additionalQuestionEntity.Id);
        actual.IsCreated.Should().BeTrue();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_AdditionalQuestion_Exists_It_Is_Updated(
        UpsertAdditionalQuestionCommand command,
        AdditionalQuestionEntity additionalQuestionEntity,
        [Frozen] Mock<IAdditionalQuestionRepository> additionalQuestionRepository,
        UpsertAdditionalQuestionCommandHandler handler)
    {
        additionalQuestionRepository.Setup(x => x.UpsertAdditionalQuestion(command.AdditionalQuestion, command.CandidateId))
            .ReturnsAsync(new Tuple<AdditionalQuestionEntity, bool>(additionalQuestionEntity, false));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.AdditionalQuestion.Id.Should().Be(additionalQuestionEntity.Id);
        actual.IsCreated.Should().BeFalse();
    }
}