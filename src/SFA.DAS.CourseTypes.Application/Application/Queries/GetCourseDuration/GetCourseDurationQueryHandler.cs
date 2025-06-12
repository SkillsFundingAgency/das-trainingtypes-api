using MediatR;
using SFA.DAS.CourseTypes.Domain.Factories;

namespace SFA.DAS.CourseTypes.Application.Application.Queries.GetCourseDuration;
public class GetCourseDurationQueryHandler(ICourseTypeFactory courseTypeFactory) : IRequestHandler<GetCourseDurationQuery, GetCourseDurationResult>
{
    public async Task<GetCourseDurationResult> Handle(GetCourseDurationQuery request, CancellationToken cancellationToken)
    {
        var courseType = courseTypeFactory.Get(request.CourseTypeShortCode);

        return new GetCourseDurationResult
        {
            MinimumDurationMonths = courseType.CourseDuration.MinimumDurationMonths,
            MaximumDurationMonths = courseType.CourseDuration.MaximumDurationMonths
        };
    }
}
