using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingCourses;
public class GetTrainingCoursesQueryResult
{
    public IEnumerable<TrainingCourse> TrainingCourses { get; init; } = [];

    public static implicit operator GetTrainingCoursesQueryResult(List<TrainingCourseEntity> source)
    {
        return new GetTrainingCoursesQueryResult
        {
            TrainingCourses = source.Select(entity => (TrainingCourse)entity)
        };
    }
}
