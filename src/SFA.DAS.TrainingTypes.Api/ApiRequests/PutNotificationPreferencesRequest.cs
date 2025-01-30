namespace SFA.DAS.TrainingTypes.Api.ApiRequests;

public class PutCandidatePreferencesRequest
{
    public List<NotificationPreference> CandidatePreferences { get; set; }

    public class NotificationPreference
    {
        public Guid PreferenceId { get; set; }
        public bool? Status { get; set; }
        public string ContactMethod { get; set; }
    }
}
