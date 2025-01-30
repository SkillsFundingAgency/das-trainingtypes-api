using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplication;

public class GetApplicationQuery : IRequest<GetApplicationQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public bool IncludeDetail { get; set; }
}