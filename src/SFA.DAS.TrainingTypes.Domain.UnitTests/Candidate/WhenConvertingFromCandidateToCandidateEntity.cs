using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.TrainingTypes.Domain.Application;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Domain.UnitTests.Candidate;

public class WhenConvertingFromCandidateToCandidateEntity
{
    [Test]
    [InlineAutoData(CandidateStatus.Incomplete)]
    [InlineAutoData(CandidateStatus.Completed)]
    public void Then_The_Fields_Are_Mapped(CandidateStatus candidateStatus, Domain.Candidate.Candidate source)
    {
        source.Status = candidateStatus;
        var actual = (CandidateEntity)source;

        actual.Should().BeEquivalentTo(source, options => options
            .Excluding(c => c.Status)
            .Excluding(c => c.Address)
        );
        actual.Status.Should().Be((short)candidateStatus);
    }
}