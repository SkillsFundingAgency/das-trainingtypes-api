using SFA.DAS.CourseTypes.Domain.Features;

namespace SFA.DAS.CourseTypes.Domain.CourseTypes;

public abstract class CourseType
{
    public abstract string ShortCode { get; }
    public abstract RecognitionOfPriorLearning RecognitionOfPriorLearning { get; }
    public abstract LearnerAge LearnerAge { get; }
    public abstract CourseDuration CourseDuration { get; }
}