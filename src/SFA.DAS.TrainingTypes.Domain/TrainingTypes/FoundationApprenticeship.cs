﻿using SFA.DAS.TrainingTypes.Domain.Features;

namespace SFA.DAS.TrainingTypes.Domain.TrainingTypes;

public class FoundationApprenticeship : TrainingType
{
    public override string ShortCode => "Foundation";
    public override RecognitionOfPriorLearning RecognitionOfPriorLearning => new RecognitionOfPriorLearningNotRequired();
    public override LearnerAge LearnerAge => new LearnerAge(minimumAge: 16, maximumAge: 25);
    public override TrainingDuration TrainingDuration => new TrainingDuration(minimumDurationMonths: 8, maximumDurationMonths: 48);
}