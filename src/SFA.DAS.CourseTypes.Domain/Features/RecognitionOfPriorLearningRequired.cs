namespace SFA.DAS.CourseTypes.Domain.Features;

public class RecognitionOfPriorLearningRequired : RecognitionOfPriorLearning
{
    public override bool IsRequired => true;
    public override int? OffTheJobTrainingMinimumHours { get; }

    public RecognitionOfPriorLearningRequired(int offTheJobTrainingMinimumHours)
    {
        OffTheJobTrainingMinimumHours = offTheJobTrainingMinimumHours;
    }
}