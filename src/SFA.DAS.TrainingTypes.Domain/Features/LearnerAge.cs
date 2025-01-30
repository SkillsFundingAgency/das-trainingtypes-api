namespace SFA.DAS.TrainingTypes.Domain.Features;

public class LearnerAge
{
    public LearnerAge(int minimumAge, int maximumAge)
    {
        MinimumAge = minimumAge;
        MaximumAge = maximumAge;
    }

    public int MinimumAge { get; }
    public int MaximumAge { get; }
}