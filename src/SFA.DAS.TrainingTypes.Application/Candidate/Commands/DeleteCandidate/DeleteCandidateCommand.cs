using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Commands.DeleteCandidate
{
    public record DeleteCandidateCommand(Guid CandidateId) : IRequest<DeleteCandidateCommandResult>;
}
