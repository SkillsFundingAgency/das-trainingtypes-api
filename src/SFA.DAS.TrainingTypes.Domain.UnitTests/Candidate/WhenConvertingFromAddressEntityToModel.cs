using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Domain.UnitTests.Candidate;

public class WhenConvertingFromAddressEntityToModel
{
    [Test, RecursiveMoqAutoData]
    public void Then_The_Fields_Are_Mapped(AddressEntity source)
    {
        var actual = (Address)source;

        actual.Should().BeEquivalentTo(source, options => options.Excluding(c => c.Candidate));
    }
}