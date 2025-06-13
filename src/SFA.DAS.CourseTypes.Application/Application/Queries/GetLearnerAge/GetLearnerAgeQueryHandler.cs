using MediatR;
using SFA.DAS.CourseTypes.Domain.Factories;

namespace SFA.DAS.CourseTypes.Application.Application.Queries.GetLearnerAge;
public class GetLearnerAgeQueryHandler(ICourseTypeFactory courseTypeFactory) : IRequestHandler<GetLearnerAgeQuery, GetLearnerAgeResult>
{
    public async Task<GetLearnerAgeResult> Handle(GetLearnerAgeQuery request, CancellationToken cancellationToken)
    {
        var courseType = courseTypeFactory.Get(request.CourseTypeShortCode);

        return new GetLearnerAgeResult
        {
            MinimumAge = courseType.LearnerAge.MinimumAge,
            MaximumAge = courseType.LearnerAge.MaximumAge
        };
    }
}
