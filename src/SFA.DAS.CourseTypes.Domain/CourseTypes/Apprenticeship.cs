﻿using SFA.DAS.CourseTypes.Domain.Features;

namespace SFA.DAS.CourseTypes.Domain.CourseTypes;

public class Apprenticeship : CourseType
{
    public override string ShortCode => "Apprenticeship";
    public override RecognitionOfPriorLearning RecognitionOfPriorLearning => new RecognitionOfPriorLearningRequired(offTheJobTrainingMinimumHours: 200);
    public override LearnerAge LearnerAge => new LearnerAge(minimumAge: 16, maximumAge: 67);
    public override CourseDuration CourseDuration => new CourseDuration(minimumDurationMonths: 8, maximumDurationMonths: 48);
}