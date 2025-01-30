using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetQualification;

public class GetQualificationQuery : IRequest<GetQualificationQueryResult>
{
    public Guid Id { get; set; }
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}