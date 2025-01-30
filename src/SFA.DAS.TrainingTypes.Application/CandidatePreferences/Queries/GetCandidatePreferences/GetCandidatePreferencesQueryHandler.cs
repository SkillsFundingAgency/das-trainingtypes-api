using MediatR;
using SFA.DAS.CandidateAccount.Data.CandidatePreferences;
using SFA.DAS.CandidateAccount.Data.Preference;
using SFA.DAS.TrainingTypes.Application.Helpers;

namespace SFA.DAS.TrainingTypes.Application.CandidatePreferences.Queries.GetCandidatePreferences;
public class GetCandidatePreferencesQueryHandler : IRequestHandler<GetCandidatePreferencesQuery, GetCandidatePreferencesQueryResult>
{
    private readonly ICandidatePreferencesRepository _candidatePreferencesRepository;
    private readonly IPreferencesRepository _preferencesRepository;

    public GetCandidatePreferencesQueryHandler(ICandidatePreferencesRepository candidatePreferencesRepository, IPreferencesRepository preferencesRepository)
    {
        _candidatePreferencesRepository = candidatePreferencesRepository;
        _preferencesRepository = preferencesRepository;
    }

    public async Task<GetCandidatePreferencesQueryResult> Handle(GetCandidatePreferencesQuery request, CancellationToken cancellationToken)
    {
        var preferences = await _preferencesRepository.GetAll();
        var candidatePreferences = await _candidatePreferencesRepository.GetAllByCandidate(request.CandidateId);

        if (candidatePreferences.Count.Equals(0))
        {
            return new GetCandidatePreferencesQueryResult()
            {
                CandidatePreferences = preferences.Select(p => new GetCandidatePreferencesQueryResult.CandidatePreference()
                {
                    PreferenceId = p.PreferenceId,
                    PreferenceMeaning = p.PreferenceMeaning,
                    PreferenceHint = p.PreferenceHint,
                    ContactMethodsAndStatus = new List<GetCandidatePreferencesQueryResult.ContactMethodStatus>()
                }).ToList()
            };
        }
        else
        {
            var result = CandidatePreferencesMappingHelper.Map(preferences, candidatePreferences);
            return result;
        }
    }
}
