using SFA.DAS.CourseTypes.Application.Application.Queries.GetCourseDuration;

namespace SFA.DAS.CourseTypes.Api.ApiResponses
{
    public class GetCourseDurationApiResponse
    {
        public int MinimumDurationMonths { get; set; }
        public int MaximumDurationMonths { get; set; }

        public static implicit operator GetCourseDurationApiResponse(GetCourseDurationResult source)
        {
            return new GetCourseDurationApiResponse
            {
                MinimumDurationMonths = source.MinimumDurationMonths,
                MaximumDurationMonths = source.MaximumDurationMonths
            };
        }
    }
}
