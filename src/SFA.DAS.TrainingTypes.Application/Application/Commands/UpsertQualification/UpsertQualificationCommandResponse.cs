using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertQualification;

public class UpsertQualificationCommandResponse
{
    public required Qualification Qualification { get; set; }
    public bool IsCreated { get; set; }
}