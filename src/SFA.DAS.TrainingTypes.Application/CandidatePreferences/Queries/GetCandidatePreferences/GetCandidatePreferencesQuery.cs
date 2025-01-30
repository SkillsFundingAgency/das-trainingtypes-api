using MediatR;

namespace SFA.DAS.TrainingTypes.Application.CandidatePreferences.Queries.GetCandidatePreferences;
public class GetCandidatePreferencesQuery : IRequest<GetCandidatePreferencesQueryResult>
{
    public Guid CandidateId { get; set; }
}
