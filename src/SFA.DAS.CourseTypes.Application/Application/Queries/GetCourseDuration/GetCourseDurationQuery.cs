using MediatR;

namespace SFA.DAS.CourseTypes.Application.Application.Queries.GetCourseDuration;
public class GetCourseDurationQuery : IRequest<GetCourseDurationResult>
{
    public string CourseTypeShortCode { get; set; }
}
