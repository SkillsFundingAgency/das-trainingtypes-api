using MediatR;

namespace SFA.DAS.CourseTypes.Application.Application.Queries.GetRecognitionOfPriorLearning;
public class GetRecognitionOfPriorLearningQuery : IRequest<GetRecognitionOfPriorLearningResult>
{
    public string CourseTypeShortCode { get; set; }
}
