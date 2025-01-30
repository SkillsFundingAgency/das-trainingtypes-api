using MediatR;
using SFA.DAS.CandidateAccount.Data.Address;

namespace SFA.DAS.TrainingTypes.Application.UserAccount.Address;
public class CreateUserAddressCommandHandler : IRequestHandler<CreateUserAddressCommand, CreateUserAddressCommandResult>
{
    private readonly IAddressRepository _addressRepository;

    public CreateUserAddressCommandHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<CreateUserAddressCommandResult> Handle(CreateUserAddressCommand request, CancellationToken cancellationToken)
    {
        var result = await _addressRepository.Upsert(new Domain.Candidate.AddressEntity()
        {
            Id = Guid.NewGuid(),
            Uprn = request.Uprn,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            Town = request.AddressLine3 ?? string.Empty,
            County = request.AddressLine4,
            Postcode = request.Postcode,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            CandidateId = request.CandidateId
        });

        return new CreateUserAddressCommandResult()
        {
            Id = result.Id,
            CandidateId = result.CandidateId
        };
    }
}
