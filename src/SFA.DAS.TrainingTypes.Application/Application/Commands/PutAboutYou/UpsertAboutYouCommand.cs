using MediatR;
using SFA.DAS.TrainingTypes.Domain.Candidate;

namespace SFA.DAS.TrainingTypes.Application.Application.Commands.PutAboutYou;
public class UpsertAboutYouCommand : IRequest<UpsertAboutYouCommandResult>
{
    public Guid CandidateId { get; set; }
    public AboutYou AboutYou { get; set; }
}
