using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Address;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetAddress;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.Address;

public class WhenHandlingGetAddressQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_Request_Is_Handled_And_Entity_Returned(
        GetAddressQuery request,
        Domain.Candidate.Address entity,
        [Frozen] Mock<IAddressRepository> addressRepository,
        GetAddressQueryHandler handler)
    {
        addressRepository.Setup(x => x.Get(request.CandidateId)).ReturnsAsync(entity);

        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Address.Should().NotBeNull();
        actual.Address.Should().BeEquivalentTo(entity);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_Request_is_Handled_And_Empty_Object_Returned(
        GetAddressQuery request,
        [Frozen] Mock<IAddressRepository> addressRepository,
        GetAddressQueryHandler handler)
    {
        addressRepository.Setup(x => x.Get(request.CandidateId)).ReturnsAsync((Domain.Candidate.Address)null!);

        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(new GetAddressQueryResult());
    }
}