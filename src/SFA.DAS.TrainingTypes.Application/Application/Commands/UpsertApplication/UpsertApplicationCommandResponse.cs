namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertApplication;

public class UpsertApplicationCommandResponse
{
    public Domain.Application.Application Application { get; set; }
    public bool IsCreated { get; set; }
}