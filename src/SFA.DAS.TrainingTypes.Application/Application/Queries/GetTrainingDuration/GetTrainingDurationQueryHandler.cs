using MediatR;
using SFA.DAS.TrainingTypes.Domain.Factories;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingDuration;
public class GetTrainingDurationQueryHandler(ITrainingTypeFactory trainingTypeFactory) : IRequestHandler<GetTrainingDurationQuery, GetTrainingDurationResult>
{
    public async Task<GetTrainingDurationResult> Handle(GetTrainingDurationQuery request, CancellationToken cancellationToken)
    {
        var trainingType = trainingTypeFactory.Get(request.TrainingTypeShortCode);

        return new GetTrainingDurationResult
        {
            MinimumDurationMonths = trainingType.TrainingDuration.MinimumDurationMonths,
            MaximumDurationMonths = trainingType.TrainingDuration.MaximumDurationMonths
        };
    }
}
