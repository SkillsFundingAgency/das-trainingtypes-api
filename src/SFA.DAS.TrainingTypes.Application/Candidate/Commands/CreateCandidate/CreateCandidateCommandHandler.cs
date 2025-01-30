using MediatR;
using SFA.DAS.CandidateAccount.Data.Candidate;
using SFA.DAS.CandidateAccount.Data.CandidatePreferences;
using SFA.DAS.CandidateAccount.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Commands.CreateCandidate;

public class CreateCandidateCommandHandler(ICandidateRepository candidateRepository, ICandidatePreferencesRepository candidatePreferencesRepository)
    : IRequestHandler<CreateCandidateCommand, CreateCandidateCommandResponse>
{
    public async Task<CreateCandidateCommandResponse> Handle(CreateCandidateCommand command, CancellationToken cancellationToken)
    {
        var result = await candidateRepository.Insert(new CandidateEntity
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            GovUkIdentifier = command.GovUkIdentifier,
            CreatedOn = DateTime.UtcNow,
            DateOfBirth = command.DateOfBirth,
            MigratedEmail = command.MigratedEmail,
            MigratedCandidateId = command.MigratedCandidateId,
            PhoneNumber = command.PhoneNumber
        });

        if (result.Item2)
        {
            await candidatePreferencesRepository.Create(result.Item1.Id);
        }

        return new CreateCandidateCommandResponse
        {
            Candidate = result.Item1!
        };
    }
}