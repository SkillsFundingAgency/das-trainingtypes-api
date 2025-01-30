using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationQualificationsByType;

public class GetApplicationQualificationsByTypeQuery : IRequest<GetApplicationQualificationsByTypeQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid QualificationReferenceId { get; set; }
}