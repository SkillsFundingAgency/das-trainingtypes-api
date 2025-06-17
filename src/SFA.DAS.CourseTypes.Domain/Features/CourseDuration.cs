namespace SFA.DAS.CourseTypes.Domain.Features;

public class CourseDuration(int minimumDurationMonths, int maximumDurationMonths)
{
    public int MinimumDurationMonths { get; } = minimumDurationMonths;
    public int MaximumDurationMonths { get; } = maximumDurationMonths;
}