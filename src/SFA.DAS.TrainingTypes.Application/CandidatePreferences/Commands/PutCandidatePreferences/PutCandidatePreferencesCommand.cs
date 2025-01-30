using MediatR;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.CandidatePreferences.Commands.PutCandidatePreferences;
public class PutCandidatePreferencesCommand : IRequest<PutCandidatePreferencesCommandResult>
{
    public List<CandidatePreference> CandidatePreferences { get; set; }
}
