using SFA.DAS.TrainingTypes.Domain.Features;

namespace SFA.DAS.TrainingTypes.Domain.TrainingTypes;

public class Apprenticeship : TrainingType
{
    public override string ShortCode => "Apprenticeship";
    public override RecognitionOfPriorLearning RecognitionOfPriorLearning => new RecognitionOfPriorLearningRequired(offTheJobTrainingMinimumHours: 200);
    public override LearnerAge LearnerAge => new LearnerAge(minimumAge: 16, maximumAge: 67);
    public override TrainingDuration TrainingDuration => new TrainingDuration(minimumDurationMonths: 8, maximumDurationMonths: 48);
}