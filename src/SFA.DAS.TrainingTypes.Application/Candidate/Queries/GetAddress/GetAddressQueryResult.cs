using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Queries.GetAddress;

public record GetAddressQueryResult
{
    public Address? Address { get; set; }
}