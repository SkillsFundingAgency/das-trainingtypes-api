using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Domain.UnitTests.Candidate;
public class WhenConvertingFromAboutYouEntityToModel
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(AboutYou source)
    {
        var actual = (AboutYouEntity)source;

        actual.Should().BeEquivalentTo(source);
    }
}
