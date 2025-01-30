namespace SFA.DAS.TrainingTypes.Domain.Features;

public class TrainingDuration
{
    public TrainingDuration(int minimumDurationMonths, int maximumDurationMonths)
    {
        MinimumDurationMonths = minimumDurationMonths;
        MaximumDurationMonths = maximumDurationMonths;
    }

    public int MinimumDurationMonths { get; }
    public int MaximumDurationMonths { get; }
}