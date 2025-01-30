using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingCourseItem;
public class GetTrainingCourseItemQuery : IRequest<GetTrainingCourseItemQueryResult>
{
    public Guid ApplicationId { get; init; }
    public Guid CandidateId { get; set; }
    public Guid Id { get; set; }
}
