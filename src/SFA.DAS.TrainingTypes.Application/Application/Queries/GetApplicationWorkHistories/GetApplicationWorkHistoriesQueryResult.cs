using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Queries.GetApplicationWorkHistories;

public record GetApplicationWorkHistoriesQueryResult
{
    public IEnumerable<WorkHistory> WorkHistories { get; init; } = [];

    public static implicit operator GetApplicationWorkHistoriesQueryResult(List<WorkHistoryEntity> source)
    {
        return new GetApplicationWorkHistoriesQueryResult
        {
            WorkHistories = source.Select(entity => (WorkHistory)entity)
        };
    }

}