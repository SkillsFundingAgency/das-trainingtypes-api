using MediatR;
using SFA.DAS.CandidateAccount.Data.Candidate;
using SFA.DAS.CandidateAccount.Data.CandidatePreferences;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Commands.UpsertCandidate;

public class UpsertCandidateCommandHandler(ICandidateRepository candidateRepository, ICandidatePreferencesRepository candidatePreferencesRepository)
    : IRequestHandler<UpsertCandidateCommand, UpsertCandidateCommandResponse>
{
    public async Task<UpsertCandidateCommandResponse> Handle(UpsertCandidateCommand request, CancellationToken cancellationToken)
    {
        var result = await candidateRepository.UpsertCandidate(request.Candidate);

        if (result.Item2)
        {
            await candidatePreferencesRepository.Create(result.Item1.Id);
        }

        return new UpsertCandidateCommandResponse
        {
            Candidate = result.Item1,
            IsCreated = result.Item2
        };
    }
}