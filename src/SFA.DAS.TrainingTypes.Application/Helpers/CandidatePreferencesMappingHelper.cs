using SFA.DAS.TrainingTypes.Application.CandidatePreferences.Queries.GetCandidatePreferences;
using SFA.DAS.TrainingTypes.Domain.Candidate;
using static SFA.DAS.TrainingTypes.Application.CandidatePreferences.Queries.GetCandidatePreferences.GetCandidatePreferencesQueryResult;

namespace SFA.DAS.TrainingTypes.Application.Helpers;
public static class CandidatePreferencesMappingHelper
{
    public static GetCandidatePreferencesQueryResult Map
        (List<PreferenceEntity> preferences, List<CandidatePreferencesEntity> candidatePreferences)
    {
        //Joins preferences and candidatePreferences.
        var groupedQuery = (from p in preferences
                            join cp in candidatePreferences
                            on p.PreferenceId equals cp.PreferenceId into gj
                            from cp in gj.DefaultIfEmpty()
                            group new { p, cp } by p.PreferenceId into grouped
                            select new GetCandidatePreferencesQueryResult.CandidatePreference
                            {
                                PreferenceId = grouped.Key,
                                PreferenceMeaning = grouped.First().p.PreferenceMeaning,
                                PreferenceHint = grouped.First().p.PreferenceHint,
                                ContactMethodsAndStatus = grouped.Select(x => new ContactMethodStatus
                                {
                                    ContactMethod = x.cp?.ContactMethod,
                                    Status = x.cp?.Status
                                }).ToList()
                            }).ToList();

        //Adds preferences that did not match any candidatePreferences in the join.
        var allPreferences = preferences.Select(p => p.PreferenceId).ToList();
        var missingPreferences = allPreferences.Except(groupedQuery.Select(gp => gp.PreferenceId).ToList()).ToList();
        var missingPreferenceResults = missingPreferences.Select(p => new GetCandidatePreferencesQueryResult.CandidatePreference
        {
            PreferenceId = p,
            PreferenceMeaning = preferences.FirstOrDefault(preference => preference.PreferenceId == p)?.PreferenceMeaning,
            PreferenceHint = preferences.FirstOrDefault(preference => preference.PreferenceId == p)?.PreferenceHint,
            ContactMethodsAndStatus = new List<ContactMethodStatus>()
        }).ToList();

        var result = groupedQuery.Concat(missingPreferenceResults).ToList();

        return new GetCandidatePreferencesQueryResult { CandidatePreferences = result };
    }
}
