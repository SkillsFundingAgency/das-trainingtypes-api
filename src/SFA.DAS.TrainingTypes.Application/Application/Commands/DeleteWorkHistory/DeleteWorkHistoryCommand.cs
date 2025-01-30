using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.DeleteWorkHistory
{
    public record DeleteWorkHistoryCommand : IRequest<Unit>
    {
        public Guid JobId { get; init; }
        public Guid ApplicationId { get; init; }
        public Guid CandidateId { get; init; }
    }
}
