using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Commands.UpsertCandidate;

public class UpsertCandidateCommand : IRequest<UpsertCandidateCommandResponse>
{
    public required Domain.Candidate.Candidate Candidate { get; set; }
}