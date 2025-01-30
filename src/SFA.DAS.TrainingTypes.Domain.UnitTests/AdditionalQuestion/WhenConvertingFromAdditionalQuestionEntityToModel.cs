using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Domain.UnitTests.AdditionalQuestion;

[TestFixture]
public class WhenConvertingFromAdditionalQuestionEntityToModel
{
    [Test, RecursiveMoqAutoData]
    public void Then_The_Fields_Are_Mapped(AdditionalQuestionEntity source)
    {
        var actual = (Domain.Application.TrainingType)source;
        actual.Should().BeEquivalentTo(source, options => options.Excluding(c => c.ApplicationEntity));
    }
}