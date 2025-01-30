using MediatR;
using SFA.DAS.CandidateAccount.Data.CandidatePreferences;

namespace SFA.DAS.TrainingTypes.Application.CandidatePreferences.Commands.PutCandidatePreferences;
public class PutCandidatePreferencesCommandHandler : IRequestHandler<PutCandidatePreferencesCommand, PutCandidatePreferencesCommandResult>
{
    private readonly ICandidatePreferencesRepository _candidatePreferencesRepository;

    public PutCandidatePreferencesCommandHandler(ICandidatePreferencesRepository candidatePreferencesRepository)
    {
        _candidatePreferencesRepository = candidatePreferencesRepository;
    }

    public async Task<PutCandidatePreferencesCommandResult> Handle(PutCandidatePreferencesCommand request, CancellationToken cancellationToken)
    {
        var result = await _candidatePreferencesRepository.Upsert(request.CandidatePreferences);

        return (PutCandidatePreferencesCommandResult)result;
    }
}
