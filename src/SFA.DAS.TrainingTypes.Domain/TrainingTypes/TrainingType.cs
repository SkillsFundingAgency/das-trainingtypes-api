using SFA.DAS.TrainingTypes.Domain.Features;

namespace SFA.DAS.TrainingTypes.Domain.TrainingTypes;

public abstract class TrainingType
{
    public abstract string ShortCode { get; }
    public abstract RecognitionOfPriorLearning RecognitionOfPriorLearning { get; }
    public abstract LearnerAge LearnerAge { get; }
    public abstract TrainingDuration TrainingDuration { get; }
}