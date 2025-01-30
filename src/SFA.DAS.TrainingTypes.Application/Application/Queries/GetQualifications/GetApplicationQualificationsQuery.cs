using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetQualifications;

public class GetApplicationQualificationsQuery : IRequest<GetApplicationQualificationsQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}