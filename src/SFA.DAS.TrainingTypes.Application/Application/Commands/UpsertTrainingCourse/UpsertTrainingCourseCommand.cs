using MediatR;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertTrainingCourse;
public class UpsertTrainingCourseCommand : IRequest<UpsertTrainingCourseCommandResponse>
{
    public required Guid ApplicationId { get; set; }
    public required TrainingCourse TrainingCourse { get; set; }
    public required Guid CandidateId { get; set; }

}
