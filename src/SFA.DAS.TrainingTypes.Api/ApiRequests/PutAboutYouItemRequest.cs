using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Api.ApiRequests;

public class PutAboutYouItemRequest
{
    public string? SkillsAndStrengths { get; set; }
    public string? Support { get; set; }
    public GenderIdentity? Sex { get; set; }
    public EthnicGroup? EthnicGroup { get; set; }
    public EthnicSubGroup? EthnicSubGroup { get; set; }
    public string? IsGenderIdentifySameSexAtBirth { get; set; }
    public string? OtherEthnicSubGroupAnswer { get; set; }
}
