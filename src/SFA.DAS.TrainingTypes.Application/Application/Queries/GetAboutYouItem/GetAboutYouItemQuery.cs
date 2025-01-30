using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetAboutYouItem;
public class GetAboutYouItemQuery : IRequest<GetAboutYouItemQueryResult>
{
    public Guid CandidateId { get; set; }
}
