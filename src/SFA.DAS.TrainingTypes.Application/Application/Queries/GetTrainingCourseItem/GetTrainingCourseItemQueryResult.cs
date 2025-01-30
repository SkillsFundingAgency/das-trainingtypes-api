using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingCourseItem;
public class GetTrainingCourseItemQueryResult
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Title { get; set; }
    public int ToYear { get; set; }

    public static implicit operator GetTrainingCourseItemQueryResult(TrainingCourseEntity source)
    {
        return new GetTrainingCourseItemQueryResult
        {
            Id = source.Id,
            ApplicationId = source.ApplicationId,
            Title = source.Title,
            ToYear = source.ToYear
        };
    }
}
