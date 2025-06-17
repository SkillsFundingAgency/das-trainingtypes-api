using SFA.DAS.CourseTypes.Application.Application.Queries.GetLearnerAge;

namespace SFA.DAS.CourseTypes.Api.ApiResponses
{
    public class GetLearnerAgeApiResponse
    {
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }

        public static implicit operator GetLearnerAgeApiResponse(GetLearnerAgeResult source)
        {
            return new GetLearnerAgeApiResponse
            {
                MinimumAge = source.MinimumAge,
                MaximumAge = source.MaximumAge
            };
        }
    }
}
