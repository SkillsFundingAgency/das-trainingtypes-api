using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Domain.UnitTests.CandidatePreferences;
public class WhenConvertingCandidatePreferenceEntityToModel
{
    [Test, RecursiveMoqAutoData]
    public void Then_The_Fields_Are_Mapped(CandidatePreferencesEntity source)
    {
        var actual = (CandidatePreference)source;

        actual.Id.Should().Be(source.Id);
        actual.PreferenceId.Should().Be(source.PreferenceId);
        actual.CandidateId.Should().Be(source.CandidateId);
        actual.ContactMethod.Should().Be(source.ContactMethod);
        actual.Status.Should().Be(source.Status);
    }
}
