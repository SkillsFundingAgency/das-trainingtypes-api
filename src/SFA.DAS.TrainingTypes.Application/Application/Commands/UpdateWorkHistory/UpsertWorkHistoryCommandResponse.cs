using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpdateWorkHistory;
public class UpsertWorkHistoryCommandResponse
{
    public required WorkHistory WorkHistory { get; set; }
    public bool IsCreated { get; set; }
}
