using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertAdditionalQuestion;

public record UpsertAdditionalQuestionCommandResponse
{
    public required TrainingType AdditionalQuestion { get; init; }
    public bool IsCreated { get; set; }
}