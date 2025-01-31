using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetLearnerAge;
public class GetLearnerAgeQuery : IRequest<GetLearnerAgeResult>
{
    public string TrainingTypeShortCode { get; set; }
}
