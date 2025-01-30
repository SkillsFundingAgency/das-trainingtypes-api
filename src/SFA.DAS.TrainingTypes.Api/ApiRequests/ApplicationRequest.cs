using System.ComponentModel.DataAnnotations;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Api.ApiRequests;

public class ApplicationRequest
{
    [Required]
    public required Guid CandidateId { get; set; }

    public ApplicationStatus Status { get; set; }
    public string? DisabilityStatus { get; set; }
    public SectionStatus IsApplicationQuestionsComplete { get; set; }
    public SectionStatus IsDisabilityConfidenceComplete { get; set; }
    public SectionStatus IsEducationHistoryComplete { get; set; }
    public SectionStatus IsInterviewAdjustmentsComplete { get; set; }
    public SectionStatus IsWorkHistoryComplete { get; set; }
    public SectionStatus IsAdditionalQuestion1Complete { get; set; }
    public SectionStatus IsAdditionalQuestion2Complete { get; set; }
    public IEnumerable<string?> AdditionalQuestions { get; set; } = [];
}