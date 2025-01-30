using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Domain.Application;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Domain.UnitTests.Candidate;

public class WhenConvertingFromCandidateEntityToModel
{
    [Test, RecursiveMoqAutoData]
    public void Then_The_Fields_Are_Mapped(CandidateEntity source, CandidateStatus status)
    {
        source.Status = (short)status;
        var actual = (Domain.Candidate.Candidate)source;

        actual.Should().BeEquivalentTo(source, options => options
            .Excluding(c => c.Applications)
            .Excluding(c => c.Status)
            .Excluding(c => c.Address)
            .Excluding(c => c.AboutYou)
            .Excluding(c => c.CandidatePreferences)
        );
        actual.Status.Should().Be(status);
    }
    [Test, RecursiveMoqAutoData]
    public void Then_The_Fields_Are_Mapped_With_No_Applications_And_No_Address(CandidateEntity source, CandidateStatus status)
    {
        source.Status = (short)status;
        source.Applications = null;
        source.Address = null;

        var actual = (Domain.Candidate.Candidate)source;

        actual.Should()
            .BeEquivalentTo(source,
                options => options.Excluding(c => c.Applications)
                    .Excluding(c => c.Status)
                    .Excluding(c => c.CandidatePreferences)
                    .Excluding(c => c.AboutYou));
        actual.Status.Should().Be(status);
    }
}