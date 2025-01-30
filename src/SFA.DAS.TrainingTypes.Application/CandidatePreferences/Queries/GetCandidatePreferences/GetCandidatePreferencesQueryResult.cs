namespace SFA.DAS.TrainingTypes.Application.CandidatePreferences.Queries.GetCandidatePreferences;
public class GetCandidatePreferencesQueryResult
{
    public List<CandidatePreference> CandidatePreferences { get; set; }

    public class CandidatePreference()
    {
        public Guid PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; }
        public string PreferenceHint { get; set; }
        public List<ContactMethodStatus>? ContactMethodsAndStatus { get; set; }
    }

    public class ContactMethodStatus()
    {
        public string? ContactMethod { get; set; }
        public bool? Status { get; set; }
    }
}
