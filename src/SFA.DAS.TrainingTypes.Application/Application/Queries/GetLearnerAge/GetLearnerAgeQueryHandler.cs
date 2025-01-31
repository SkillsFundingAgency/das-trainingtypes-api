using MediatR;
using SFA.DAS.TrainingTypes.Domain.Factories;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetLearnerAge;
public class GetLearnerAgeQueryHandler(ITrainingTypeFactory trainingTypeFactory) : IRequestHandler<GetLearnerAgeQuery, GetLearnerAgeResult>
{
    public async Task<GetLearnerAgeResult> Handle(GetLearnerAgeQuery request, CancellationToken cancellationToken)
    {
        var trainingType = trainingTypeFactory.Get(request.TrainingTypeShortCode);

        return new GetLearnerAgeResult
        {
            MinimumAge = trainingType.LearnerAge.MinimumAge,
            MaximumAge = trainingType.LearnerAge.MaximumAge
        };
    }
}
