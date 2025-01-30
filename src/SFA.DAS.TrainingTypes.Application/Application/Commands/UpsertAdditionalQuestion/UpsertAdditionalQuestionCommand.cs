using MediatR;
using SFA.DAS.TrainingTypes.Domain.Application;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.UpsertAdditionalQuestion;

public record UpsertAdditionalQuestionCommand : IRequest<UpsertAdditionalQuestionCommandResponse>
{
    public required TrainingType AdditionalQuestion { get; init; }
    public required Guid CandidateId { get; init; }
}