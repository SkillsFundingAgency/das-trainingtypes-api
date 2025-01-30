using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertTrainingCourse;
public class UpsertTrainingCourseCommandResponse
{
    public required TrainingCourse TrainingCourse { get; set; }
    public bool IsCreated { get; set; }
}
