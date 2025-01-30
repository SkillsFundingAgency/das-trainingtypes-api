using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetAdditionalQuestion;

public class GetAdditionalQuestionItemQuery : IRequest<GetAdditionalQuestionItemQueryResult>
{
    public Guid ApplicationId { get; init; }
    public Guid CandidateId { get; set; }
    public Guid Id { get; set; }
}