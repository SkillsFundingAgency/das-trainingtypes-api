using SFA.DAS.CourseTypes.Domain.Features;

namespace SFA.DAS.CourseTypes.Domain.CourseTypes;

public class FoundationApprenticeship : CourseType
{
    public override string ShortCode => "FoundationApprenticeship";
    public override RecognitionOfPriorLearning RecognitionOfPriorLearning => new RecognitionOfPriorLearningNotRequired();
    public override LearnerAge LearnerAge => new LearnerAge(minimumAge: 16, maximumAge: 25);
    public override CourseDuration CourseDuration => new CourseDuration(minimumDurationMonths: 8, maximumDurationMonths: 48);
}