namespace SFA.DAS.CourseTypes.Domain.Features;

public abstract class RecognitionOfPriorLearning
{
    public abstract bool IsRequired { get; }
    public abstract int? OffTheJobTrainingMinimumHours { get; }
}