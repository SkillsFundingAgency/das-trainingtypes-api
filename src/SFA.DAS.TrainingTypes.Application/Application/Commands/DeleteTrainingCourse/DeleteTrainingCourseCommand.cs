using MediatR;


namespace SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteTrainingCourse
{
    public class DeleteTrainingCourseCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
    }
}
