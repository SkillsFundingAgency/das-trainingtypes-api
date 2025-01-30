using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Application.Application.Queries.GetAdditionalQuestion;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.ApiResponses;

[TestFixture]
public class WhenMappingGetAdditionalQuestionQueryResponseToModel
{
    [Test, RecursiveMoqAutoData]
    public void Then_The_Fields_Are_Mapped(GetAdditionalQuestionItemQueryResult source)
    {
        var actual = (GetAdditionalQuestionItemApiResponse)source;
        actual.Should().BeEquivalentTo(source);
    }
}