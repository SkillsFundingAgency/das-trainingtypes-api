using MediatR;
using SFA.DAS.TrainingTypes.Domain.Factories;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetRecognitionOfPriorLearning;
public class GetRecognitionOfPriorLearningQueryHandler(ITrainingTypeFactory trainingTypeFactory) : IRequestHandler<GetRecognitionOfPriorLearningQuery, GetRecognitionOfPriorLearningResult>
{
    public async Task<GetRecognitionOfPriorLearningResult> Handle(GetRecognitionOfPriorLearningQuery request, CancellationToken cancellationToken)
    {
        var trainingType = trainingTypeFactory.Get(request.TrainingTypeShortCode);

        return new GetRecognitionOfPriorLearningResult
        {
            IsRequired = trainingType.RecognitionOfPriorLearning.IsRequired,
            OffTheJobTrainingMinimumHours = trainingType.RecognitionOfPriorLearning.OffTheJobTrainingMinimumHours
        };
    }
}
