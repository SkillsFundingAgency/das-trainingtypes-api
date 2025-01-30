using MediatR;
using SFA.DAS.CandidateAccount.Data.Candidate;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Commands.DeleteCandidate
{
    public record DeleteCandidateCommandHandler(ICandidateRepository CandidateRepository)
        : IRequestHandler<DeleteCandidateCommand, DeleteCandidateCommandResult>
    {
        public async Task<DeleteCandidateCommandResult> Handle(DeleteCandidateCommand command, CancellationToken cancellationToken)
        {
            var result = await CandidateRepository.DeleteCandidate(command.CandidateId);

            if (result.Item2)
            {
                return new DeleteCandidateCommandResult
                {
                    Candidate = result.Item1,
                };
            }

            return new DeleteCandidateCommandResult
            {
                Candidate = null
            };
        }
    }
}
