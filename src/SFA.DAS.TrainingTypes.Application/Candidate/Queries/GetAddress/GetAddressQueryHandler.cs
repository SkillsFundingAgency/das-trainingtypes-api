using MediatR;
using SFA.DAS.CandidateAccount.Data.Address;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetAddress
{
    public record GetAddressQueryHandler(IAddressRepository Repository) : IRequestHandler<GetAddressQuery, GetAddressQueryResult>
    {
        public async Task<GetAddressQueryResult> Handle(GetAddressQuery request, CancellationToken cancellationToken)
        {
            var address = await Repository.Get(request.CandidateId);

            return new GetAddressQueryResult
            {
                Address = address
            };
        }
    }
}
