using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetTrainingDuration;
public class GetTrainingDurationQuery : IRequest<GetTrainingDurationResult>
{
    public string TrainingTypeShortCode { get; set; }
}
