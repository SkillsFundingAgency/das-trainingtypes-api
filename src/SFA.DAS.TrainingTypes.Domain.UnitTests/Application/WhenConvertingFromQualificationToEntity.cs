using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Domain.UnitTests.Application;

public class WhenConvertingFromQualificationToEntity
{
    [Test, RecursiveMoqAutoData]
    public void Then_The_Fields_Are_Mapped(Qualification source)
    {
        var actual = (QualificationEntity)source;

        actual.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
    }
}