using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetRecognitionOfPriorLearning;
public class GetRecognitionOfPriorLearningQuery : IRequest<GetRecognitionOfPriorLearningResult>
{
    public string TrainingTypeShortCode { get; set; }
}
