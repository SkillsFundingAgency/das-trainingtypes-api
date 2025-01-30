using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Candidate;
using SFA.DAS.CandidateAccount.Data.CandidatePreferences;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Candidate.Commands.UpsertCandidate;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Candidate;

public class WhenHandlingUpsertCandidateCommand
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Candidate_Created(
        UpsertCandidateCommand command,
        CandidateEntity candidateEntity,
        [Frozen] Mock<ICandidatePreferencesRepository> candidatePreferencesRepository,
        [Frozen] Mock<ICandidateRepository> candidateRepository,
        UpsertCandidateCommandHandler handler)
    {
        candidateRepository.Setup(x =>
            x.UpsertCandidate(command.Candidate)).ReturnsAsync(new Tuple<CandidateEntity, bool>(candidateEntity, true));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Candidate.Id.Should().Be(candidateEntity.Id);
        actual.IsCreated.Should().BeTrue();
        candidatePreferencesRepository.Verify(x => x.Create(candidateEntity.Id), Times.Once);

    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_If_The_Candidate_And_Application_Exist_It_Is_Updated(
        UpsertCandidateCommand command,
        CandidateEntity candidateEntity,
        [Frozen] Mock<ICandidatePreferencesRepository> candidatePreferencesRepository,
        [Frozen] Mock<ICandidateRepository> candidateRepository,
        UpsertCandidateCommandHandler handler)
    {
        candidateRepository.Setup(x => x.UpsertCandidate(command.Candidate))
            .ReturnsAsync(new Tuple<CandidateEntity, bool>(candidateEntity, false));

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Candidate.Id.Should().Be(candidateEntity.Id);
        actual.IsCreated.Should().BeFalse();
        candidatePreferencesRepository.Verify(x => x.Create(candidateEntity.Id), Times.Never);
    }
}