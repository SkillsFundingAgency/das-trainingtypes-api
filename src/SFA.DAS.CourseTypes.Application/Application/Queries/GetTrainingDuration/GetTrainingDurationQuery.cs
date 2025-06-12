using MediatR;

namespace SFA.DAS.CourseTypes.Application.Application.Queries.GetTrainingDuration;
public class GetTrainingDurationQuery : IRequest<GetTrainingDurationResult>
{
    public string CourseTypeShortCode { get; set; }
}
