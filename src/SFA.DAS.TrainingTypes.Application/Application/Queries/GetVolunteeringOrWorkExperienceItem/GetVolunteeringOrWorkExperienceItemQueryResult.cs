using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetVolunteeringOrWorkExperienceItem;
public record GetVolunteeringOrWorkExperienceItemQueryResult
{
    public Guid Id { get; private init; }
    public string? Employer { get; private init; }
    public DateTime StartDate { get; private init; }
    public DateTime? EndDate { get; private init; }
    public Guid ApplicationId { get; private init; }
    public string? Description { get; private init; }

    public static implicit operator GetVolunteeringOrWorkExperienceItemQueryResult(WorkHistoryEntity source)
    {
        return new GetVolunteeringOrWorkExperienceItemQueryResult
        {
            Id = source.Id,
            ApplicationId = source.ApplicationId,
            Description = source.Description,
            Employer = source.Employer,
            EndDate = source.EndDate,
            StartDate = source.StartDate
        };
    }
}
