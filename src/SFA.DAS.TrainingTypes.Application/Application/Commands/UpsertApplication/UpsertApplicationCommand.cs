using MediatR;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertApplication;

public record UpsertApplicationCommand : IRequest<UpsertApplicationCommandResponse>
{
    public required string VacancyReference { get; init; }
    public Guid CandidateId { get; init; }
    public ApplicationStatus Status { get; init; }
    public SectionStatus? IsDisabilityConfidenceComplete { get; init; }
    public SectionStatus? IsApplicationQuestionsComplete { get; init; }
    public SectionStatus? IsEducationHistoryComplete { get; init; }
    public SectionStatus? IsInterviewAdjustmentsComplete { get; init; }
    public SectionStatus? IsWorkHistoryComplete { get; init; }
    public SectionStatus? IsAdditionalQuestion1Complete { get; init; }
    public SectionStatus? IsAdditionalQuestion2Complete { get; init; }
    public string? DisabilityStatus { get; init; }
    public List<string?> AdditionalQuestions { get; init; } = [];
}