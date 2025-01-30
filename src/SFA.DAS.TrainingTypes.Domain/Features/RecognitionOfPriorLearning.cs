namespace SFA.DAS.TrainingTypes.Domain.Features;

public abstract class RecognitionOfPriorLearning
{
    public abstract bool IsRequired { get; }
    public abstract int? OffTheJobTrainingMinimumHours { get; }
}