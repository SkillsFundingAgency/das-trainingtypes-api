using MediatR;

namespace SFA.DAS.CourseTypes.Application.Application.Queries.GetLearnerAge;
public class GetLearnerAgeQuery : IRequest<GetLearnerAgeResult>
{
    public string CourseTypeShortCode { get; set; }
}
