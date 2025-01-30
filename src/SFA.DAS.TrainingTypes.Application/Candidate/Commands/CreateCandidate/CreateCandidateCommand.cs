using MediatR;

namespace SFA.DAS.TrainingTypes.Application.Candidate.Commands.CreateCandidate;

public class CreateCandidateCommand : IRequest<CreateCandidateCommandResponse>
{
    public required string GovUkIdentifier { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? MigratedEmail { get; set; }
    public Guid? MigratedCandidateId { get; set; }
    public string? PhoneNumber { get; set; }
}