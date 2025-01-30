using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetVolunteeringOrWorkExperienceItem;
public record GetVolunteeringOrWorkExperienceItemQuery : IRequest<GetVolunteeringOrWorkExperienceItemQueryResult>
{
    public Guid Id { get; init; }
    public Guid ApplicationId { get; init; }
    public Guid CandidateId { get; init; }
}
