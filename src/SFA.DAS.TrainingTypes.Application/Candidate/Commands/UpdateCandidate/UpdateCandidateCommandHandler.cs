using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Commands.UpdateCandidate;

public class UpdateCandidateCommandHandler : IRequestHandler<UpdateCandidateCommand, UpdateCandidateCommandResponse>
{
    public async Task<UpdateCandidateCommandResponse> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}