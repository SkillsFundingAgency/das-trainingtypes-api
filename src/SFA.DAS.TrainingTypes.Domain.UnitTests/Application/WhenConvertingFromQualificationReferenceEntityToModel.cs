using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Domain.UnitTests.Application;

public class WhenConvertingFromQualificationReferenceEntityToModel
{
    [Test, RecursiveMoqAutoData]
    public void Then_The_Fields_Are_Mapped(QualificationReferenceEntity source)
    {
        var actual = (QualificationReference)source;

        actual.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
    }
}