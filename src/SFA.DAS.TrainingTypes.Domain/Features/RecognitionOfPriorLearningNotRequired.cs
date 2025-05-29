namespace SFA.DAS.TrainingTypes.Domain.Features;

public class RecognitionOfPriorLearningNotRequired : RecognitionOfPriorLearning
{
    public override bool IsRequired => false;
    public override int? OffTheJobTrainingMinimumHours => null;
}