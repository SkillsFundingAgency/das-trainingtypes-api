using SFA.DAS.CourseTypes.Application.Application.Queries.GetTrainingDuration;

namespace SFA.DAS.CourseTypes.Api.ApiResponses
{
    public class GetTrainingDurationApiResponse
    {
        public int MinimumDurationMonths { get; set; }
        public int MaximumDurationMonths { get; set; }

        public static implicit operator GetTrainingDurationApiResponse(GetTrainingDurationResult source)
        {
            return new GetTrainingDurationApiResponse
            {
                MinimumDurationMonths = source.MinimumDurationMonths,
                MaximumDurationMonths = source.MaximumDurationMonths
            };
        }
    }
}
