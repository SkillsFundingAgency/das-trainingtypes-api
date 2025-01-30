using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.AboutYou;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Application.Commands.PutAboutYou;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.AboutYou;
public class WhenHandlingUpsertAboutYouCommand
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Candidate_Created(
        UpsertAboutYouCommand command,
        Domain.Candidate.AboutYou aboutYouEntity,
        [Frozen] Mock<IAboutYouRespository> aboutYouRepository,
        UpsertAboutYouCommandHandler handler)
    {
        aboutYouRepository.Setup(x =>
            x.Upsert(command.AboutYou, command.CandidateId)).ReturnsAsync(new Tuple<Domain.Candidate.AboutYou, bool>(aboutYouEntity, true));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.AboutYou.Id.Should().Be(aboutYouEntity.Id);
        actual.IsCreated.Should().BeTrue();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_Candidate_And_Application_Exist_It_Is_Updated(
        UpsertAboutYouCommand command,
        Domain.Candidate.AboutYou aboutYouEntity,
        [Frozen] Mock<IAboutYouRespository> aboutYouRepository,
        UpsertAboutYouCommandHandler handler)
    {
        aboutYouRepository.Setup(x =>
                    x.Upsert(command.AboutYou, command.CandidateId)).ReturnsAsync(new Tuple<Domain.Candidate.AboutYou, bool>(aboutYouEntity, false));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.AboutYou.Id.Should().Be(aboutYouEntity.Id);
        actual.IsCreated.Should().BeFalse();
    }
}
