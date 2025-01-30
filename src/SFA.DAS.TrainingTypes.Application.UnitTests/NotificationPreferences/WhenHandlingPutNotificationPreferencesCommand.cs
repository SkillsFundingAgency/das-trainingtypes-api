using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.CandidatePreferences;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.CandidatePreferences.Commands.PutCandidatePreferences;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.NotificationPreferences;
public class WhenHandlingPutNotificationPreferencesCommand
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Upsert_Is_Done(
        PutCandidatePreferencesCommand command,
        List<Tuple<bool, CandidatePreferencesEntity>> upsertResponse,
        [Frozen] Mock<ICandidatePreferencesRepository> repository,
        PutCandidatePreferencesCommandHandler handler)
    {
        repository.Setup(x =>
            x.Upsert(It.IsAny<List<CandidatePreference>>())).ReturnsAsync(upsertResponse);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.CandidatePreferences.Count.Should().Be(command.CandidatePreferences.Count);
    }
}
