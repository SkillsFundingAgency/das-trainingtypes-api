using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingCourses;
public class GetTrainingCoursesQuery : IRequest<GetTrainingCoursesQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}
