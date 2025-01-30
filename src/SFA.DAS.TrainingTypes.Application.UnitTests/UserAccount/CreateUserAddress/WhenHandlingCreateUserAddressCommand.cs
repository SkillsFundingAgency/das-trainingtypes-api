using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.CandidateAccount.Data.Address;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.TrainingTypes.Application.UserAccount.Address;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.UnitTests.UserAccount.CreateUserAddress;
public class WhenHandlingCreateUserAddressCommand
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_Request_Is_Handled_And_Entity_Created(
        CreateUserAddressCommand command,
        AddressEntity addressEntity,
        [Frozen] Mock<IAddressRepository> addressRepository,
        [Greedy] CreateUserAddressCommandHandler handler)
    {
        addressRepository.Setup(x => x.Upsert(It.Is<AddressEntity>(x => x.CandidateId == command.CandidateId))).ReturnsAsync(addressEntity);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Id.Should().Be(addressEntity.Id);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Then_Town_IsNull_Request_Is_Handled_And_Entity_Created(
        CreateUserAddressCommand command,
        AddressEntity addressEntity,
        [Frozen] Mock<IAddressRepository> addressRepository,
        [Greedy] CreateUserAddressCommandHandler handler)
    {
        addressEntity.Town = null;

        addressRepository.Setup(x => x.Upsert(It.Is<AddressEntity>(x => x.CandidateId == command.CandidateId))).ReturnsAsync(addressEntity);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Id.Should().Be(addressEntity.Id);
    }
}
