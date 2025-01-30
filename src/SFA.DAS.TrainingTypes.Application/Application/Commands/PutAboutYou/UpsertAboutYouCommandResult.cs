using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.PutAboutYou;
public class UpsertAboutYouCommandResult
{
    public required AboutYou AboutYou { get; set; }
    public bool IsCreated { get; set; }
}
