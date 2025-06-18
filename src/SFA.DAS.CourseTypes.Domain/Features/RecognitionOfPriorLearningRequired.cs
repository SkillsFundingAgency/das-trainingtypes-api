namespace SFA.DAS.CourseTypes.Domain.Features;

public class RecognitionOfPriorLearningRequired(int offTheJobTrainingMinimumHours) : RecognitionOfPriorLearning
{
    public override bool IsRequired => true;
    public override int? OffTheJobTrainingMinimumHours { get; } = offTheJobTrainingMinimumHours;
}