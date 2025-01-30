using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Api.ApiResponses;
using SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetAddress;

namespace SFA.DAS.TrainingTypes.Api.UnitTests.ApiResponses;

public class WhenMappingGetAddressQueryResponseToModel
{
    [Test, RecursiveMoqAutoData]
    public void Then_The_Fields_Are_Mapped(GetAddressQueryResult source)
    {
        var actual = (GetAddressApiResponse)source;
        actual.Should().BeEquivalentTo(source.Address);
    }
}